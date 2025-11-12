using Unity.VisualScripting;
using UnityEngine;
using System.Collections;

public class TempleMovement : MonoBehaviour
{
    [Header("Movement")]
    public float runSpeed = 2f;
    public float movementSpeed = 5f;
    public float laneOffset = 5.5f;
    public float groundOffset = 1f;

    [Header("Corner Turn")]
    public bool useSmoothTurn = true;
    public float turnDuration = 0.5f; // smooth turn time
   
    public Transform cameraRig = null;

    Vector3 moveDirection = Vector3.forward;
    Vector3 sideAxis = Vector3.right;
    Vector3 vertical = Vector3.zero;
    Vector3 laneCenter;
    bool isTurning = false;

    // store original ground height so it doesn't shift over time
    float baseGroundHeight;

    void Start()
    {
        laneCenter = transform.position;
        baseGroundHeight = laneCenter.y;
    }

    void Update()
    {
        // Always move forward in current direction
        transform.Translate(moveDirection * runSpeed * Time.deltaTime, Space.World);

        // Handle up/down movement
        vertical = Vector3.zero;

        if (Input.GetKey(KeyCode.Q) || Input.GetKey(KeyCode.UpArrow))
        {
            vertical = Vector3.up;
        }
        else if (Input.GetKey(KeyCode.E) || Input.GetKey(KeyCode.DownArrow))
        {
            vertical = -Vector3.up;
        }

        if (vertical != Vector3.zero)
        {
            // Calculate new position
            Vector3 newPos = transform.position + vertical * movementSpeed * Time.deltaTime;

            // Measure distance from the lane center
            float distanceFromCenter = Vector3.Dot(newPos - laneCenter, Vector3.up);

            if (Mathf.Abs(distanceFromCenter) <= groundOffset)
            {
                transform.position = newPos;
            }
        }

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

    public void TurnPlayer(int direction)
    {
        // Rotate the movement direction vector
        moveDirection = Quaternion.Euler(0, 90 * direction, 0) * moveDirection;

        // Rotate the player visually
        transform.rotation = Quaternion.LookRotation(moveDirection);

        // Update the side axis (always perpendicular to moveDirection)
        sideAxis = Vector3.Cross(Vector3.up, moveDirection).normalized;

        // Keep vertical movement functional after turning
        vertical = Vector3.zero;

        // Keep same ground height reference to prevent drift
        laneCenter = new Vector3(transform.position.x, baseGroundHeight, transform.position.z);
    }

    public void ResetTurn()
    {
        
        moveDirection = Vector3.forward;
        transform.rotation = Quaternion.identity;


        sideAxis = Vector3.right;
        vertical = Vector3.zero;

        // Reset lane center to current spawn point
        laneCenter = new Vector3(transform.position.x, baseGroundHeight, transform.position.z);
    }
    
        public void StartCornerTurn(int direction)  // -1 = left, 1 = right
    {
        if (isTurning) return;
        StartCoroutine(CornerTurnRoutine(direction, turnDuration));
    }

    IEnumerator CornerTurnRoutine(int direction, float duration)
    {
        isTurning = true;
        float savedRunSpeed = runSpeed;
        runSpeed = 0f;

        Vector3 startMoveDir = moveDirection;
        Vector3 targetMoveDir = Quaternion.Euler(0, 90 * direction, 0) * startMoveDir;

        Quaternion startRot = transform.rotation;
        Quaternion targetRot = Quaternion.LookRotation(targetMoveDir, Vector3.up);

        // rotate player as camera 
        Quaternion camStart = cameraRig ? cameraRig.rotation : Quaternion.identity;
        Quaternion camTarget = cameraRig ? Quaternion.LookRotation(targetMoveDir, Vector3.up) : Quaternion.identity;

        float t = 0f;
        duration = Mathf.Max(0.01f, duration);
        while (t < 1f)
        {
            t += Time.deltaTime / duration;

            transform.rotation = Quaternion.Slerp(startRot, targetRot, t);
            if (cameraRig) cameraRig.rotation = Quaternion.Slerp(camStart, camTarget, t);

            yield return null;
        }

        transform.rotation = targetRot;
        if (cameraRig) cameraRig.rotation = camTarget;

        moveDirection = targetMoveDir;
        sideAxis = Vector3.Cross(Vector3.up, moveDirection).normalized;
        vertical = Vector3.zero;
        
        // keep Y fixed to base ground height
        laneCenter = new Vector3(transform.position.x, baseGroundHeight, transform.position.z);

        runSpeed = savedRunSpeed;
        isTurning = false;
    }

}
