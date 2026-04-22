using UnityEngine;

/// <summary>
/// Controls how the user is able to move around
/// the unity scene
/// </summary>
public class PlayerMovement : MonoBehaviour
{
    public float speed = 3f;
    public Transform cameraTransform; // assign CenterEyeAnchor

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        // Read input (example: left thumbstick)
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        // Move relative to headset direction
        Vector3 forward = cameraTransform.forward;
        forward.y = 0;
        forward.Normalize();

        Vector3 right = cameraTransform.right;
        right.y = 0;
        right.Normalize();

        Vector3 move = (forward * y + right * x) * speed;

        rb.MovePosition(rb.position + move * Time.fixedDeltaTime);
    }
}
