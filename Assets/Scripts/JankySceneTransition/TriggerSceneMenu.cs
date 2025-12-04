using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
public class TriggerSceneMenu : MonoBehaviour
{
    public GameObject menu;
    //public Animator animator;

    void OnTriggerEnter2D(Collider2D other)
    {
        menu.SetActive(true);
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (menu != null){
            menu.SetActive(false);
        }
    }

    public void TeethCleaningScene()
    {
        LevelManager.LoadSpecificScene("MINIGTeethCleaning");
    }

    public void BowlingScene()
    {
        LevelManager.LoadSpecificScene("Bowling");
    }
    public void SharkShoot()
    {
        LevelManager.LoadSpecificScene("SharkShootout");
    }
    public void TempShark()
    {
        LevelManager.LoadSpecificScene("Temple Shark");
    }
    public void RandMiniGame()
    {
        LevelManager.LoadRandMiniGame();
    }
}
