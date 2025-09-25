using UnityEngine;

public class Gutter : MonoBehaviour
{
    private BowlingManager manager;

    void Start()
    {
        manager = Object.FindAnyObjectByType<BowlingManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            Debug.Log("Gutter Ball!");

            Rigidbody rb = other.GetComponent<Rigidbody>();

            // Stop ball movement
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;

            // Call manager to handle round + reset
            if (manager != null)
            {
                manager.CountPinsDown();
                manager.NewRound();
                manager.ResetPins();
            }
        }
    }
}
