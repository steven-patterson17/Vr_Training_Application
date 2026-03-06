using UnityEngine;

public class BallLifecycleTracker : MonoBehaviour
{
    private bool hitPaddle = false;

    void OnEnable()
    {
        BallReturnMetricsProvider.OnBallReturn += OnBallReturn;
    }

    void OnDisable()
    {
        BallReturnMetricsProvider.OnBallReturn -= OnBallReturn;
    }

    private void OnBallReturn(float speed, float spin, float angle, float distance)
    {
        // This event only fires for THIS ball if the provider is on the ball
        hitPaddle = true;
    }

    void OnDestroy()
    {
        if (!hitPaddle)
        {
            // This ball never hit the paddle → it's a miss
            var session = FindObjectOfType<SessionMetricsManager>();
            if (session != null)
            {
                session.RegisterMiss();
            }
        }
    }
}
