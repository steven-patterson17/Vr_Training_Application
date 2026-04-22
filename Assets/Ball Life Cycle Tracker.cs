using UnityEngine;


/// <summary>
/// Tracks the cycle of the ball to make sure the ball is
/// hit or missed
/// </summary>
public class BallLifecycleTracker : MonoBehaviour
{
    /// <summary>
    /// Indicates whether the ball has been successfully hit by the paddle.
    /// </summary>
    private bool hitPaddle = false;

    /// <summary>
    /// Subscribes to the ball return event when the object is enabled.
    /// </summary>
    void OnEnable()
    {
        BallReturnMetricsProvider.OnBallReturn += OnBallReturn;
    }

    /// <summary>
    /// Unsubscribes from the ball return event when the object is disabled
    /// to prevent memory leaks or unintended callbacks.
    /// </summary>
    void OnDisable()
    {
        BallReturnMetricsProvider.OnBallReturn -= OnBallReturn;
    }

    /// <summary>
    /// Event handler triggered when the ball is successfully returned by the paddle.
    /// Marks the ball as having been hit.
    /// </summary>
    /// <param name="speed">The speed of the ball at the time of return.</param>
    /// <param name="spin">The spin applied to the ball.</param>
    /// <param name="angle">The return angle of the ball.</param>
    /// <param name="distance">The distance traveled after the return.</param>
    private void OnBallReturn(float speed, float spin, float angle, float distance)
    {
        // This event only fires for THIS ball if the provider is on the ball
        hitPaddle = true;
    }

    /// <summary>
    /// Called when the ball object is destroyed.
    /// If the ball was not hit by the paddle, registers a miss
    /// with the session metrics manager.
    /// </summary>
    void OnDestroy()
    {
        if (!hitPaddle)
        {
            // This ball never hit the paddle → it's a miss
            var session = FindFirstObjectByType<SessionMetricsManager>();

            if (session != null)
            {
                session.RegisterMiss();
            }
        }
    }
}
