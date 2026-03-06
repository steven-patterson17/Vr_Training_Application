using UnityEngine;
using Firebase.Database;

public class MetricsUploader : MonoBehaviour
{
    public void UploadReturnMetrics(string playerId, GameMetrics metrics)
    {
        if (!FirebaseInitializer.IsReady)
        {
            Debug.LogWarning("Firebase not ready — cannot upload metrics yet.");
            return;
        }

        Debug.Log($"[MetricsUploader] Uploading metrics for player '{playerId}' | " + $"score={metrics.score}, hits={metrics.hits}, misses={metrics.misses}, " + $"speed={metrics.speed}, distance={metrics.distance}, spin={metrics.spin}");

        string json = JsonUtility.ToJson(metrics);

        FirebaseInitializer.RootRef
            .Child("players")
            .Child(playerId)
            .Child("sessions")
            .Push()
            .SetRawJsonValueAsync(json)
            .ContinueWith(task =>
            {
                if (task.IsCompleted)
                    Debug.Log("Metrics uploaded successfully.");
                else
                    Debug.LogError("Upload failed: " + task.Exception);
            });
    }
}
