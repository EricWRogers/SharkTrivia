using System.Collections;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;


public class MainMenu : MonoBehaviour
{
    GameObject _difficultyPanel;

    public void Awake()
    {
        _difficultyPanel = GameObject.Find("Difficulty Panel");

        if (_difficultyPanel != null)
        {
            _difficultyPanel.SetActive(false);
        }
    }

    public void TheMagicButton()
    {
        LevelManager.LoadSpecificScene("TriviaR1"); //IntroCutscene
    }

    public void TheLoserButton()
    {
        Application.Quit();
    }

    public void SelectDifficulty()
    {
        if (_difficultyPanel != null )
        {
            _difficultyPanel.SetActive(true);
        }
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