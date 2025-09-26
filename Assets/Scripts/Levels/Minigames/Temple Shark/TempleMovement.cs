using Unity.VisualScripting;
using UnityEngine;

public class TempleMovement : MonoBehaviour
{
    [Header("Movement")]
    public float runSpeed = 2f;
    public float movementSpeed = 5f;
    public float laneOffset = 5.5f;

    private Vector3 moveDirection = Vector3.forward; // Current running direction
    private Vector3 sideAxis = Vector3.right; // Axis for lane shifting

    void Update()
    {
        // Always move forward in current direction
        transform.Translate(moveDirection * runSpeed * Time.deltaTime, Space.World);

        // Handle side movement
        Vector3 side = Vector3.zero;

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            side = -sideAxis;
        }
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            side = sideAxis;
        }

        // Calculate new position
        Vector3 newPos = transform.position + side * movementSpeed * Time.deltaTime;

        // Keep within lane limits (relative to current side axis)
        float distanceFromCenter = Vector3.Dot(newPos, sideAxis);
        if (Mathf.Abs(distanceFromCenter) <= laneOffset)
        {
            transform.position = newPos;
        }
    }

    public void TurnPlayer(int direction) // -1 = left, 1 = right
    {
        // Rotate the movement direction vector
        moveDirection = Quaternion.Euler(0, 90 * direction, 0) * moveDirection;

        // Rotate the player visually
        transform.rotation = Quaternion.LookRotation(moveDirection);

        // Update the side axis (always perpendicular to moveDirection)
        sideAxis = Quaternion.Euler(0, 90, 0) * moveDirection;
        sideAxis.Normalize(); // Keep clean
    }
}
