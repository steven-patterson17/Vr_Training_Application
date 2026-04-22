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
    /// The Firebase authentication instance used to manage user sign-in and authentication state.
    /// </summary>
    public static FirebaseAuth auth;

    /// <summary>
    /// The currently authenticated Firebase user.
    /// </summary>
    public static FirebaseUser user;

    /// <summary>
    /// Unity coroutine that initializes Firebase authentication.
    /// Checks if a user is already signed in and, if not, performs anonymous sign-in.
    /// </summary>
    /// <returns>
    /// An IEnumerator used by Unity to handle asynchronous execution without blocking the main thread.
    /// </returns>
    IEnumerator Start()
    {
        // Let XR Interaction Toolkit + scene objects initialize first
        yield return null;

        /// <summary>
        /// Initializes the default Firebase authentication instance.
        /// </summary>
        auth = FirebaseAuth.DefaultInstance;

        // Already signed in?
        user = auth.CurrentUser;

        /// <summary>
        /// If a user is already authenticated, reuse the existing session and skip sign-in.
        /// </summary>
        if (user != null)
        {
            yield break;
        }

        /// <summary>
        /// Initiates anonymous sign-in using Firebase Authentication.
        /// </summary>
        var task = auth.SignInAnonymouslyAsync();

        // Wait for Firebase to finish without blocking the main thread
        yield return new WaitUntil(() => task.IsCompleted);

        /// <summary>
        /// Handles the result of the authentication attempt.
        /// On success, assigns the authenticated user.
        /// On failure, logs an error message.
        /// </summary>
        if (task.Exception == null)
        {
            user = task.Result.User;
        }
        else
        {
        }
    }
}