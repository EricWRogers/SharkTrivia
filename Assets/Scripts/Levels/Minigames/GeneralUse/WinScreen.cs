using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class WinScreen : MonoBehaviour
{
    public Timer timer; 
    //public TotalScore totalScore;

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
        // int points = totalScore.GetScore();

        // if(points >= winThreshold)
        //     winText.text = "YOU WIN!";
        // if(points < winThreshold)
        //     winText.text = "you lose!";
        

        // //winTime.text
        // winStat.text = ("Score - " + points);
        ShowWinScreen();    //show the win screen when the minigame is over
    }

    void HideWinScreen(){   //hide the win screen
        winScreen.SetActive(false);
    }
    void ShowWinScreen(){   //show the win screen
        winScreen.SetActive(true);
    }
}
