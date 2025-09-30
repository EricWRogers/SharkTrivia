using UnityEngine;

public class PositionTurner : MonoBehaviour
{
    [Tooltip("Set -1 for left turn, 1 for right turn")]
    public int turnDirection = 1;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        TempleMovement player = other.GetComponent<TempleMovement>();
        if (player != null)
        {
            Debug.Log("Turning player " + (turnDirection == 1 ? "right" : "left"));
            player.TurnPlayer(turnDirection);
        }
    }
}
