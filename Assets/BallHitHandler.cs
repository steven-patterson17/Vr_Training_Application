using UnityEngine;

public class BallHitHandler : MonoBehaviour
{
    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void OnHitByPaddle(Vector3 hitPoint, Vector3 paddleVelocity, Vector3 paddleNormal)
    {
        // Use paddle velocity directly
        Vector3 returnVelocity = paddleVelocity * 0.8f; // tweak multiplier

        // Reflect based on paddle face
        returnVelocity = Vector3.Reflect(returnVelocity, -paddleNormal);

        rb.linearVelocity = returnVelocity;

        // Optional spin
        rb.AddTorque(Random.insideUnitSphere * 0.2f, ForceMode.Impulse);
    }

}
