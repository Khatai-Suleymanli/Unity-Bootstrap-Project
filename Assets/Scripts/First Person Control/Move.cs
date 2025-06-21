using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider), typeof(Rigidbody))]
public class Move : MonoBehaviour
{
    public float walkSpeed = 5;
    public float runSpeed = 10;
    public KeyCode runKey = KeyCode.LeftShift;

    [Header("View Bobbing")]
    public Transform viewTransform; // Camera or head transform
    public float walkBobSpeed = 14f;
    public float walkBobAmount = 0.05f;
    public float runBobSpeed = 18f;
    public float runBobAmount = 0.1f;

    private Rigidbody rb;
    private float defaultYPos = 0;
    private float timer = 0;
    private bool isMoving = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (viewTransform != null)
        {
            defaultYPos = viewTransform.localPosition.y;
        }
    }

    void Update()
    {
        // Handle movement
        float speed = Input.GetKey(runKey) ? runSpeed : walkSpeed;
        float inputX = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
        float inputZ = Input.GetAxis("Vertical") * speed * Time.deltaTime;

        isMoving = Mathf.Abs(inputX) > 0.01f || Mathf.Abs(inputZ) > 0.01f;

        rb.transform.Translate(inputX, 0, inputZ);

        // Handle view bobbing
        if (isMoving && viewTransform != null)
        {
            float currentBobSpeed = Input.GetKey(runKey) ? runBobSpeed : walkBobSpeed;
            float currentBobAmount = Input.GetKey(runKey) ? runBobAmount : walkBobAmount;

            timer += Time.deltaTime * currentBobSpeed;
            float waveSlice = Mathf.Sin(timer);
            float totalBob = waveSlice * currentBobAmount;

            Vector3 localPos = viewTransform.localPosition;
            viewTransform.localPosition = new Vector3(localPos.x, defaultYPos + totalBob, localPos.z);
        }
        else if (viewTransform != null)
        {
            // Smoothly return to default position when not moving
            Vector3 localPos = viewTransform.localPosition;
            viewTransform.localPosition = Vector3.Lerp(
                localPos,
                new Vector3(localPos.x, defaultYPos, localPos.z),
                Time.deltaTime * walkBobSpeed
            );
            timer = 0; // Reset timer when not moving
        }
    }
}