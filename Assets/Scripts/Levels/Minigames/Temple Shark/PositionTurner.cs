using UnityEngine;

public class PositionTurner : MonoBehaviour
{
    public int turnDirection = 1; // -1 left, 1 right

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        var tm = other.GetComponent<TempleMovement>();
        if (tm) tm.StartCornerTurn(turnDirection);
    }
}
