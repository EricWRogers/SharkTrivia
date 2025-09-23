using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
public class TriggerSceneMenu : MonoBehaviour
{
    public GameObject menu;
    public Animator animator;

    void OnTriggerEnter2D(Collider2D other)
    {
        menu.SetActive(true);
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        menu.SetActive(false);
    }

    public void TeethCleaningScene()
    {
        StartCoroutine(LoadLevel("MINIGTeethCleaning"));
    }

    public void BowlingScene()
    {
        StartCoroutine(LoadLevel("BowlingTest"));;
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
