using Firebase.Auth;
using UnityEngine;
using System.Collections;

public class AuthenticationManager : MonoBehaviour
{
    public static FirebaseAuth auth;
    public static FirebaseUser user;

    IEnumerator Start()
    {
        // Let XR Interaction Toolkit + scene objects initialize first
        yield return null;

        auth = FirebaseAuth.DefaultInstance;

        // Already signed in?
        user = auth.CurrentUser;
        if (user != null)
        {
            Debug.Log("Already signed in. UID = " + user.UserId);
            yield break;
        }

        // Sign in anonymously
        var task = auth.SignInAnonymouslyAsync();

        // Wait for Firebase to finish without blocking the main thread
        yield return new WaitUntil(() => task.IsCompleted);

        if (task.Exception == null)
        {
            user = task.Result.User;
            Debug.Log("Signed in anonymously. UID = " + user.UserId);
        }
        else
        {
            Debug.LogError("Anonymous sign-in failed: " + task.Exception);
        }
    }
}
