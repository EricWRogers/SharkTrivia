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

    public void TriviaR1()
    {
        LevelManager.LoadSpecificScene("TriviaR1");
    }

    public void TriviaR2()
    {
        LevelManager.LoadSpecificScene("TriviaR2");
    }

    public void TriviaR3()
    {
        LevelManager.LoadSpecificScene("TriviaR3");
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
