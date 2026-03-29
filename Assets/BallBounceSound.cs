using UnityEngine;

public class BallBounceSound : MonoBehaviour
{
    public AudioClip bounceSound;   // table, wall, floor
    public AudioClip hitSound;      // paddle hit

    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

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
