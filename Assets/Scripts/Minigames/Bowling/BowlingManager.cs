using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class BowlingManager : MonoBehaviour
{
    // Move the ball
    // Manage the score
    // Manage the turns

    public GameObject ball;
    public int score = 0;
    public int realScore = 0;
    public int rounds = 0;
    private bool pinsUp = true;
    GameObject[] pins;
    public TMP_Text scoreUI;
    public TMP_Text roundsUI;
    public CameraSwitch cameraSwitch;
    public GameOverManager gameOverManager;
    public VideoPlayerScript videoPlayerScript;

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

        if (ball.GetComponent<BowlingBall>().hasLaunched && (ball.transform.position.y < -20 || rb.IsSleeping()))
        {
            CountPinsDown();
            NewRound();
            ResetPins();
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
        // Tracks pins knocked down for scoring
        for (int i = 0; i < pins.Length; i++)
        {
            if (pins[i].transform.eulerAngles.z > 5 && pins[i].transform.eulerAngles.z < 355 && pins[i].activeSelf)
            {
                score++;
                pins[i].SetActive(false);
                realScore = score;
            }
            pinsUp = false;
        }
        realScore = score % 10; // Resets score to 0 after 10 for video purposes
        videoPlayerScript.SelectVideoClip(realScore);
        StartCoroutine(videoPlayerScript.PlayVideoAndStop());
        scoreUI.text = score.ToString();
        
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
        // Resets ball into original position + resets motion
        ball.transform.position = new Vector3(0, 0.108f, -4f);
        ball.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
        ball.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        ball.transform.rotation = Quaternion.identity;

        BowlingBall bowlingBall = ball.GetComponent<BowlingBall>();
        if (bowlingBall != null)
        {
            bowlingBall.ResetBall();
        }

        // Swaps cameras on and off after reset
        cameraSwitch.camera1.SetActive(true);
        cameraSwitch.camera2.SetActive(false);

        cameraSwitch.gameObject.SetActive(true);

        // Resets Power bar for next throw
        PowerBar powerBar = FindAnyObjectByType<PowerBar>();
        if (powerBar != null)
        {
            powerBar.ResetBar();
        }
    }
    
    
    public void NewRound() //Updates the round counter
    {
        if (pinsUp == false && rounds < 3)// If the pins are up and the round is not 3, the game continues.
        {
            rounds++;
        }
        else
        {
            cameraSwitch.camera2.SetActive(false);
            cameraSwitch.camera1.SetActive(true);
            if (gameOverManager == null) { Debug.Log("NNNNN"); }
            gameOverManager.GameOverShow();
        }
        roundsUI.text = rounds.ToString();
    }
}
