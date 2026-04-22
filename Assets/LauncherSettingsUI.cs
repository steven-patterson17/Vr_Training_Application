using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Controls the firerate and launch force 
/// text to show what the user selected
/// </summary>
public class LauncherSettingsUI : MonoBehaviour
{
    public PingPongLauncher launcher; // ✅ FIXED TYPE

    public Slider launchForceSlider;
    public Slider fireRateSlider;

    public TextMeshProUGUI launchForceText;
    public TextMeshProUGUI fireRateText;

    void OnEnable()
    {
        if (launcher == null)
        {
            Debug.LogError("Launcher is NOT assigned!");
            return;
        }

        if (launchForceSlider == null || fireRateSlider == null)
        {
            Debug.LogError("One or more sliders are NOT assigned!");
            return;
        }

        if (launchForceText == null || fireRateText == null)
        {
            Debug.LogError("One or more text fields are NOT assigned!");
            return;
        }

        // Initialize slider values from launcher
        launchForceSlider.value = launcher.currentLaunchForce;
        fireRateSlider.value = launcher.currentFireRate;

        UpdateLaunchForceText(launchForceSlider.value);
        UpdateFireRateText(fireRateSlider.value);

        // Add listeners
        launchForceSlider.onValueChanged.AddListener(OnLaunchForceChanged);
        fireRateSlider.onValueChanged.AddListener(OnFireRateChanged);
    }

    void OnDisable()
    {
        if (launchForceSlider != null)
            launchForceSlider.onValueChanged.RemoveListener(OnLaunchForceChanged);

        if (fireRateSlider != null)
            fireRateSlider.onValueChanged.RemoveListener(OnFireRateChanged);
    }

    public void OnLaunchForceChanged(float value)
    {
        if (launcher == null) return;

        launcher.currentLaunchForce = value;
        UpdateLaunchForceText(value);
    }

    public void OnFireRateChanged(float value)
    {
        if (launcher == null) return;

        launcher.currentFireRate = value;
        UpdateFireRateText(value);
    }

    private void UpdateLaunchForceText(float value)
    {
        if (launchForceText != null)
            launchForceText.text = "Launch Force: " + value.ToString("F1");
    }

    private void UpdateFireRateText(float value)
    {
        if (fireRateText != null)
            fireRateText.text = "Fire Rate: " + value.ToString("F1");
    }
}