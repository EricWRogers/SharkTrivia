using System.Collections;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;


public class MainMenu : MonoBehaviour
{

    public void TheMagicButton()
    {
        LevelManager.LoadBackStage();
    }

    public void TheLoserButton()
    {
        Application.Quit();
    }

    public void TheGearButton()
    {
        //LevelManager.LoadSettingsMenu();
    }
}