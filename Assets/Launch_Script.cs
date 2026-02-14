using UnityEngine;

public class PingPongLauncher : MonoBehaviour
{
    [Header("Ball Settings")]
    public GameObject Ping_Pong_Ball;
    public Transform LaunchPoint;
    public float launchForce = 12f;

    [Header("Auto Aim & Fire")]
    public Transform player;
    public float fireRate = 5f;

    private float timer;

    [Header("UI")]
    public MetricsBoardUI metricsBoard;

    void Update()
    {
        if (player != null)
        {
            LaunchPoint.forward = (player.position - LaunchPoint.position).normalized;
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

        Vector3 spread = LaunchPoint.forward
    + new Vector3(
        Random.Range(-currentSpread, currentSpread),
        Random.Range(-currentSpread, currentSpread),
        0f);

        rb.linearVelocity = spread.normalized * currentLaunchForce;



        rb.AddTorque(Random.insideUnitSphere * 0.1f, ForceMode.Impulse);

        // Assign providers to the board
        metricsBoard.speedProvider = ball.GetComponent<BallSpeedProvider>();
        metricsBoard.distanceProvider = ball.GetComponent<BallDistanceProvider>();

        Destroy(ball, 5f);
    }

    public Difficulty difficulty = Difficulty.Beginner;

    public float currentLaunchForce;
    public float currentSpread;
    public float currentFireRate;


    private void ApplyDifficulty()
    {
        switch (difficulty)
        {
            case Difficulty.Beginner:
                currentLaunchForce = 8f;
                currentSpread = 0.02f;
                currentFireRate = 3f;
                break;

            case Difficulty.Intermediate:
                currentLaunchForce = 12f;
                currentSpread = 0.05f;
                currentFireRate = 2f;
                break;

            case Difficulty.Advanced:
                currentLaunchForce = 16f;
                currentSpread = 0.12f;
                currentFireRate = 1f;
                break;
        }
    }

    void Start()
    {
        ApplyDifficulty();
    }


}
