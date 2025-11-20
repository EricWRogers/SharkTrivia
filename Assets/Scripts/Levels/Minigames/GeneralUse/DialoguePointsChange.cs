using UnityEngine;

public class DialoguePointsChange : MonoBehaviour
{
    public GameObject scoreM;
    TotalScore _ts;
    void Start()
    {
        scoreM = GameObject.Find("TotalScore");
        _ts = scoreM.GetComponent<TotalScore>();
    }

    public void AddPoints(int points)
    {
        Instantiate(this.gameObject);

        Debug.Log("Adding points.. " + points);
        _ts.totalScore += points;
        _ts.SaveScore();
    }
    public void TakePoints(int points)
    {
        Instantiate(this.gameObject);

        Debug.Log("Removing points.. -" + points);
        _ts.totalScore -= points;
        _ts.SaveScore();
    }
}
