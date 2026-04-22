using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Firebase.Database;
using Firebase.Auth;
using Firebase.Extensions;   // <-- IMPORTANT

public class LeaderboardManager : MonoBehaviour
{
    [Header("UI References")]
    public GameObject leaderboardPanel;          // UI panel to enable
    public TextMeshProUGUI[] leaderboardLines;   // size = 5

    private DatabaseReference dbRef;
    private string userUID;

    void Awake()
    {
        dbRef = FirebaseDatabase.DefaultInstance.RootReference;
    }

    // ⭐ NEW: Automatically load leaderboard when scene starts
    void Start()
    {
        // Make sure leaderboard panel is visible
        if (leaderboardPanel != null)
        {
            leaderboardPanel.SetActive(true);
        }

        // Ensure this object is active
        gameObject.SetActive(true);

        var user = FirebaseAuth.DefaultInstance.CurrentUser;

        if (user == null)
        {
            return;
        }

        userUID = user.UserId;

        // Load leaderboard immediately
        LoadTop5Sessions();
    }

    // OPTIONAL: You can keep this if you still want a button to reopen the panel
    public void OpenLeaderboard()
    {
        if (leaderboardPanel == null)
        {
            Debug.LogError("Leaderboard panel missing!");
            return;
        }
        else
        {
            leaderboardPanel.SetActive(true);
        }

        gameObject.SetActive(true);

        var user = FirebaseAuth.DefaultInstance.CurrentUser;

        if (user == null)
        {
            return;
        }

        userUID = user.UserId;
        LoadTop5Sessions();
    }

    private void LoadTop5Sessions()
    {
        if (string.IsNullOrEmpty(userUID))
        {
            return;
        }


        DatabaseReference sessionsRef = FirebaseDatabase.DefaultInstance
            .GetReference("players")
            .Child(userUID)
            .Child("sessions");

        sessionsRef
            .GetValueAsync()
            .ContinueWithOnMainThread(task =>
            {

                if (task.IsFaulted || task.IsCanceled)
                {
                    return;
                }

                DataSnapshot snapshot = task.Result;
                List<SessionData> allSessions = new List<SessionData>();

                foreach (var child in snapshot.Children)
                {
                    int score = int.Parse(child.Child("score").Value.ToString());
                    int hits = int.Parse(child.Child("hits").Value.ToString());
                    int misses = int.Parse(child.Child("misses").Value.ToString());
                    float sessionTime = float.Parse(child.Child("sessionTime").Value.ToString());

                    allSessions.Add(new SessionData(score, hits, misses, sessionTime));
                }

                // ⭐ SORT BY COMBINED PERFORMANCE SCORE
                allSessions.Sort((a, b) => b.PerformanceScore().CompareTo(a.PerformanceScore()));

                // ⭐ TAKE TOP 5
                List<SessionData> top5 = allSessions.Count > 5 ? allSessions.GetRange(0, 5) : allSessions;

                DisplayLeaderboard(top5);
            });
    }

    private void DisplayLeaderboard(List<SessionData> sessions)
    {
        for (int i = 0; i < leaderboardLines.Length; i++)
        {
            if (leaderboardLines[i] == null)
            {
                continue;
            }

            if (i < sessions.Count)
            {
                var s = sessions[i];

                float accuracy = s.Accuracy() * 100f;
                int attempts = s.Attempts();

                leaderboardLines[i].text =
                    $"#{i + 1}  Score: {s.score}  |  Acc: {accuracy:0}%  |  Attempts: {attempts}";
            }
            else
            {
                leaderboardLines[i].text = $"#{i + 1}  ---";
            }
        }
    }
}

public struct SessionData
{
    public int score;
    public int hits;
    public int misses;
    public float sessionTime;

    public SessionData(int score, int hits, int misses, float sessionTime)
    {
        this.score = score;
        this.hits = hits;
        this.misses = misses;
        this.sessionTime = sessionTime;
    }

    public int Attempts()
    {
        return hits + misses;
    }

    public float Accuracy()
    {
        int attempts = hits + misses;
        return attempts > 0 ? (float)hits / attempts : 0f;
    }

    // ⭐ Combined performance score for ranking
    public float PerformanceScore()
    {
        float accuracy = Accuracy();
        float attempts = Attempts();

        return score * accuracy * Mathf.Log(1f + attempts);
    }
}
