using UnityEngine;
using TMPro;

public class BallSpeedUI : MonoBehaviour
{
    public TextMeshProUGUI speedText;
    private Rigidbody currentBallRB;

    void Update()
    {
        if (currentBallRB != null)
        {
            float speed = currentBallRB.linearVelocity.magnitude;
            speedText.text = "Speed: " + speed.ToString("F2") + " f/s";
        }
    }

    public void SetBall(Rigidbody rb)
    {
        currentBallRB = rb;
    }
}
