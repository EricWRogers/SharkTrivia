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
    public int rounds = 0;
    GameObject[] pins;
    public  TMP_Text scoreUI;
    public TMP_Text roundsUI;

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
            if (Input.GetKeyDown(KeyCode.Space) || ball.transform.position.y < -20) // Default is Left Mouse Button or Ctrl
            {
                CountPinsDown();
            }
            else if(ball.transform.position.z < -30)
            {
                ball.SetActive(false);
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

    void CountPinsDown()
    {
        for (int i = 0; i < pins.Length; i++)
        {
            if (pins[i].transform.eulerAngles.z > 5 && pins[i].transform.eulerAngles.z < 355 && pins[i].activeSelf)
            {
                score++;
                pins[i].SetActive(false);
            }
        }

        scoreUI.text = score.ToString();
    }
    /*
    void NewRound() //WIP new round script.
    {
        if (!ball.gameObject.activeInHierarchy && rounds != 3) 
        {
            Instantiate(ball);
            ball.gameObject.SetActive(true);
        }
        roundsUI.text = rounds.ToString();
    }
    */
}
