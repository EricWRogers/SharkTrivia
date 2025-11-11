using System;
using UnityEngine;

public class CanvasPathFollwer : MonoBehaviour
{
    public Transform[] waypoints;
    public float moveSpeed = 2f;
    public bool movingForward = false;
    public int currentWaypointIndex = 0;
    public BowlingManager bowlingManager;
    public BowlingBall bowlingBall;
    public VideoPlayerScript videoPlayerScript;

    void Start()
    {
        // Initialize position to the first waypoint
        if (waypoints.Length > 0)
        { 
            transform.position = waypoints[0].position;
        }
    }

    void Update()
    {
        // Move along the path only if the ball has been launched
        if (bowlingBall.hasLaunched)
        {
            MoveAlongPath();
        }
    }
    private void MoveAlongPath()
    {
        movingForward = true;
        // Move towards the next waypoint
        if (movingForward && currentWaypointIndex < waypoints.Length - 1)
        {
            // Just a safety check to prevent index out of range
            Transform targetWaypoint = waypoints[currentWaypointIndex + 1];
            // Move towards the target waypoint at the specified speed
            transform.position = Vector3.MoveTowards(transform.position, targetWaypoint.position, moveSpeed * Time.deltaTime);
            // Check if reached the waypoint
            if (Vector3.Distance(transform.position, targetWaypoint.position) < 0.1f)
            {
                currentWaypointIndex++;
            }
        }
    }
    public void ResetPath()
    {
        // Reset to the starting waypoint
        currentWaypointIndex = 0;
        transform.position = waypoints[0].position;
        movingForward = false;
    }
}