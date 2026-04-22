using UnityEngine;
using TMPro;

public class MetricsBoardUI : MonoBehaviour
{
    public static MetricsBoardUI Instance { get; private set; }

    void Awake()
    {
        Instance = this;
    }

    public BallSpeedProvider speedProvider;
    public BallDistanceProvider distanceProvider;

    public TextMeshProUGUI speedText;
    public TextMeshProUGUI distanceText;
    public TextMeshProUGUI returnAngleText;
    public TextMeshProUGUI returnSpeedText;
    public TextMeshProUGUI returnSpinText;
    public TextMeshProUGUI swingTypeText;

    void Update()
    {
        if (speedProvider != null && speedText != null)
        {
            speedText.text = "Speed: " + speedProvider.Speed.ToString("F2") + " m/s";
        }

        if (distanceProvider != null && distanceText != null)
        {
            distanceText.text = "Distance: " + distanceProvider.Distance.ToString("F2") + " m";
        }
    }

    public void SetReturnMetrics(float angle, float speed, float spin)
    {
        if (returnAngleText != null)
            returnAngleText.text = "Return Angle: " + $"{angle:F2}°";

        if (returnSpeedText != null)
            returnSpeedText.text = "Return Speed: " + $"{speed:F2} m/s";

        if (returnSpinText != null)
            returnSpinText.text = "Return Spin: " + $"{spin:F2} rad/s";
    }

    public void SetSwingType(string swingtype)
    {
        if (swingTypeText != null)
            swingTypeText.text = "Swing : " + swingtype;
    }
}