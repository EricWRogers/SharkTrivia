using UnityEngine;
using UnityEngine.SceneManagement;
public class TriggerSceneMenu : MonoBehaviour
{
    public GameObject menu;

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
        SceneManager.LoadScene(1);
    }

    public void BowlingScene()
    {
        SceneManager.LoadScene(2);
    }
}
