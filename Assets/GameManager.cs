using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [Header("Panels")]
    public GameObject mainPanel;
    public GameObject gameTypesPanel;

    public void OpenGameTypes()
    {
        mainPanel.SetActive(false);
        gameTypesPanel.SetActive(true);
    }

    public void CloseGameTypes()
    {
        gameTypesPanel.SetActive(false);
        mainPanel.SetActive(true);
    }
}
