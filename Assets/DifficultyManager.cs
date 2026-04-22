using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Sets the difficulty to the user's chosen choice
/// and syncs the sliders to the values of the 
/// difficulty selected
/// </summary>
public class DifficultyManager : MonoBehaviour
{
    /// <summary>
    /// Reference to the PingPongLauncher that controls ball behavior.
    /// </summary>
    public PingPongLauncher launcher;

    /// <summary>
    /// Reference to the metrics board UI for displaying session data.
    /// </summary>
    public MetricsBoardUI metricsBoard;

    /// <summary>
    /// UI button for selecting beginner difficulty.
    /// </summary>
    public GameObject beginnerButton;

    /// <summary>
    /// UI button for selecting intermediate difficulty.
    /// </summary>
    public GameObject intermediateButton;

    /// <summary>
    /// UI button for selecting advanced difficulty.
    /// </summary>
    public GameObject advancedButton;

    /// <summary>
    /// UI button for selecting random difficulty
    /// </summary>
    public GameObject randomButton;


    /// <summary>
    /// Slider used to adjust and display launch force.
    /// </summary>
    public Slider launchForceSlider;

    /// <summary>
    /// Slider used to adjust and display fire rate.
    /// </summary>
    public Slider fireRateSlider;

    /// <summary>
    /// Sets the game difficulty to Beginner, applies launcher settings,
    /// updates UI highlighting, and synchronizes sliders.
    /// </summary>
    public void SetBeginner()
    {
        launcher.difficulty = Difficulty.Beginner;
        launcher.SendMessage("ApplyDifficulty");
        HighlightButton(Difficulty.Beginner);

        SyncSlidersToLauncher();
    }

    /// <summary>
    /// Sets the game difficulty to Intermediate, applies launcher settings,
    /// updates UI highlighting, and synchronizes sliders.
    /// </summary>
    public void SetIntermediate()
    {
        launcher.difficulty = Difficulty.Intermediate;
        launcher.SendMessage("ApplyDifficulty");
        HighlightButton(Difficulty.Intermediate);

        SyncSlidersToLauncher();
    }

    /// <summary>
    /// Sets the game difficulty to Advanced, applies launcher settings,
    /// updates UI highlighting, and synchronizes sliders.
    /// </summary>
    public void SetAdvanced()
    {
        launcher.difficulty = Difficulty.Advanced;
        launcher.SendMessage("ApplyDifficulty");
        HighlightButton(Difficulty.Advanced);

        SyncSlidersToLauncher();
    }

    public void SetRandom()
    {
        launcher.difficulty = Difficulty.Random;
        launcher.SendMessage("ApplyDifficulty");
        HighlightButton(Difficulty.Random);

        SyncSlidersToLauncher();
    }

    /// <summary>
    /// Synchronizes UI sliders with the current values applied to the launcher.
    /// </summary>
    private void SyncSlidersToLauncher()
    {
        if (launchForceSlider != null)
            launchForceSlider.value = launcher.currentLaunchForce;

        if (fireRateSlider != null)
            fireRateSlider.value = launcher.currentFireRate;
    }

    /// <summary>
    /// Highlights the selected difficulty button and resets others to default color.
    /// </summary>
    /// <param name="difficulty">The selected difficulty level.</param>
    private void HighlightButton(Difficulty difficulty)
    {
        SetButtonColor(beginnerButton, Color.white);
        SetButtonColor(intermediateButton, Color.white);
        SetButtonColor(advancedButton, Color.white);
        SetButtonColor(randomButton, Color.white);

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

            case Difficulty.Random:
                SetButtonColor(randomButton, Color.blue);
                break;
        }
    }

    /// <summary>
    /// Sets the visual color of a UI button.
    /// </summary>
    /// <param name="button">The button GameObject to modify.</param>
    /// <param name="color">The color to apply to the button.</param>
    private void SetButtonColor(GameObject button, Color color)
    {
        var image = button.GetComponent<Image>();
        if (image != null)
            image.color = color;
    }
}
