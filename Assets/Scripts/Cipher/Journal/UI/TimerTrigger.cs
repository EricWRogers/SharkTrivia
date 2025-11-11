using UnityEngine;
using TMPro;
public class TimerTrigger : MonoBehaviour
{
    public GameObject timerM;
    Timer timer;

    bool timeHasStarted;
    bool isTimeUp;

    public DNode nodeOnTimeout;

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Awake()
    {
        isTimeUp = false;
    }
    void Start()
    {
        timerM = GameObject.Find("TimeManager");
        timer = timerM.GetComponent<Timer>();
    }

    private void Update()
    {

        if (!isTimeUp)
        {
            //Debug.Log("Time has begun");

            if (!timer.timeRunning)
            {
                TimeUp();
                isTimeUp = true;
            }
        }
    }

    public void SpawnTimer()
    {
        Instantiate(this.gameObject);

        timerM = GameObject.Find("TimeManager");
        timer = timerM.GetComponent<Timer>();

        StartTimer();
    }

    public void StartTimer()
    {
        Debug.Log("Hello! I have been spawned!");

        timer.timeRunning = true;  
    }

    public void StopTimer()
    {
        Debug.Log("Goodbye! I have been stopped!");
        timer.timeRunning = false;
    }

    public void TimeUp()
    {
        Debug.Log("Pencils down! Time is up!");

        enabled = false;

        var dm = DialogueManagerIntegrated.Instance;
        if (!dm || !nodeOnTimeout) return;
        dm.JumpToNode(nodeOnTimeout);
    }
}
