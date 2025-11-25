using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevelCall : MonoBehaviour
{
    public string nextLevel;

    public void NextLevel(string sceneName)
    {
        sceneName = nextLevel;

        Debug.Log("Calling new level: " + sceneName);
        SceneManager.LoadScene(sceneName);
    }

    public void CallTriviaR1()
    {
        SceneManager.LoadScene("TriviaR1");
    }

    public void CallBackstage()
    {
        Debug.Log("Calling backstage");
        LevelManager.LoadBackStage();
    }
    
}
