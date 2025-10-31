using UnityEngine;
using UnityEngine.Events;

public class TotalScore : MonoBehaviour
{
    public static TotalScore instance;
    public int totalScore = 0;
    public float wager = 0.0f;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            //LoadScore();
        }
        else
        {
            Destroy(gameObject);

        }
    }
    public void PlayerLost()
    {
        totalScore -= (int) wager;
    }
    public void AddPoints(int points)
    {
        totalScore += points + (2 * (int) wager);
        //SaveScore();
    }

    public int GetScore()
    {
        return totalScore;
    }

    private void SaveScore(){
        PlayerPrefs.SetInt("TotalScore",totalScore);
        PlayerPrefs.Save();
    }
    
    private void LoadScore(){
        totalScore = PlayerPrefs.GetInt("TotalScore",0);
    }
}