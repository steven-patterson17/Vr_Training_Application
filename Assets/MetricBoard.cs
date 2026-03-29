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
    public TextMeshProUGUI difficultyText;
    public TextMeshProUGUI returnAngleText;
    public TextMeshProUGUI returnSpeedText;
    public TextMeshProUGUI returnSpinText;
    public TextMeshProUGUI swingTypeText;



    void Update()
    {
        if (speedProvider != null)
            speedText.text = "Speed: " + speedProvider.Speed.ToString("F2") + " m/s";

        if (distanceProvider != null)
            distanceText.text = "Distance: " + distanceProvider.Distance.ToString("F2") + " m";
    }
    public void SetDifficulty(Difficulty difficulty)
    {
        difficultyText.text = "Difficulty: " + difficulty.ToString();
    }

    public void SetReturnMetrics(float angle, float speed, float spin)
    {
        returnAngleText.text = "Return Angle:" + $"{angle:F1}°";
        returnSpeedText.text = "Return Speed:" + $"{speed:F1} m/s";
        returnSpinText.text = "Return Spin:" + $"{spin:F1} rad/s";
    }

    public void SetSwingType(string swingtype)
    {
        swingTypeText.text = "Swing : " + swingtype;
    }


}
