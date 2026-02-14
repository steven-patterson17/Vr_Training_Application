using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LauncherSettingsUI : MonoBehaviour
{
    public PingPongLauncher launcher;

    public Slider launchForceSlider;
    public Slider fireRateSlider;

    public TextMeshProUGUI launchForceText;
    public TextMeshProUGUI fireRateText;

    void Start()
    {
        // Initialize slider values from launcher
        launchForceSlider.value = launcher.currentLaunchForce;
        fireRateSlider.value = launcher.currentFireRate;

        UpdateLaunchForceText(launchForceSlider.value);
        UpdateFireRateText(fireRateSlider.value);

        // Add listeners
        launchForceSlider.onValueChanged.AddListener(OnLaunchForceChanged);
        fireRateSlider.onValueChanged.AddListener(OnFireRateChanged);
    }

    public void OnLaunchForceChanged(float value)
    {
        launcher.currentLaunchForce = value;
        UpdateLaunchForceText(value);
    }

    public void OnFireRateChanged(float value)
    {
        launcher.currentFireRate = value;
        UpdateFireRateText(value);
    }

    private void UpdateLaunchForceText(float value)
    {
        launchForceText.text = "Launch Force: " + value.ToString("F1");
    }

    private void UpdateFireRateText(float value)
    {
        fireRateText.text = "Fire Rate: " + value.ToString("F1");
    }
}
