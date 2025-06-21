using UnityEngine;

public class Trampoline : MonoBehaviour
{
    public float launchForce = 20f; // How strong the bounce is
    public string playerTag = "Player";

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(playerTag))
        {
            Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                // Cancel downward velocity and launch upward
                rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
                rb.AddForce(Vector3.up * launchForce, ForceMode.VelocityChange);
            }
        }
    }
}
