using UnityEngine;

/// <summary>
/// Handles audio playback for ball collisions, including bounce sounds
/// and paddle hit sounds based on the collision type.
/// </summary>
/// <summary>
/// Controls which sound the ball gives off (hit or miss)
/// </summary>
public class BallBounceSound : MonoBehaviour
{
    /// <summary>
    /// Audio clip played when the ball collides with surfaces such as the table, wall, or floor.
    /// </summary>
    public AudioClip bounceSound;

    /// <summary>
    /// Audio clip played when the ball collides with the paddle.
    /// </summary>
    public AudioClip hitSound;

    /// <summary>
    /// AudioSource component used to play sound effects.
    /// </summary>
    private AudioSource audioSource;

    /// <summary>
    /// Initializes the AudioSource component reference when the object starts.
    /// </summary>
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    /// <summary>
    /// Called when the ball collides with another object.
    /// Plays different sounds depending on whether the collision
    /// is with a paddle or another surface.
    /// </summary>
    /// <param name="collision">Collision data including contact and velocity information.</param>
    void OnCollisionEnter(Collision collision)
    {
        // Ignore tiny collisions
        if (collision.relativeVelocity.magnitude < 0.1f)
            return;

        // If the ball hit the paddle
        if (collision.collider.CompareTag("Paddle"))
        {
            audioSource.PlayOneShot(hitSound);
        }
        else
        {
            audioSource.PlayOneShot(bounceSound);
        }
    }
}
