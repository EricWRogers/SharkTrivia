using UnityEngine;

public class Dirt : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Toothbrush"))
        {
            ScoreManager.instance.AddPoint();

        }
    }
}
