using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// --- NEW CLASS FOR TRACKING FRAME SCORES ---
// This private class defines what information is stored for each of the 10 frames.
[System.Serializable]
public class FrameScore
{
    public int roll1 = -1; // -1 indicates roll hasn't happened yet
    public int roll2 = -1;
    public int score = 0; // The final calculated score for this frame (including bonuses)
    public bool isStrike => roll1 == 10;
    public bool isSpare => roll1 != 10 && roll1 + roll2 == 10;
    public bool IsFrameComplete => roll1 != -1 && (isStrike || roll2 != -1);
}

public class BowlingManager : MonoBehaviour
{
    // UNIVERSAL SCORE MANAGER
    public ScoreManager scoreManager;
    public WinScreen winScreen;

    public GameObject ball;
    public GameObject bowlingScreen;
    
    // SCORE TRACKING
    public List<FrameScore> frames = new List<FrameScore>();
    public int currentFrameIndex = 0; // 0 to 9
    public int currentRollInFrame = 1; // 1 or 2
    public int maxFrames = 10;
    
    // UI ELEMENTS
    public TMP_Text scoreUI;
    public TMP_Text roundsUI; // Now shows current frame
    public TMP_Text totalScoreUI;
    
    // DEPENDENCY REFERENCES
    public CameraSwitch cameraSwitch;
    public VideoPlayerScript videoPlayerScript;
    public CanvasPathFollwer canvasPathFollwer;

    private GameObject[] pins;
    private Vector3[] initialPinPositions;

    private const int TOTAL_PINS = 10;
    private int pinsDownThisRoll = 0;
    private int totalScore = 0;
    
    // Store reference to BowlingBall component for efficiency
    private BowlingBall ballComponent; 

    void Start()
    {
        // Cache the BowlingBall component
        ballComponent = ball.GetComponent<BowlingBall>();

        // Tracks game objs with pin tag for the ball to hit in their current positions
        pins = GameObject.FindGameObjectsWithTag("Pin");
        initialPinPositions = new Vector3[pins.Length];

        for (int i = 0; i < pins.Length; i++)
        {
            initialPinPositions[i] = pins[i].transform.position;
        }

        // Initialize frames
        for (int i = 0; i < maxFrames; i++)
        {
            frames.Add(new FrameScore());
        }
        UpdateRoundsUI();
    }

    void Update()
    {
        // Cache the Rigidbody component
        Rigidbody rb = ball.GetComponent<Rigidbody>();
        
        // Horizontal movement before launch
        if (!ballComponent.hasLaunched)
            MoveBall();

        // Launch logic: Ball has stopped (IsSleeping) or fallen off the lane
        if (ballComponent.hasLaunched && (ball.transform.position.y < -20 || rb.IsSleeping()))
        {
            CountPinsDown();
            ProcessRoll();
            ResetBall();
        }

        // Stuck logic (if the ball hits a collider that sets its tag to "Stuck")
        if (ballComponent.hasLaunched && (ball.tag == "Stuck"))
        {
            // If the ball gets stuck, we still need to process the roll count
            CountPinsDown(); 
            ProcessRoll();
            ResetBall();
            ball.tag = "BowlingBall"; // Reset tag
        }
    }

    void MoveBall()
    {
        // Horizontal movement before launch
        Vector3 pos = ball.transform.position;
        pos += Vector3.right * Input.GetAxis("Horizontal") * Time.deltaTime;
        pos.x = Mathf.Clamp(pos.x, -0.525f, 0.525f); // Adjust lane bounds
        ball.transform.position = pos;
    }

    public void CountPinsDown()
    {
        pinsDownThisRoll = 0;
        
        // Tracks pins knocked down for scoring
        for (int i = 0; i < pins.Length; i++)
        {
            // Check if the pin is active and if its rotation indicates it has fallen
            // We use the Dot Product check for robustness (pin's up vector vs world up vector)
            bool pinFallen = Vector3.Dot(pins[i].transform.up, Vector3.up) < 0.9f; 

            if (pins[i].activeSelf && pinFallen)
            {
                pinsDownThisRoll++;
                pins[i].SetActive(false);
            }
        }
        
        // Update the temporary score display
        scoreUI.text = pinsDownThisRoll.ToString();
    }

