using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;

    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI highScoreText;
    int score;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        score = 0;
        UpdateHighScoreText();
        UpdateScoreText();

    }
    public void AddPoint()
    {
        AddPoints(1);
    }
    public void AddPoints(int amount)
    {
        score += amount;
        UpdateScoreText();
        CheckHighScore();


    }
    void CheckHighScore()
    {
        if(score > PlayerPrefs.GetInt("HighScore", 0))
        {
            PlayerPrefs.SetInt("HighScore", score);
            UpdateHighScoreText();
        }
    }
    void UpdateScoreText()
    {
        scoreText.text = $"Score: {score}"; 
    }
    void UpdateHighScoreText()
    {
        highScoreText.text = $"HighScore: {PlayerPrefs.GetInt("HighScore", 0)}";
    }
    public int GetScore(){
        return score;
    }
}
