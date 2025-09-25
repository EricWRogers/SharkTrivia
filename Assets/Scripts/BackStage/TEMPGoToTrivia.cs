using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class TEMPGoToTrivia : MonoBehaviour
{
    //This will be temporary and should mostly be for testing purposes


    private void OnTriggerEnter2D(Collider2D other){
        //Debug.Log("Working");
        if(other.tag == "Player"){
            //Debug.Log("Player working");
            SceneManager.LoadScene("TriviaFloorTest");  //trivial floor test dosn't work for some reason but everything else is fine
        }
    }
}
