using UnityEngine;

public class BallReturnMetricsProvider : MonoBehaviour
{
    public float ReturnAngle { get; private set; }
    public float ReturnSpeed { get; private set; }
    public float ReturnSpin { get; private set; }

    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Paddle"))
        {
            Vector3 v = rb.linearVelocity;

            // Speed
            ReturnSpeed = v.magnitude;

            // Angle relative to horizontal plane
            Vector3 horizontal = new Vector3(v.x, 0f, v.z);
            ReturnAngle = Vector3.Angle(v, horizontal);

            // Spin (angular velocity magnitude)
            ReturnSpin = rb.angularVelocity.magnitude;

            // Notify UI
            MetricsBoardUI.Instance.SetReturnMetrics(ReturnAngle, ReturnSpeed, ReturnSpin);
        }
    }
}
