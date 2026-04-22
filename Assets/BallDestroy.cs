using UnityEngine;

/// <summary>
/// Destroys the ball when it collides with specified surfaces such as the floor or walls.
/// Used to clean up objects and prevent unnecessary persistence in the scene.
/// </summary>
/// <summary>
/// Instantly destroys the ball if the ball is out of play
/// </summary>
public class DestroyOnHit : MonoBehaviour
{
    /// <summary>
    /// Called when the object collides with another collider.
    /// If the collision is with the floor or a wall, the object is destroyed.
    /// </summary>
    /// <param name="collision">Collision data containing information about the impact.</param>
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Floor") ||
            collision.collider.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
    }
}
