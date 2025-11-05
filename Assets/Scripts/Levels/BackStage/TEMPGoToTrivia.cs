using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class TEMPGoToTrivia : MonoBehaviour
{
    public GameObject menu;
    //This will be temporary and should mostly be for testing purposes
    void OnTriggerEnter2D(Collider2D other)
    {
        menu.SetActive(true);
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        menu.SetActive(false);
        if (menu == null)
        {
            Debug.Log("now thats not a very nice phrase is it?");
        }
    }

    public void TimedTrivia()
    {
        LevelManager.LoadSpecificScene("TestTriviaTimer");
    }

    public void LimitedGuessesTrivia()
    {
        LevelManager.LoadSpecificScene("TestTriviaLimitedGuesses");
    }

    public void LimitedCorrect()
    {
        LevelManager.LoadSpecificScene("TestTriviaLimitedCorrect");
    }

    /// code from when this would only load one scene
    /*
    private void OnTriggerEnter2D(Collider2D other){
        //Debug.Log("Working");
        if(other.tag == "Player"){
            //Debug.Log("Player working");
            LevelManager.LoadSpecificScene("AidanTestTrivia");  //trivial floor test dosn't work for some reason but everything else is fine
        }
    } */
}
