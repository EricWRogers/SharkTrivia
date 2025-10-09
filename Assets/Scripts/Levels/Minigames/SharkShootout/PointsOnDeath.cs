using UnityEngine;

public class PointsOnDeath : MonoBehaviour
{
    [HideInInspector] public int points;

    private void OnDestroy()
    {
        
        if (ScoreManager.instance != null && Application.isPlaying)
        {
            ScoreManager.instance.AddPoints(points);
        }
    }
}
