using UnityEngine;
using TMPro;
public class TimerTrigger : MonoBehaviour
{
    public GameObject timerM;
    Timer timer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        timerM = GameObject.Find("TimeManager");
        timer = timerM.GetComponent<Timer>();
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
        timer.timeRunning = true;
    }
}
