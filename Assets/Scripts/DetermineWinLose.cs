using UnityEngine;

public class DetermineWinLose : MonoBehaviour
{
    public DNode nodeToWin;
    public DNode nodeToLose;

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
}
