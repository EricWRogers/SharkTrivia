using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class BowlingManager : MonoBehaviour
{
    // Move the ball
    // Manage the score
    // Manage the turns

    //UNIVERSAL SCORE MANAGER
    public ScoreManager scoreManager;
    public WinScreen winScreen;

    public GameObject ball;
    public int score = 0;
    public int totalScore = 0;
    public int maxRounds = 6;
    public int roundsPlayed = 0;
    private int bonus = 10;
    private bool pinsUp = false;
    GameObject[] pins;
    public TMP_Text scoreUI;
    public TMP_Text roundsUI;
    public TMP_Text totalScoreUI;
    public CameraSwitch cameraSwitch;
    public VideoPlayerScript videoPlayerScript;
    public CanvasPathFollwer canvasPathFollwer; 

    Vector3[] positions;

    void Start()
    {
        // Tracks game objs with pin tag for the ball to hit in their current positions
        pins = GameObject.FindGameObjectsWithTag("Pin");
        positions = new Vector3[pins.Length];

        for (int i = 0; i < pins.Length; i++)
        {
            positions[i] = pins[i].transform.position;
        }
    }

    void Update()
    {
        if (!ball.GetComponent<BowlingBall>().hasLaunched) // Only move before launch
            MoveBall();

        // Launch the ball
        Rigidbody rb = ball.GetComponent<Rigidbody>();
        if (roundsPlayed % 2 == 0 && !pinsUp)
        {
            ResetPins();
            pinsUp = true;
        }
        if (ball.GetComponent<BowlingBall>().hasLaunched && (ball.transform.position.y < -20 || rb.IsSleeping()))
        {
            CountPinsDown();
            ResetBall();
            NewRound();
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
        int pinsDownThisRound = 0;
        
        // Tracks pins knocked down for scoring
        for (int i = 0; i < pins.Length; i++)
        {
            if (pins[i].transform.eulerAngles.z > 5 && pins[i].transform.eulerAngles.z < 355 && pins[i].activeSelf)
            {
                pinsDownThisRound++;
                pins[i].SetActive(false);
            }
            pinsUp = false;
        }
        // Update total score and score managers
        scoreManager.AddPoints(pinsDownThisRound);  // UNIVERSAL SCORE MANAGER
        totalScore += pinsDownThisRound;

        // Handle video playback
        if (canvasPathFollwer.currentWaypointIndex == canvasPathFollwer.waypoints.Length - 1)
        {
            videoPlayerScript.SelectVideoClip(pinsDownThisRound);
            StartCoroutine(videoPlayerScript.PlayVideoAndStop());
        }

        // Update UI (using string interpolation)
        scoreUI.text = pinsDownThisRound.ToString();
        if(totalScoreUI != null)
            totalScoreUI.text = $"Total: {totalScore}";
    }

    public void ResetPins()
    {
        // Resets the pins into their original spots & resets the collision motion
        for (int i = 0; i < pins.Length; i++)
        {
            pins[i].SetActive(true);
            pins[i].transform.position = positions[i];
            pins[i].GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
            pins[i].GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            pins[i].transform.rotation = Quaternion.identity;
        }
        
        //OLD POWER BAR IMPLEMENTATION
        /*
        // Resets Power bar for next throw
        PowerBar powerBar = FindAnyObjectByType<PowerBar>();
        if (powerBar != null)
        {
            powerBar.ResetBar();
        }
        */
    }

    public void ResetBall()
    {
        BowlingBall bowlingBall = ball.GetComponent<BowlingBall>();
        // Resets ball into original position + resets motion
        ball.transform.position = new Vector3(0, 0.108f, -4f);
        ball.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
        ball.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        ball.transform.rotation = Quaternion.identity;
        bowlingBall.hasLaunched = false; // allows launch again
        bowlingBall.spinDirection = 0f;

        // Swaps cameras on and off after reset
        cameraSwitch.camera1.SetActive(true);
        cameraSwitch.camera2.SetActive(false);

        cameraSwitch.gameObject.SetActive(true);
    }

    public void NewRound() //Updates the round counter
    {
        roundsPlayed++;
        roundsUI.text = roundsPlayed.ToString();

        if (roundsPlayed >= maxRounds)
        {
            cameraSwitch.camera2.SetActive(false);
            cameraSwitch.camera1.SetActive(true);
            winScreen.ShowWinScreen();
        }
    }
}
