using UnityEngine;

public class GhostMovement : MonoBehaviour
{
    public Transform[] waypoints;
    private int waypointIndex = 0;
    public float speed = 3.0f;
    public float rotationSpeed = 20.0f;
    public float activationDistance = 5.0f;
    public Transform player;

    private bool isMoving = false;

    void Start()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }

        // Start moving to the first waypoint immediately
        isMoving = true;
    }

    void Update()
    {
        if (waypointIndex >= waypoints.Length) return;
        if (waypoints[waypointIndex] == null) return;

        // Check if reached current waypoint
        bool reachedWaypoint = Vector3.Distance(transform.position, waypoints[waypointIndex].position) < 0.1f;

        if (reachedWaypoint)
        {
            // Stop moving when reaching a waypoint
            isMoving = false;

            // Check if player is close enough to start moving to next waypoint
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);
            if (distanceToPlayer < activationDistance)
            {
                waypointIndex++;
                isMoving = true;
            }
        }

        // Only move if isMoving is true
        if (isMoving)
        {
            // Move towards current waypoint
            transform.position = Vector3.MoveTowards(
                transform.position,
                waypoints[waypointIndex].position,
                speed * Time.deltaTime
            );

            // Rotate towards current waypoint
            Vector3 direction = waypoints[waypointIndex].position - transform.position;
            if (direction != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.RotateTowards(
                    transform.rotation,
                    targetRotation,
                    rotationSpeed * Time.deltaTime
                );
            }
        }
    }
}