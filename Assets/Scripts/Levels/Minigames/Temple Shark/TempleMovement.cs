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
    private Vector3 laneCenter;

    void Start()
    {
        laneCenter = transform.position; // whereever the player starts is the lane center
    }

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

        if (side != Vector3.zero)
        {
            // Calculate new position
            Vector3 newPos = transform.position + side * movementSpeed * Time.deltaTime;

            // Measure distance from the lane center
            float distanceFromCenter = Vector3.Dot(newPos - laneCenter, sideAxis);

            if (Mathf.Abs(distanceFromCenter) <= laneOffset)
            {
                transform.position = newPos;
            }
        }
    }

    public void TurnPlayer(int direction) // -1 = left, 1 = right
    {
        // Rotate the movement direction vector
        moveDirection = Quaternion.Euler(0, 90 * direction, 0) * moveDirection;

        // Rotate the player visually
        transform.rotation = Quaternion.LookRotation(moveDirection);

        // Update the side axis (always perpendicular to moveDirection)
        sideAxis = Vector3.Cross(Vector3.up, moveDirection).normalized;

        // Reset lane center after turning
        laneCenter = transform.position;
    }

    public void ResetTurn()
    {
        // Reset your internal direction variable (example)
        moveDirection = Vector3.forward;
        transform.rotation = Quaternion.identity;
        
        // Recalculate side axis so A/D movement works again
        sideAxis = Vector3.right;

        // Reset lane center to current spawn point
        laneCenter = transform.position;
    }

}
