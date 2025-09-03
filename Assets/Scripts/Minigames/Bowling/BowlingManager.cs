using UnityEngine;

public class BowlingManager : MonoBehaviour
{
    // Move the ball
    // Manage the score
    // Manage the turns

    public GameObject ball;
    public int score = 0;
    GameObject[] pins;

    private bool bowling = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        pins = GameObject.FindGameObjectsWithTag("Pin");
    }

    // Update is called once per frame
    void Update()
    {
        MoveBall();

        // Launch the ball
            if (Input.GetButtonDown("Fire1") || ball.transform.position.y < -20) // Default is Left Mouse Button or Ctrl
            {
                CountPinsDown();
            }
    }

    void MoveBall()
    {
        if (!bowling)
        {
            // Horizontal movement before launch
            Vector3 pos = ball.transform.position;
            float horizontalInput = Input.GetAxis("Horizontal");
            pos += Vector3.right * horizontalInput * Time.deltaTime;
            pos.x = Mathf.Clamp(pos.x, -0.525f, 0.525f); // Adjust lane bounds
            ball.transform.position = pos;
        }
    }

    void CountPinsDown()
    {
        for (int i = 0; i < pins.Length; i++)
        {
            if (pins[i].transform.eulerAngles.z > 5 && pins[i].transform.eulerAngles.z < 355)
            {
                score++;
            }
        }

        Debug.Log(score);
    }
}
