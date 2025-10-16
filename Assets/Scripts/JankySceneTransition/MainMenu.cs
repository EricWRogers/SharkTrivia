using System.Collections;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;


public class MainMenu : MonoBehaviour
{

    public void TheMagicButton()
    {
        LevelManager.LoadSpecificScene("IntroCutscene");
    }

    public void TheLoserButton()
    {
        Application.Quit();
    }

    public void TheGearButton()
    {
        LevelManager.LoadSettingMenu();
    }
    public void TheEarButton()
    {
        LevelManager.LoadAudioMenu();
    }
    public void TheMainButton()
    {
        LevelManager.LoadMainMenu();
    }
}