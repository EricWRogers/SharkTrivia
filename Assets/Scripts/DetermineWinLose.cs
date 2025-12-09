using UnityEngine;

public class DetermineWinLose : MonoBehaviour
{
    public DNode nodeToWin;
    //public GameObject winScreen;

    public DNode nodeToLose;
    //public GameObject loseScreen;

    public void Start()
    {
        //winScreen.SetActive(false);
        //loseScreen.SetActive(false);
    }

    public void DetermineScore(int winThreshold)
    {
        gameObject.SetActive(true);

        if (gameObject == null)
        {
            Instantiate(gameObject);
        }

        Debug.Log("Determining score...");

        int points = ScoreManager.instance.score;

        var dm = DialogueManagerIntegrated.Instance;
        if (!dm || !nodeToWin || !nodeToLose) Debug.LogError("Cant find node");

        if (points >= winThreshold)
        {
            Debug.Log("You win!");
            dm.JumpToNode(nodeToWin);
        }
        if (points < winThreshold)
        {
            Debug.Log("You lose!");
            dm.JumpToNode(nodeToLose);
        }
    }

    public void EnableWinScreen(GameObject _winScreen)
    {
        Instantiate(_winScreen);
        _winScreen.SetActive(true);
    }

    public void EnableLoseScreen(GameObject _loseScreen)
    {
        Instantiate(_loseScreen);
        _loseScreen.SetActive(true);
    }
}
