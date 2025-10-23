using System.Collections;
using UnityEngine;

public class PositionTurner : MonoBehaviour
{
    public int turnDirection = 1; // -1 left, 1 right
    public float cooldown = 1f; // cooldown time between turns
    private bool isCoolingDown = false;

    void OnTriggerEnter(Collider other)
    {
        if (isCoolingDown || !other.CompareTag("Player")) return;

        var tm = other.GetComponent<TempleMovement>();

        if (tm)
        {
            tm.StartCornerTurn(turnDirection);
            StartCoroutine(CooldownCoroutine());
        }

        IEnumerator CooldownCoroutine()
        {
            isCoolingDown = true;
            yield return new WaitForSeconds(cooldown);
            isCoolingDown = false;
        }

    }
}
