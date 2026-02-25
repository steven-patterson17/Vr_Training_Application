using UnityEngine;

public class PingPongLauncher : MonoBehaviour
{
    [Header("Ball Settings")]
    public GameObject Ping_Pong_Ball;
    public Transform LaunchPoint;
    public Transform tableTarget;   // NEW: aim point on the table
    public float launchForce = 11f;

    [Header("Auto Fire")]
    public float fireRate = 5f;
    private float timer;

    [Header("UI")]
    public MetricsBoardUI metricsBoard;

    [Header("Difficulty")]
    public Difficulty difficulty = Difficulty.Beginner;
    public float currentLaunchForce;
    public float currentSpread;
    public float currentFireRate;

    void Start()
    {
        ApplyDifficulty();
    }

    void Update()
    {
        // Aim at the table, not the player
        if (tableTarget != null)
        {
            LaunchPoint.forward = (tableTarget.position - LaunchPoint.position).normalized;
        }

        timer += Time.deltaTime;
        if (timer >= currentFireRate)
        {
            Shoot();
            timer = 0f;
        }
    }

    public void Shoot()
    {
        GameObject ball = Instantiate(Ping_Pong_Ball, LaunchPoint.position, LaunchPoint.rotation);
        Rigidbody rb = ball.GetComponent<Rigidbody>();

        // Base direction toward table
        Vector3 dir = (tableTarget.position - LaunchPoint.position).normalized;

        // Add controlled spread WITHOUT tilting upward
        dir += new Vector3(
            Random.Range(-currentSpread, currentSpread),
            Random.Range(-currentSpread * 0.5f, currentSpread * 0.5f), // less vertical randomness
            0f
        );

        // Slight downward tilt so ball lands on table
        dir.y -= 0.05f;

        dir.Normalize();

        // Apply velocity directly for consistent speed
        rb.linearVelocity = dir * currentLaunchForce;

        // Add small random spin
        rb.AddTorque(Random.insideUnitSphere * 0.1f, ForceMode.Impulse);

        // Update metrics
        metricsBoard.speedProvider = ball.GetComponent<BallSpeedProvider>();
        metricsBoard.distanceProvider = ball.GetComponent<BallDistanceProvider>();

        Destroy(ball, 5f);
    }

    private void ApplyDifficulty()
    {
        switch (difficulty)
        {
            case Difficulty.Beginner:
                currentLaunchForce = 6f;
                currentSpread = 0.02f;
                currentFireRate = 3f;
                break;

            case Difficulty.Intermediate:
                currentLaunchForce = 14f;
                currentSpread = 0.05f;
                currentFireRate = 2f;
                break;

            case Difficulty.Advanced:
                currentLaunchForce = 20f;
                currentSpread = 0.12f;
                currentFireRate = 1f;
                break;
        }
    }
}
