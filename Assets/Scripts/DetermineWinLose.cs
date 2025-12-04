using UnityEngine;

public class DetermineWinLose : MonoBehaviour
{
    public DNode nodeToWin;
    public DNode nodeToLose;

    public void DetermineScore(int winThreshold)
    {
        gameObject.SetActive(true);
        Instantiate(this.gameObject);
        
        int points = ScoreManager.instance.score;

        var dm = DialogueManagerIntegrated.Instance;
        if (!dm || !nodeToWin || !nodeToLose) Debug.LogError("Cant find node");

        if (points >= winThreshold)
        {
            dm.JumpToNode(nodeToWin);
        }
        if (points < winThreshold)
        {
            dm.JumpToNode(nodeToLose);
        }
    }
}