    void ProcessRoll()
{
    FrameScore currentFrame = frames[currentFrameIndex];

    // Store the roll result
    if (currentRollInFrame == 1)
    {
        currentFrame.roll1 = pinsDownThisRoll;

        if (currentFrame.isStrike)
        {
            Debug.Log("STRIKE X");
            // Frame ends on a strike. Reset all pins and advance frame.
            ResetPins(); 
            AdvanceFrame();
        }
        else // Open Frame - Set up for Roll 2
        {
            currentRollInFrame = 2; // Move to the second roll
            // Do NOT call ResetPins() here. The downed pins are correctly disabled.
            // Only ResetBall() will be called next, preparing for the second shot at the remaining pins.
        }
    }
    else // currentRollInFrame == 2 (Roll 2 of the frame)
    {
        currentFrame.roll2 = pinsDownThisRoll;

        if (currentFrame.isSpare)
        {
            Debug.Log("SPARE /");
        }

        // Frame is COMPLETE after Roll 2 (Spare or Open). Reset all pins and advance frame.
        ResetPins();
        AdvanceFrame();
        currentRollInFrame = 1; // Reset roll counter for the next frame
    }

    CalculateScore(currentFrameIndex);
    
    // Update the global score display
    totalScoreUI.text = $"{totalScore}";
}
    
    void CalculateScore(int frameIndex)
    {
        totalScore = 0;

        for (int i = 0; i <= frameIndex; i++)
        {
            FrameScore frame = frames[i];
            
            if (!frame.IsFrameComplete) continue;

            int frameBaseScore = frame.roll1 + frame.roll2;

            if (frame.isStrike)
            {
                frame.score = 10;
                // STRIKE BONUS: + next two rolls
                if (i + 1 < maxFrames && frames[i+1].roll1 != -1)
                {
                    frame.score += frames[i+1].roll1;
                    
                    if (frames[i+1].isStrike && i + 2 < maxFrames)
                    {
                        // Double strike: need roll 1 of frame i+2
                        frame.score += frames[i+2].roll1;
                    }
                    else if (frames[i+1].roll2 != -1)
                    {
                        // Standard score: need roll 2 of frame i+1
                        frame.score += frames[i+1].roll2;
                    }
                }
            }
            else if (frame.isSpare)
            {
                frame.score = 10;
                // SPARE BONUS: + next one roll
                if (i + 1 < maxFrames && frames[i+1].roll1 != -1)
                {
                    frame.score += frames[i+1].roll1;
                }
            }
            else
            {
                // Open Frame: base score is final score
                frame.score = frameBaseScore;
            }
            
            // Only add points if the score is fully calculated (not waiting for bonus)
            if (frame.score > frameBaseScore)
            {
                totalScore += frame.score;
            }
            else
            {
                totalScore += frameBaseScore;
            }
        }
        
        // Update Universal Score Manager (simplified: adding points based on pins down)
        // You might want to pass totalScore instead of pinsDownThisRoll
        scoreManager.AddPoints(pinsDownThisRoll * 10);
    }
    
    void AdvanceFrame()
    {
        currentFrameIndex++;
        UpdateRoundsUI();

        if (currentFrameIndex >= maxFrames)
        {
            EndGame();
        }
    }
    
    void EndGame()
    {
        cameraSwitch.camera2.SetActive(false);
        cameraSwitch.camera1.SetActive(true);
        winScreen.DisplayWinResults();
    }


    public void ResetPins()
    {
        // Resets the pins into their original spots & resets the collision motion
        for (int i = 0; i < pins.Length; i++)
        {
            pins[i].SetActive(true);
            pins[i].transform.position = initialPinPositions[i];
            pins[i].GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
            pins[i].GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            pins[i].transform.rotation = Quaternion.identity;
        }
    }

    public void ResetBall()
    {
        // Resets ball into original position + resets motion
        ball.transform.position = new Vector3(0, 0.108f, -4f);
        ball.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
        ball.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        ball.transform.rotation = Quaternion.identity;
        ballComponent.hasLaunched = false; // allows launch again
        ballComponent.spinDirection = 0f;

        // Swaps cameras on and off after reset
        cameraSwitch.camera1.SetActive(true);
        cameraSwitch.camera2.SetActive(false);
        cameraSwitch.gameObject.SetActive(true);

        // Handle video playback
        if (canvasPathFollwer.currentWaypointIndex == canvasPathFollwer.waypoints.Length - 1)
        {
            videoPlayerScript.SelectVideoClip(pinsDownThisRoll);
            StartCoroutine(videoPlayerScript.PlayVideoAndStop());
        }
    }

    void UpdateRoundsUI()
    {
        // Display Frame Number (1-10) and Roll Number (1 or 2)
        roundsUI.text = $"Frame: {currentFrameIndex + 1}\nRoll: {currentRollInFrame}";
    }
}