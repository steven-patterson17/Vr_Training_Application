using Oculus.Interaction;
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

            ReturnSpeed = v.magnitude;

            Vector3 horizontal = new Vector3(v.x, 0f, v.z);
            ReturnAngle = Vector3.Angle(v, horizontal);

            ReturnSpin = rb.angularVelocity.magnitude;

            // Classify swing
            string swingType = ClassifySwing(collision.collider.transform);
            MetricsBoardUI.Instance.SetSwingType(swingType);

            // Register swing type
            var session = FindFirstObjectByType<SessionMetricsManager>();
            session?.RegisterSwingType(swingType);

            // ⭐ NEW — check if swing matches the selected game mode
            if (session != null)
            {
                if (session.IsCorrectSwing(swingType))
                    session.RegisterHit();
                else
                    session.RegisterMiss();
            }

            // Update metrics board
            MetricsBoardUI.Instance.SetReturnMetrics(ReturnAngle, ReturnSpeed, ReturnSpin);

            // Distance
            var distanceProv = GetComponent<BallDistanceProvider>();
            float distance = distanceProv != null ? distanceProv.Distance : 0f;

            OnBallReturn?.Invoke(ReturnSpeed, ReturnSpin, ReturnAngle, distance);
        }
    }



    private string ClassifySwing(Transform paddle)
    {
        Vector3 paddleForward = paddle.forward;
        Vector3 playerRight = Camera.main.transform.right;

        float sideDot = Vector3.Dot(paddleForward, playerRight);
        float upDot = Vector3.Dot(paddleForward, Vector3.up);
        float downDot = Vector3.Dot(paddleForward, Vector3.down);

        // Smash
        if (downDot > 0.5f)
            return "Smash";

        // Slice
        if (upDot > 0.5f)
            return "Slice";

        // Forehand / Backhand depends on handedness
        if (SessionMetricsManager.IsLeftHanded)
        {
            // Flip logic for left-handed players
            return sideDot > 0 ? "Forehand" : "Backhand";
        }
        else
        {
            // Normal logic for right-handed players
            return sideDot > 0 ? "Backhand" : "Forehand";
        }
    }


}