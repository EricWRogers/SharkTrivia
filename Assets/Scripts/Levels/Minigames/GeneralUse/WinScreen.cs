using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

// winScreen.DisplayWinResults();
//just turn off the object in the scene (the prefab should automatically be off)

///////////// TO USE THE WIN SCREEN (it also works as a lose screen) /////////////
// winScreen.DisplayWinResults();
//just turn off the object in the scene (the prefab should automatically be off)

//////////// Win screen (lose screen) with the timer ////////////
// go to the "time manager" game object in the editor
// find Time out() 
// click add (+)
// drag the win screen into the space under the "Runtime Only" drop down
// select "WinScreen.DisplayLoseResults" in the second dropdown menu

public class WinScreen : MonoBehaviour
{
    public Timer timer;
    //public LevelManager levelManager;

    public int winThreshold = 100; //change this to be whatever the minimum anount of points needed to win

    public GameObject winScreen;    //the whole win screen

    //all the text in the win screen
    public TMP_Text winText;
    public TMP_Text winTime;
    public TMP_Text winStat;


    public void DisplayWinResults(int points)
    {
        //int points = ScoreManager.instance.score;

        if (points >= winThreshold)
        {
            winText.text = "YOU WIN!";
            //TotalScore.instance.AddPoints(points);
        }
        if (points < winThreshold)
        {
            winText.text = "you lose!";
            //TotalScore.instance.PlayerLost();
        }

        winTime.text = "Time - " + timer.GetFormattedTime();
        winStat.text = "Score - " + points.ToString();
        winScreen.SetActive(true);
        
        ShowWinScreen();    //show the win screen when the minigame is over

        //whenever a minigame is over, call function
        StopGame();

    }


    public void DisplayLoseResults()
    {   //whenever the timer runs out
        //TotalScore.instance.PlayerLost();
        winText.text = "you lose!";

        winTime.text = "Out of Time!";
        winStat.text = "";
        //winScreen.SetActive(true);
        ShowWinScreen();    //show the win screen when the minigame is over

    }

    public void ReturnButton()
    {
        LevelManager.StaticResume();
        LevelManager.LoadBackStage();   //temp for now

        //send player back to what ever scene they entered from.
    }

    public void HideWinScreen()
    {   //hide the win screen
        //winScreen.SetActive(false);
    }
    public void ShowWinScreen()
    {   //show the win screen
        winScreen.SetActive(true);
    }

    private void StopGame()
    {
        //Debug.Log(SceneManager.GetActiveScene().name);
        Time.timeScale = 0;
    }
}
