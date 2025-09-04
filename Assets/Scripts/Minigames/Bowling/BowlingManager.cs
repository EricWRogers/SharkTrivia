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
    int turnCounter = 0;
    GameObject[] pins;
    public TMP_Text scoreUI;
    public CameraSwitch cameraSwitch;

    Vector3[] positions;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        pins = GameObject.FindGameObjectsWithTag("Pin");
        positions = new Vector3[pins.Length];

        for (int i = 0; i < pins.Length; i++)
        {
            positions[i] = pins[i].transform.position;
        }
    }

    // Update is called once per frame
    void Update()
    {
        MoveBall();

        // Launch the ball
        if (Input.GetKeyDown(KeyCode.Space) || ball.transform.position.y < -20) // Default is Left Mouse Button or Ctrl
        {
            CountPinsDown();
            turnCounter++;
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

    void ResetPins()
    {
        for (int i = 0; i < pins.Length; i++)
        {
            pins[i].SetActive(true);
            pins[i].transform.position = positions[i];
            pins[i].GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
            pins[i].GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            pins[i].transform.rotation = Quaternion.identity;
        }

        ball.transform.position = new Vector3(0, 0.108f, -4f);
        ball.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
        ball.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        ball.transform.rotation = Quaternion.identity;

        cameraSwitch.camera1.SetActive(true);
        cameraSwitch.camera2.SetActive(false);
    }
}
