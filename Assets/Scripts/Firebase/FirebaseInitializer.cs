using UnityEngine;
using Firebase;
using Firebase.Auth;
using Firebase.Database;

public class FirebaseInitializer : MonoBehaviour
{
    public static bool IsReady { get; private set; }
    public static FirebaseAuth Auth { get; private set; }
    public static FirebaseDatabase Database { get; private set; }
    public static DatabaseReference RootRef { get; private set; }

    void Awake()
    {
        DontDestroyOnLoad(gameObject);

        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            var status = task.Result;
            if (status == DependencyStatus.Available)
            {
                InitializeFirebase();
            }
            else
            {
                Debug.LogError("Firebase dependencies could not be resolved: " + status);
            }
        });
    }

    private void InitializeFirebase()
    {
        Auth = FirebaseAuth.DefaultInstance;
        Database = FirebaseDatabase.DefaultInstance;
        RootRef = Database.RootReference;

        IsReady = true;
        Debug.Log("Firebase initialized successfully.");
    }
}
