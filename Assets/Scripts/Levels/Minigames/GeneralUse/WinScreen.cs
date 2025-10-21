using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

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

    void Start()
    {
        HideWinScreen();
    }



    public void DisplayWinResults(){    //whenever a minigame is over, call function
        int points = totalScore.GetScore();

        if(points >= winThreshold)
            winText.text = "YOU WIN!";
        if(points < winThreshold)
            winText.text = "you lose!";
        
        winTime.text = ("Time - " + timer.GetFormattedTime());
        winStat.text = ("Score - " + points);
        ShowWinScreen();    //show the win screen when the minigame is over
    }

    /////////////// THIS FUNCTION IS ONLY FOR WHEN A TIMER RUNS OUT //////////////
    public void DisplayLoseResults(){    //whenever the timer runs out
        int points = totalScore.GetScore();
        winText.text = "you lose!";
        
        winTime.text = "The timer ran out!";
        winStat.text = ("Score - " + points);
        ShowWinScreen();    //show the win screen when the minigame is over
    }

    public void ReturnButton(){
        LevelManager.LoadBackStage();   //temp for now

        //send player back to what ever scene they entered from.
    }

    void HideWinScreen(){   //hide the win screen
        winScreen.SetActive(false);
    }
    void ShowWinScreen(){   //show the win screen
        winScreen.SetActive(true);
    }
}
