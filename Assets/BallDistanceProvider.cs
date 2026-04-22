using UnityEngine;

/// <summary>
/// Provides the distance the ball travels to
/// he metric board
/// </summary>
public class BallDistanceProvider : MonoBehaviour
{
    /// <summary>
    /// The starting position of the ball when it is initialized.
    /// </summary>
    private Vector3 startPos;

    /// <summary>
    /// Gets the total distance traveled by the ball from its starting position.
    /// </summary>
    public float Distance { get; private set; }

    public void Start()
    {
        startPos = transform.position;
    }

    /// <summary>
    /// Updates the distance value each frame based on the current position
    /// relative to the starting position.
    /// </summary>
    void Update()
    {
        Distance = Vector3.Distance(startPos, transform.position);
    }
}
