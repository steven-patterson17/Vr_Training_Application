using UnityEngine;

/// <summary>
/// Provides real-time speed data for a ball using its Rigidbody component.
/// Calculates the magnitude of velocity each frame.
/// </summary>
/// <summary>
/// Provides speed of the ball from the launcher
/// </summary>
public class BallSpeedProvider : MonoBehaviour
{
    /// <summary>
    /// Reference to the Rigidbody component used to determine velocity.
    /// </summary>
    private Rigidbody rb;

    /// <summary>
    /// Gets the current speed of the ball, calculated as the magnitude of its velocity.
    /// </summary>
    public float Speed { get; private set; }

    /// <summary>
    /// Initializes the Rigidbody component reference when the object starts.
    /// </summary>
    public void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    /// <summary>
    /// Updates the speed value every frame based on the Rigidbody's velocity.
    /// </summary>
    void Update()
    {
        Speed = rb.linearVelocity.magnitude;
    }
}
