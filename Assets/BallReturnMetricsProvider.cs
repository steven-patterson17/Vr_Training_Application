using System;
using UnityEngine;

public class BallReturnMetricsProvider : MonoBehaviour
{
    public float ReturnAngle { get; private set; }
    public float ReturnSpeed { get; private set; }
    public float ReturnSpin { get; private set; }

    // Raised when the ball is returned by a paddle. Args: speed, spin, angle, distance
    public static event Action<float, float, float, float> OnBallReturn;

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

            // Notify subscribers (e.g., session aggregator). Provide distance if available.
            var distanceProv = GetComponent<BallDistanceProvider>();
            float distance = distanceProv != null ? distanceProv.Distance : 0f;
            OnBallReturn?.Invoke(ReturnSpeed, ReturnSpin, ReturnAngle, distance);
        }
    }
}
