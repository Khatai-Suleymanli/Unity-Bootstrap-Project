using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public float moveDistance = 5f;      // How far to move left and right
    public float moveSpeed = 2f;         // How fast it moves

    private Vector3 startPos;
    private int direction = 1;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        transform.Translate(Vector3.right * direction * moveSpeed * Time.deltaTime);

        // Reverse direction if reached limits
        if (Mathf.Abs(transform.position.x - startPos.x) >= moveDistance)
        {
            direction *= -1;
        }
    }
}
