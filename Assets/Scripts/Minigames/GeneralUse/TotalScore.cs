using UnityEngine;

public class TotalScore : MonoBehaviour
{
    public static TotalScore instance;
    private int totalScore = 0;

    private void Awake()
    {
        if(instance==null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            LoadScore();
        }
        else
        {
            Destroy(gameObject);

        }
    }
    public void AddPoints(int points)
    {
        totalScore += points;
        SaveScore();
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
