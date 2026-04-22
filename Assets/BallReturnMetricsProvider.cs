using Oculus.Interaction;
using System;
using UnityEngine;

/// <summary>
/// Provides metrics when a ball is returned by a paddle, including speed,
/// angle, spin, and distance. Also classifies swing type and updates session data.
/// </summary>
/// <summary>
/// Calculates and displays the user's return
/// metrics of hitting the ball
/// </summary>
public class BallReturnMetricsProvider : MonoBehaviour
{
    /// <summary>
    /// Gets the angle of the ball's return relative to the horizontal plane.
    /// </summary>
    public float ReturnAngle { get; private set; }

    /// <summary>
    /// Gets the speed of the ball at the moment of return.
    /// </summary>
    public float ReturnSpeed { get; private set; }

    /// <summary>
    /// Gets the spin (angular velocity magnitude) of the ball at return.
    /// </summary>
    public float ReturnSpin { get; private set; }

    /// <summary>
    /// Event triggered when the ball is returned by a paddle.
    /// Provides return speed, spin, angle, and distance traveled.
    /// </summary>
    public static event Action<float, float, float, float> OnBallReturn;

    /// <summary>
    /// Reference to the Rigidbody component used for physics calculations.
    /// </summary>
    private Rigidbody rb;

    /// <summary>
    /// Initializes the Rigidbody component reference.
    /// </summary>
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    /// <summary>
    /// Called when the ball collides with another object.
    /// If the object is a paddle, calculates return metrics,
    /// classifies the swing, updates session data, and raises an event.
    /// </summary>
    /// <param name="collision">Collision data containing contact and velocity information.</param>
    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Paddle"))
        {
            Vector3 v = rb.linearVelocity;

            // Calculate return speed
            ReturnSpeed = v.magnitude;

            // Calculate return angle relative to horizontal plane
            Vector3 horizontal = new Vector3(v.x, 0f, v.z);
            ReturnAngle = Vector3.Angle(v, horizontal);

            // Calculate spin (angular velocity magnitude)
            ReturnSpin = rb.angularVelocity.magnitude;

            // Classify swing type
            string swingType = ClassifySwing(collision.collider.transform);
            MetricsBoardUI.Instance.SetSwingType(swingType);

            // Register swing type in session metrics
            var session = FindFirstObjectByType<SessionMetricsManager>();
            session?.RegisterSwingType(swingType);

            // Check if swing matches selected game mode
            if (session != null)
            {
                if (session.IsCorrectSwing(swingType))
                    session.RegisterHit();
                else
                    session.RegisterMiss();
            }

            // Update UI with return metrics
            MetricsBoardUI.Instance.SetReturnMetrics(ReturnAngle, ReturnSpeed, ReturnSpin);

            // Calculate distance traveled
            var distanceProv = GetComponent<BallDistanceProvider>();
            float distance = distanceProv != null ? distanceProv.Distance : 0f;

            // Raise event for other systems
            OnBallReturn?.Invoke(ReturnSpeed, ReturnSpin, ReturnAngle, distance);
        }
    }

    /// <summary>
    /// Classifies the type of swing (e.g., forehand, backhand, smash, slice)
    /// based on the paddle's orientation relative to the player.
    /// </summary>
    /// <param name="paddle">The transform of the paddle that hit the ball.</param>
    /// <returns>A string representing the detected swing type.</returns>
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