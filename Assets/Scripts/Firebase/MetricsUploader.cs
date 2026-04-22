using UnityEngine;
using Firebase.Database;

/// <summary>
/// Handles uploading player performance metrics to the Firebase Realtime Database.
/// Ensures Firebase is initialized before attempting to send data.
/// </summary>
public class MetricsUploader : MonoBehaviour
{
    public void UploadReturnMetrics(string playerId, GameMetrics metrics)
    {
        if (!FirebaseInitializer.IsReady)
        {
            Debug.LogWarning("Firebase not ready — cannot upload metrics yet.");
            return;
        }
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