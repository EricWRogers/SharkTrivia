using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ChangeSceneButtons : MonoBehaviour
{
    public GameObject goBackstage;  //load the backstage scene
    public GameObject playMiniGame; //load a random minigame
    public GameObject continueQuiz; //continue the trivia

//adjust for number of working minigames
    public int minigame = 2;    //minigame total

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        HideButtons();
        
    }

    public void HideButtons(){  //call as a singleton to hide all the buttons
        goBackstage.SetActive(false);
        playMiniGame.SetActive(false);
        continueQuiz.SetActive(false);
    }

    public void ShowButtons(){  //call as a singleton to show all the buttons
        goBackstage.SetActive(true);
        playMiniGame.SetActive(true);
        continueQuiz.SetActive(true);
    }

    public void LoadBackStage(){
        SceneManager.LoadScene("BackStage");
    }

    public void KeepGoing(){
        //return to the trivia
    }

    public void LoadMiniGames(){
        int game = Random.Range(minigame,0);
        //Debug.Log(game);

        if (game == 1){
            SceneManager.LoadScene("Bowling");   //load the bowling minigame
        } 
        if (game == 2){
            SceneManager.LoadScene("MINIGTeethCleaning");   //load the Teethcleaning game
        }
        else{
            Debug.Log("Something has gone wrong");  //just incase it chooses an unuseable number
        }

    }
    
}
