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
    /// Backing field for Firebase authentication instance.
    /// </summary>
    private static FirebaseAuth _auth;

    /// <summary>
    /// Public read-only access to FirebaseAuth.
    /// </summary>
    public static FirebaseAuth Auth => _auth;

    /// <summary>
    /// Backing field for the currently authenticated Firebase user.
    /// </summary>
    private static FirebaseUser _user;

    /// <summary>
    /// Public read-only access to the authenticated user.
    /// </summary>
    public static FirebaseUser User => _user;

    /// <summary>
    /// Unity coroutine that initializes Firebase authentication.
    /// Checks if a user is already signed in and, if not, performs anonymous sign-in.
    /// </summary>
    IEnumerator Start()
    {
        // Allow XR and scene objects to initialize first
        yield return null;

        // Initialize Firebase Auth
        _auth = FirebaseAuth.DefaultInstance;

        // Already signed in?
        _user = _auth.CurrentUser;
        if (_user != null)
        {
            yield break;
        }

        // Perform anonymous sign-in
        var task = _auth.SignInAnonymouslyAsync();

        // Wait for Firebase to finish
        yield return new WaitUntil(() => task.IsCompleted);

        // Assign user if successful
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
