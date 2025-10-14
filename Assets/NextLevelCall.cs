using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevelCall : MonoBehaviour
{
    public string nextLevel;
    public DialogueManagerIntegrated dm;

    public void Awake()
    {
        //dm = GetComponent<DialogueManagerIntegrated>();
    }

    public void Update()
    {
        if (dm.isFinished)
        {
            NextLevel(nextLevel);
        }
    }

    public void NextLevel(string sceneName)
    {
        sceneName = nextLevel;

        SceneManager.LoadScene(sceneName);
    }

    public void CallBackstage()
    {
        Debug.Log("Calling backstage");
        LevelManager.LoadBackStage();
    }
    
}
