using UnityEngine;

public class TimerStop : MonoBehaviour
{
    [Tooltip("This will ONLY work if you have a timer trigger object loaded in already")]
    public GameObject _ttObj;
    TimerTrigger _tt;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _ttObj = GameObject.Find("TimerTrigger");
        _tt = _ttObj.GetComponent<TimerTrigger>();
    }

    // this is soooo scuffed
    public void StopTimer()
    {
        Instantiate(this.gameObject);

        _ttObj = GameObject.Find("TimerTrigger");
        _tt = _ttObj.GetComponent<TimerTrigger>();

        _tt.StopTimer();   
    }
}
