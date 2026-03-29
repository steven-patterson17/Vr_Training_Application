using UnityEngine;
using TMPro;

public class QuitSessionController : MonoBehaviour
{
    public PingPongLauncher launcher;
    public SessionMetricsManager sessionManager;

    public GameObject postSessionPanel;

    public TextMeshProUGUI hitsText;
    public TextMeshProUGUI missesText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI avgSpeedText;
    public TextMeshProUGUI avgSpinText;
    public TextMeshProUGUI sessionTimeText;
    public TextMeshProUGUI accuracyText;

    public void QuitSession()
    {
        // 1. Stop launcher
        if (launcher != null)
            launcher.enabled = false;

        // 2. End session + upload
        if (sessionManager != null)
            sessionManager.EndSessionAndUpload();

        // 3. Show post-session UI
        ShowPostSessionData();
    }

    public void ExitGame()
    {
        // Quit the application
        Application.Quit();

        // Extra safety for the editor
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }


    private void ShowPostSessionData()
    {
        var metrics = sessionManager.GetCurrentMetrics();

        hitsText.text = "Hits: " + metrics.hits;
        missesText.text = "Misses: " + metrics.misses;
        scoreText.text = "Score: " + metrics.score;
        avgSpeedText.text = "Avg Speed: " + metrics.speed.ToString("F1") + "m/s";
        avgSpinText.text = "Avg Spin: " + metrics.spin.ToString("F1") + "rad/s";
        sessionTimeText.text = "Session Time: " + metrics.sessionTime.ToString("F1") + "s";

        float accuracy = 0f;
        int total = metrics.hits + metrics.misses;

        if (total > 0)
            accuracy = (float)metrics.hits / total;

        accuracyText.text = "Accuracy: " + (accuracy * 100f).ToString("F1") + "%";

        postSessionPanel.SetActive(true);
    }

}
