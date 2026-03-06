using System.Collections;
using UnityEngine;

/// <summary>
/// Aggregates per-return metrics during a play session and uploads a single summary
/// to Firebase Realtime Database when the session ends.
/// Attach this to a persistent GameObject (DontDestroyOnLoad) or a manager object in your scene.
/// </summary>
public class SessionMetricsManager : MonoBehaviour
{
    private float sessionStartTime;
    private int returnCount;
    private int missCount;
    private float sumSpeed;
    private float sumSpin;
    private float maxSpeed;
    private bool uploaded;

    void Start()
    {
        sessionStartTime = Time.time;
        BallReturnMetricsProvider.OnBallReturn += HandleBallReturn;
    }

    void OnDestroy()
    {
        BallReturnMetricsProvider.OnBallReturn -= HandleBallReturn;
    }

    void OnApplicationQuit()
    {
        EndSessionAndUpload();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            Debug.Log("Manual upload triggered.");
            EndSessionAndUpload();
        }
    }


    private void HandleBallReturn(float speed, float spin, float angle, float distance)
    {
        returnCount++;
        sumSpeed += speed;
        sumSpin += spin;
        if (speed > maxSpeed) maxSpeed = speed;
        Debug.Log("RETURN SPEED = " + speed);

    }

    /// <summary>
    /// Call this to mark a miss from your game logic (out of bounds, hit floor, etc.).
    /// </summary>
    public void RegisterMiss()
    {
        missCount++;
    }

    /// <summary>
    /// Builds a GameMetrics summary and attempts to upload it to Firebase.
    /// If Firebase isn't ready yet, it will wait up to 10s for initialization.
    /// </summary>
    public void EndSessionAndUpload()
    {
        if (uploaded) return; // avoid double uploads
        uploaded = true;

        float sessionTime = Time.time - sessionStartTime;

        float avgSpeed = returnCount > 0 ? sumSpeed / returnCount : 0f;
        float avgSpin = returnCount > 0 ? sumSpin / returnCount : 0f;

        // Try to get the ball's distance provider if present
        var distanceProv = FindFirstObjectByType<BallDistanceProvider>();
        float distance = distanceProv != null ? distanceProv.Distance : 0f;

        int score = Mathf.Max(0, returnCount*100); // simplistic score calculation

        var metrics = new GameMetrics(
            playerId: GetPlayerId(),
            score: score,
            hits: returnCount,
            misses: missCount,
            sessionTime: sessionTime,
            speed: avgSpeed,
            distance: distance,
            spin: avgSpin
        );

        StartCoroutine(WaitForFirebaseAndUpload(metrics, 10f));
    }

    private string GetPlayerId()
    {
        if (FirebaseInitializer.Auth != null && FirebaseInitializer.Auth.CurrentUser != null)
            return FirebaseInitializer.Auth.CurrentUser.UserId;
        return "unknown";
    }

    private IEnumerator WaitForFirebaseAndUpload(GameMetrics metrics, float timeoutSeconds)
    {
        float t = 0f;
        while (!FirebaseInitializer.IsReady && t < timeoutSeconds)
        {
            t += 0.1f;
            yield return new WaitForSeconds(0.1f);
        }

        var uploader = FindFirstObjectByType<MetricsUploader>();
        if (uploader == null)
        {
            Debug.LogWarning("No MetricsUploader found in scene; session metrics were not uploaded.");
            yield break;
        }

        string playerId = metrics.playerId ?? GetPlayerId();
        uploader.UploadReturnMetrics(playerId, metrics);
    }

    public GameMetrics GetCurrentMetrics()
    {
        float sessionTime = Time.time - sessionStartTime;
        float avgSpeed = returnCount > 0 ? sumSpeed / returnCount : 0f;
        float avgSpin = returnCount > 0 ? sumSpin / returnCount : 0f;
        float distance = FindObjectOfType<BallDistanceProvider>()?.Distance ?? 0f;
        int score = Mathf.Max(0, returnCount*100 - missCount*10);

        return new GameMetrics(
            playerId: GetPlayerId(),
            score: score,
            hits: returnCount,
            misses: missCount,
            sessionTime: sessionTime,
            speed: avgSpeed,
            distance: distance,
            spin: avgSpin
        );
    }


}
