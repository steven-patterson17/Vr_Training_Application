using UnityEngine;
using Firebase.Database;
using Firebase.Extensions;

public class MetricsUploader : MonoBehaviour
{
    private DatabaseReference rootRef;

    void Start()
    {
        rootRef = FirebaseInitializer.RootReference;
    }

    public void UploadReturnMetrics(string playerId, GameMetrics metrics)
    {
        if (!FirebaseInitializer.IsReady || rootRef == null)
        {
            Debug.LogWarning("Firebase not ready - metrics not uploaded. Ensure FirebaseInitializer runs before uploader.");
            return;
        }

        string json = JsonUtility.ToJson(metrics);
        var newRef = rootRef.Child("gamesessions").Child(playerId).Push();
        newRef.SetRawJsonValueAsync(json).ContinueWithOnMainThread(task => {
            if (task.Exception != null)
                Debug.LogError("Failed to upload metrics: " + task.Exception);
            else
                Debug.Log("Metrics uploaded: " + newRef.Key);
        });
    }
}
