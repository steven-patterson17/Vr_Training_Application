using Firebase.Auth;
using UnityEngine;
using System.Collections;

/// <summary>
/// Manages user authentication using Firebase within a Unity application.
/// Handles automatic sign-in, including anonymous authentication if no user is currently signed in.
/// </summary>
public class AuthenticationManager : MonoBehaviour
{
    /// <summary>
    /// Singleton instance for global access.
    /// </summary>
    public static AuthenticationManager Instance { get; private set; }

    /// <summary>
    /// Backing field for Firebase authentication instance.
    /// </summary>
    private FirebaseAuth _auth;

    /// <summary>
    /// Public read-only access to FirebaseAuth.
    /// </summary>
    public FirebaseAuth Auth => _auth;

    /// <summary>
    /// Backing field for the currently authenticated Firebase user.
    /// </summary>
    private FirebaseUser _user;

    /// <summary>
    /// Public read-only access to the authenticated user.
    /// </summary>
    public FirebaseUser User => _user;

    /// <summary>
    /// Ensure only one instance exists.
    /// </summary>
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Optional: persist across scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Unity coroutine that initializes Firebase authentication.
    /// Checks if a user is already signed in and, if not, performs anonymous sign-in.
    /// </summary>
    IEnumerator Start()
    {
        // Allow scene to initialize first
        yield return null;

        // Initialize Firebase Auth
        _auth = FirebaseAuth.DefaultInstance;

        // Check if already signed in
        _user = _auth.CurrentUser;
        if (_user != null)
        {
            yield break;
        }

        // Perform anonymous sign-in
        var task = _auth.SignInAnonymouslyAsync();

        // Wait for Firebase task to complete
        yield return new WaitUntil(() => task.IsCompleted);

        if (task.Exception == null)
        {
            _user = task.Result.User;
        }
        else
        {
            Debug.LogError("Anonymous sign-in failed: " + task.Exception);
        }
    }
}