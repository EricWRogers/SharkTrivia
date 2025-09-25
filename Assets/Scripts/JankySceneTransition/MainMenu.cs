using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MainMenu : MonoBehaviour
{
    public Animator animator;
    public void TheMagicButton()
    {
        StartCoroutine(LoadLevel("BackStage"));
    }

    public void TheLoserButton()
    {
        Application.Quit();
    }
    IEnumerator LoadLevel(string levelName)
    {
        if (animator != null)
        {
            animator.SetTrigger("Start");
            yield return new WaitForSeconds(1);
        }
        SceneManager.LoadScene(levelName);
    }
}