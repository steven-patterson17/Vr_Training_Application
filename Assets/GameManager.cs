using UnityEngine;

/// <summary>
/// Manages menu navigation between different UI panels,
/// such as the main menu and game type selection screen.
/// </summary>
/// <summary>
/// Manages the panels that show up when users switches
/// to a gamemode or ends the session
/// </summary>
public class MenuManager : MonoBehaviour
{
    /// <summary>
    /// The main menu panel displayed at startup.
    /// </summary>
    [Header("Panels")]
    public GameObject mainPanel;

    /// <summary>
    /// The panel that displays available game types.
    /// </summary>
    public GameObject gameTypesPanel;

    /// <summary>
    /// Opens the game types panel and hides the main menu panel.
    /// </summary>
    public void OpenGameTypes()
    {
        mainPanel.SetActive(false);
        gameTypesPanel.SetActive(true);
    }

    /// <summary>
    /// Closes the game types panel and returns to the main menu panel.
    /// </summary>
    public void CloseGameTypes()
    {
        gameTypesPanel.SetActive(false);
        mainPanel.SetActive(true);
    }
}
