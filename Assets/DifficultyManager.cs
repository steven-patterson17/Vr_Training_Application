using UnityEngine;
using UnityEngine.UI;

public class DifficultyManager : MonoBehaviour
{
    public PingPongLauncher launcher;
    public MetricsBoardUI metricsBoard;

    // Difficulty buttons
    public GameObject beginnerButton;
    public GameObject intermediateButton;
    public GameObject advancedButton;

    // Sliders to sync when difficulty changes
    public Slider launchForceSlider;
    public Slider fireRateSlider;

    public void SetBeginner()
    {
        Debug.Log("SetBeginner() CALLED");

        launcher.difficulty = Difficulty.Beginner;
        launcher.SendMessage("ApplyDifficulty");
        HighlightButton(Difficulty.Beginner);

        Debug.Log("About to call SyncSlidersToLauncher()");

        SyncSlidersToLauncher();
    }



    public void SetIntermediate()
    {
        launcher.difficulty = Difficulty.Intermediate;
        launcher.SendMessage("ApplyDifficulty");
        HighlightButton(Difficulty.Intermediate);

        SyncSlidersToLauncher();
    }

    public void SetAdvanced()
    {
        launcher.difficulty = Difficulty.Advanced;
        launcher.SendMessage("ApplyDifficulty");
        HighlightButton(Difficulty.Advanced);

        SyncSlidersToLauncher();
    }

    // Sync UI sliders to the values the launcher applied
    private void SyncSlidersToLauncher()
    {
        Debug.Log("Syncing sliders: " + launcher.currentLaunchForce + " / " + launcher.currentFireRate);

        if (launchForceSlider != null)
            launchForceSlider.value = launcher.currentLaunchForce;

        if (fireRateSlider != null)
            fireRateSlider.value = launcher.currentFireRate;
    }


    // Button highlighting
    private void HighlightButton(Difficulty difficulty)
    {
        SetButtonColor(beginnerButton, Color.white);
        SetButtonColor(intermediateButton, Color.white);
        SetButtonColor(advancedButton, Color.white);

        switch (difficulty)
        {
            case Difficulty.Beginner:
                SetButtonColor(beginnerButton, Color.green);
                break;

            case Difficulty.Intermediate:
                SetButtonColor(intermediateButton, Color.yellow);
                break;

            case Difficulty.Advanced:
                SetButtonColor(advancedButton, Color.red);
                break;
        }
    }

    private void SetButtonColor(GameObject button, Color color)
    {
        var image = button.GetComponent<Image>();
        if (image != null)
            image.color = color;
    }
}
