using UnityEngine;

public class RandomMinigameCall : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void CallRandomLevel()
    {
        LevelManager.LoadRandMiniGame();
    }
}
