using UnityEngine;

public class DialoguePointsChange : MonoBehaviour
{
    public void AddPoints(int points)
    {
        Debug.Log("Adding points.. " + points);
        ScoreManager.instance.AddPoints(points);
    }
    public void TakePoints(int points)
    {
        ScoreManager.instance.AddPoints(-points);
        Debug.Log("Removing points.. -" + points);
    }
}
