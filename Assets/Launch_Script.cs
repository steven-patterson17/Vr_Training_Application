using UnityEngine;

public class PingPongLauncher : MonoBehaviour
{
    [Header("Ball Settings")]
    public GameObject Ping_Pong_Ball;
    public Transform LaunchPoint;
    public Transform tableTarget;
    public float launchForce = 11f;   // base value (not used directly)

    [Header("Auto Fire")]
    public float fireRate = 5f;       // base value (not used directly)
    private float timer;

    [Header("UI")]
    public MetricsBoardUI metricsBoard;

    [Header("Difficulty")]
    public Difficulty difficulty = Difficulty.Beginner;

    [Header("Session Control")]
    public bool isRunning = false;


    // These are the REAL values used by the launcher
    public float currentLaunchForce;
    public float currentSpread;
    public float currentFireRate;

    void Start()
    {
        ApplyDifficulty();
    }

    public void StartLaunching()
    {
        Debug.Log("Launcher STARTING");
        ApplyDifficulty();   // ← THIS FIXES 90% OF CASES
        isRunning = true;
        timer = 0f;
    }



    public void StopLaunching()
    {
        isRunning = false;
    }

    void Update()
    {
        if (!isRunning)
            return;

        // Aim at the table
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

        // Add controlled spread
        dir += new Vector3(
            Random.Range(-currentSpread, currentSpread),
            Random.Range(-currentSpread * 0.5f, currentSpread * 0.5f),
            0f
        );

        // Slight downward tilt
        dir.y -= 0.05f;
        dir.Normalize();

        // Apply velocity
        rb.linearVelocity = dir * currentLaunchForce;

        // Add small random spin
        rb.AddTorque(Random.insideUnitSphere * 0.1f, ForceMode.Impulse);

        // Update metrics
        metricsBoard.speedProvider = ball.GetComponent<BallSpeedProvider>();
        metricsBoard.distanceProvider = ball.GetComponent<BallDistanceProvider>();

        Destroy(ball, 5f);
    }

    // Called by DifficultyManager
    public void ApplyDifficulty()
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

    // Called by sliders
    public void SetLaunchForce(float value)
    {
        currentLaunchForce = value;
    }

    public void SetFireRate(float value)
    {
        currentFireRate = value;
    }
}
