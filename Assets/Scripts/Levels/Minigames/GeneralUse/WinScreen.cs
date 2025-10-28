using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

// winScreen.DisplayWinResults();

public class WinScreen : MonoBehaviour
{
    public Timer timer;
    public ScoreManager totalScore;
    //public LevelManager levelManager;

    public int winThreshold = 100; //change this to be whatever the minimum anount of points needed to win

    public GameObject winScreen;    //the whole win screen

    //all the text in the win screen
    public TMP_Text winText;
    public TMP_Text winTime;
    public TMP_Text winStat;

    public void DisplayWinResults(int score)
    {    //whenever a minigame is over, call function
        int points = score;


        if (points >= winThreshold)
        {
            winText.text = "YOU WIN!";
            TotalScore.instance.AddPoints(score);
        }
        if (points < winThreshold)
        {
            winText.text = "you lose!";
            TotalScore.instance.PlayerLost();
        }

        winTime.text = "Time - " + timer.GetFormattedTime();
        winStat.text = "Score - " + points;
        winScreen.SetActive(true);
        
        ShowWinScreen();    //show the win screen when the minigame is over

        if (SceneManager.GetActiveScene().name == "SharkShootout")
        {
            StopGame();
        }
        if (SceneManager.GetActiveScene().name == "Bowling")
        {
            StopGame();
        }

    }


    public void DisplayLoseResults()
    {    //whenever the timer runs out
        TotalScore.instance.PlayerLost();
        winText.text = "you lose!";

        winTime.text = "Out of Time!";
        winStat.text = "Score - " + TotalScore.instance.totalScore;
        //winScreen.SetActive(true);
        ShowWinScreen();    //show the win screen when the minigame is over

        if (SceneManager.GetActiveScene().name == "SharkShootout")
        {
            StopGame();
        }
        if (SceneManager.GetActiveScene().name == "Bowling")
        {
            StopGame();
        }

    }

    public void ReturnButton()
    {
        LevelManager.LoadBackStage();   //temp for now

        //send player back to what ever scene they entered from.
    }

    public void HideWinScreen()
    {   //hide the win screen
        winScreen.SetActive(false);
    }
    public void ShowWinScreen()
    {   //show the win screen
        winScreen.SetActive(true);
    }

    public void StopGame()
    {
        Time.timeScale = 0;
    }
}
