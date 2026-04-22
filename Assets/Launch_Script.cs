using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Controls the launching of ping pong balls toward a target area with configurable
/// difficulty, launch parameters, and firing behavior.
/// Handles automatic firing, trajectory calculation, and difficulty-based targeting.
/// </summary>
public class PingPongLauncher : MonoBehaviour
{
    [Header("Ball Settings")]
    public GameObject Ping_Pong_Ball;
    public Transform LaunchPoint;

    [Header("Launch Angle")]
    [Range(5f, 35f)]
    public float launchAngle = 20f;

    [Header("Net Clearance")]
    public float minUpwardLift = 0.25f;

    [Header("Auto Fire")]
    public float currentFireRate = 2f; // shots per second

    private float timer;

    [Header("Difficulty")]
    public Difficulty difficulty = Difficulty.Beginner;

    [Header("Session Control")]
    public bool isRunning = false;

    public float currentLaunchForce = 6f;

    [Header("Difficulty Target Bands")]
    public Transform beginnerLeft;
    public Transform beginnerRight;
    public Transform intermediateLeft;
    public Transform intermediateRight;
    public Transform advancedLeft;
    public Transform advancedRight;

    [Header("Default Target Band")]
    public Transform defaultLeft;
    public Transform defaultRight;

    public bool difficultyChosen = false;

    // Optional UI (safe)
    public Slider launchForceSlider;
    public Slider fireRateSlider;

    void Start()
    {
        if (Ping_Pong_Ball == null)
            Debug.LogError("Ping Pong Ball prefab is NOT assigned!");

        if (LaunchPoint == null)
            Debug.LogError("LaunchPoint is NOT assigned!");

        if (!difficultyChosen)
        {
            currentLaunchForce = 6.0f;
            currentFireRate = 2.5f;
        }
    }

    public void StartLaunching()
    {
        isRunning = true;
        timer = 0f;
    }

    public void StopLaunching()
    {
        isRunning = false;
    }

    void Update()
    {
        if (!isRunning) return;

        timer += Time.deltaTime;

        // Fire rate = shots per second (SAFE)
        if (timer >= (1f / Mathf.Max(currentFireRate, 0.01f)))
        {
            Shoot();
            timer = 0f;
        }
    }

    private (Transform left, Transform right) GetDifficultyBand()
    {
        switch (difficulty)
        {
            case Difficulty.Beginner:
                return (beginnerLeft, beginnerRight);

            case Difficulty.Intermediate:
                return (intermediateLeft, intermediateRight);

            case Difficulty.Advanced:
                return (advancedLeft, advancedRight);

            case Difficulty.Random:
                return (advancedLeft, advancedRight);

            default:
                return (defaultLeft, defaultRight);
        }
    }

    public void Shoot()
    {
        if (Ping_Pong_Ball == null || LaunchPoint == null)
        {
            Debug.LogError("Cannot shoot: Missing prefab or launch point.");
            return;
        }

        if (difficulty == Difficulty.Random)
        {
            currentLaunchForce = Random.Range(3f, 9f);
            currentFireRate = Random.Range(0.5f, 3f);

            if (launchForceSlider != null)
                launchForceSlider.value = currentLaunchForce;

            if (fireRateSlider != null)
                fireRateSlider.value = currentFireRate;
        }

        var band = GetDifficultyBand();

        if (band.left == null || band.right == null)
        {
            Debug.LogError("Difficulty band transforms are NOT assigned!");
            return;
        }

        GameObject ball = Instantiate(Ping_Pong_Ball, LaunchPoint.position, LaunchPoint.rotation);

        if (ball == null)
        {
            Debug.LogError("Ball instantiation failed!");
            return;
        }

        Rigidbody rb = ball.GetComponent<Rigidbody>();

        if (rb == null)
        {
            Debug.LogError("Ball prefab is missing Rigidbody!");
            return;
        }

        // Target selection
        Vector3 target = Vector3.Lerp(band.left.position, band.right.position, Random.value);

        // Direction
        Vector3 dir = (target - LaunchPoint.position).normalized;
        dir = Quaternion.Euler(launchAngle, 0f, 0f) * dir;

        // Physics-based lift
        float horizontalDist = new Vector2(
            target.x - LaunchPoint.position.x,
            target.z - LaunchPoint.position.z
        ).magnitude;

        float time = horizontalDist / Mathf.Max(currentLaunchForce, 0.01f);

        float requiredVy =
            (target.y - LaunchPoint.position.y +
            0.5f * Mathf.Abs(Physics.gravity.y) * time * time) / time;

        float forceFactor = Mathf.InverseLerp(5f, 10f, currentLaunchForce);
        float liftMultiplier = Mathf.Lerp(1.25f, 1.82f, Mathf.Pow(forceFactor, 1.35f));
        requiredVy *= liftMultiplier;

        float requiredLift = requiredVy / Mathf.Max(currentLaunchForce, 0.01f);
        dir.y = Mathf.Max(dir.y, requiredLift, minUpwardLift);

        // Apply velocity safely
        rb.linearVelocity = dir * currentLaunchForce;
        rb.angularVelocity = Vector3.zero;

        rb.AddTorque(Random.insideUnitSphere * 0.1f, ForceMode.Impulse);

        // Safe Metrics hookup
        if (MetricsBoardUI.Instance != null)
        {
            MetricsBoardUI.Instance.speedProvider = ball.GetComponent<BallSpeedProvider>();
            MetricsBoardUI.Instance.distanceProvider = ball.GetComponent<BallDistanceProvider>();
        }

        Destroy(ball, 3f);
    }

    public void ApplyDifficulty()
    {
        difficultyChosen = true;

        switch (difficulty)
        {
            case Difficulty.Beginner:
                currentLaunchForce = 3.5f;
                currentFireRate = 1f;
                break;

            case Difficulty.Intermediate:
                currentLaunchForce = 5.5f;
                currentFireRate = 2f;
                break;

            case Difficulty.Advanced:
                currentLaunchForce = 8f;
                currentFireRate = 3f;
                break;
        }
    }
}