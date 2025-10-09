using UnityEngine;

public class Points : MonoBehaviour
{
    [SerializeField] int points = 10;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ScoreManager.instance.AddPoints(points); 
            Destroy(gameObject);
        }
    }
}
