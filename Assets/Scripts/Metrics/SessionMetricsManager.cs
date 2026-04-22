using Newtonsoft.Json.Bson;
using System.Collections;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Aggregates per-return metrics during a play session and uploads a single summary
/// to Firebase Realtime Database when the session ends.
/// </summary>
public class SessionMetricsManager : MonoBehaviour
{
    private float sessionStartTime;

    private int returnCount;
    private int missCount;
    private int hitCount;

    private float sumSpeed;
    private float sumSpin;
    private float maxSpeed;

    private bool uploaded;

    // Correct swing counters (only incremented when swing matches game mode)
    private int forehandCount;
    private int backhandCount;
    private int smashCount;
    private int sliceCount;

    public GameMode CurrentGameMode { get; private set; } = GameMode.None;
    
    public void SetGameMode(GameMode mode)
    {
        CurrentGameMode = mode;
    }

    public static bool IsLeftHanded { get; private set; }
    public static void SetHandness(bool isLeft)
    {
        IsLeftHanded = isLeft;
    }

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

    /// <summary>
    /// Handles ball return events and applies game-mode logic.
    /// </summary>
    private void HandleBallReturn(string swingType, float speed, float spin, float angle, float distance)
    {
        // Always track raw return metrics
        returnCount++;
        sumSpeed += speed;
        sumSpin += spin;
        if (speed > maxSpeed) maxSpeed = speed;

        // Game mode logic: only correct swings count as hits
        if (IsCorrectSwing(swingType))
        {
            RegisterHit();
            RegisterSwingType(swingType); // only correct swings are counted
        }
        else
        {
            RegisterMiss();
        }
    }

    public void RegisterHit() => hitCount++;
    public void RegisterMiss() => missCount++;

    /// <summary>
    /// Tracks correct swing types only.
    /// </summary>
    private void RegisterSwingType(string swingType)
    {
        switch (swingType)
        {
            case "Forehand": forehandCount++; break;
            case "Backhand": backhandCount++; break;
            case "Smash": smashCount++; break;
            case "Slice": sliceCount++; break;
        }
    }

    // Game mode setters
    public void SetForehandMode() => CurrentGameMode = GameMode.ForehandOnly;
    public void SetBackhandMode() => CurrentGameMode = GameMode.BackhandOnly;
    public void SetSmashMode() => CurrentGameMode = GameMode.SmashOnly;
    public void SetSliceMode() => CurrentGameMode = GameMode.SliceOnly;

    /// <summary>
    /// Determines whether the swing matches the selected game mode.
    /// </summary>
    public bool IsCorrectSwing(string swing)
    {
        return CurrentGameMode switch
        {
            GameMode.ForehandOnly => swing == "Forehand",
            GameMode.BackhandOnly => swing == "Backhand",
            GameMode.SmashOnly => swing == "Smash",
            GameMode.SliceOnly => swing == "Slice",
            _ => true // Free play mode
        };
    }

    /// <summary>
    /// Builds a GameMetrics summary and uploads it.
    /// </summary>
    public void EndSessionAndUpload()
    {
        if (uploaded)
            return;

        uploaded = true;

        float sessionTime = Time.time - sessionStartTime;
        float avgSpeed = returnCount > 0 ? sumSpeed / returnCount : 0f;
        float avgSpin = returnCount > 0 ? sumSpin / returnCount : 0f;

        float distance = FindFirstObjectByType<BallDistanceProvider>()?.Distance ?? 0f;
        int score = Mathf.Max(0, hitCount * 100 - missCount * 10);

        var metrics = new GameMetrics(
            playerId: GetPlayerId(),
            score: score,
            hits: hitCount,
            misses: missCount,
            sessionTime: sessionTime,
            speed: avgSpeed,
            distance: distance,
            spin: avgSpin,
            forehands: forehandCount,
            backhands: backhandCount,
            smashes: smashCount,
            slices: sliceCount
        );

        StartCoroutine(WaitForFirebaseAndUpload(metrics, 10f));
    }

    private string GetPlayerId()
    {
        return AuthenticationManager.user?.UserId ?? "unknown";
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
            Debug.LogWarning("No MetricsUploader found; session metrics were not uploaded.");
            yield break;
        }

        uploader.UploadReturnMetrics(metrics.playerId, metrics);
    }

    public static void SetLeftHanded(bool value) => IsLeftHanded = value;

    /// <summary>
    /// Returns a snapshot of current metrics for UI display.
    /// </summary>
    public GameMetrics GetCurrentMetrics()
    {
        float sessionTime = Time.time - sessionStartTime;
        float avgSpeed = returnCount > 0 ? sumSpeed / returnCount : 0f;
        float avgSpin = returnCount > 0 ? sumSpin / returnCount : 0f;
        float distance = FindFirstObjectByType<BallDistanceProvider>()?.Distance ?? 0f;
        int score = Mathf.Max(0, hitCount * 100 - missCount * 10);

        return new GameMetrics(
            playerId: GetPlayerId(),
            score: score,
            hits: hitCount,
            misses: missCount,
            sessionTime: sessionTime,
            speed: avgSpeed,
            distance: distance,
            spin: avgSpin,
            forehands: forehandCount,
            backhands: backhandCount,
            smashes: smashCount,
            slices: sliceCount
        );
    }
}
