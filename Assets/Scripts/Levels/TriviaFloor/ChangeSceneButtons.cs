using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeSceneButtons : MonoBehaviour
{
    public static ChangeSceneButtons Instance { get; private set; }

    [Header("Buttons")]
    [SerializeField] private GameObject goBackstage;   // load the backstage scene
    [SerializeField] private GameObject playMiniGame;  // load a random minigame
    [SerializeField] private GameObject continueQuiz;  // continue the trivia

    [Header("Minigame Count (1..N)")]
    [Min(1)] public int minigame = 2; // number of working minigames

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        HideButtons();
    }

    public void HideButtons()
    {
        SetActiveSafe(goBackstage, false);
        SetActiveSafe(playMiniGame, false);
        SetActiveSafe(continueQuiz, false);
    }

    public void ShowButtons()
    {
        SetActiveSafe(goBackstage, true);
        SetActiveSafe(playMiniGame, true);
        SetActiveSafe(continueQuiz, true);
    }

    // Optional helper if you want to show only the minigame button
    public void ShowOnlyMiniGame()
    {
        SetActiveSafe(goBackstage, false);
        SetActiveSafe(playMiniGame, true);
        SetActiveSafe(continueQuiz, false);
    }

    private void SetActiveSafe(GameObject go, bool on)
    {
        if (go) go.SetActive(on);
        else Debug.LogWarning($"ChangeSceneButtons: a button reference is missing.");
    }


    public void LoadBackStage()
    {
        SceneManager.LoadScene("BackStage");
    }

    public void KeepGoing()
    {
       
        HideButtons();
        
    }

    public void LoadMiniGames()
    {
        //pick 1 minigame
        int game = Random.Range(1, minigame + 1);

        switch (game)
        {
            case 1:
                SceneManager.LoadScene("Bowling");
                break;
            case 2:
                SceneManager.LoadScene("MINIGTeethCleaning");
                break;

            default:
                Debug.LogWarning($"No scene mapped for game index {game}.");
                break;
        }
    }
}