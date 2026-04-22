using UnityEngine;
using Firebase;
using Firebase.Auth;
using Firebase.Database;

/// <summary>
/// Initializes Firebase services within the Unity application and provides
/// global access to authentication and database instances.
/// Ensures dependencies are resolved before Firebase is used.
/// </summary>
namespace VRTraining
{
    public class FirebaseInitializer : MonoBehaviour
    {
        /// <summary>
        /// Indicates whether Firebase has been successfully initialized and is ready for use.
        /// </summary>
        public static bool IsReady { get; private set; }
    
        /// <summary>
        /// Provides access to the Firebase authentication instance.
        /// </summary>
        public static FirebaseAuth Auth { get; private set; }
    
        /// <summary>
        /// Provides access to the Firebase Realtime Database instance.
        /// </summary>
        public static FirebaseDatabase Database { get; private set; }
    
        /// <summary>
        /// Reference to the root of the Firebase Realtime Database.
        /// </summary>
        public static DatabaseReference RootRef { get; private set; }
    
        /// <summary>
        /// Unity lifecycle method that initializes Firebase dependencies when the object awakens.
        /// Ensures the object persists across scene loads and checks for required Firebase dependencies.
        /// </summary>
        void Awake()
        {
            DontDestroyOnLoad(gameObject);
    
            FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
            {
                var status = task.Result;
    
                // If dependencies are available, proceed with initialization
                if (status == DependencyStatus.Available)
                {
                    InitializeFirebase();
                }
                else
                {
                }
            });
        }
    
        /// <summary>
        /// Initializes Firebase services, including authentication and database access.
        /// Sets up global references and marks the system as ready.
        /// </summary>
        private void InitializeFirebase()
        {
            Auth = FirebaseAuth.DefaultInstance;
            Database = FirebaseDatabase.DefaultInstance;
            RootRef = Database.RootReference;
    
            IsReady = true;
        }
    }
}
