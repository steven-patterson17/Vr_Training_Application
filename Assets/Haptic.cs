using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR;

/// <summary>
/// Controls the feedback given to the controllers
/// when user hits the ball
/// </summary>
public class Haptic : MonoBehaviour
{
// public XRNode controllerNode = XRNode.RightHand; // set LeftHand or RightHand in Inspector



    [Range(0f, 1f)] public float amplitude = 0.7f;
    public float duration = 0.08f;

    private InputDevice device;
    private XRNode controllerNode;

    void Start()
    {
        //controllerNode = SessionMetricsManager.IsLeftHanded ? XRNode.LeftHand : XRNode.RightHand;
        //device = InputDevices.GetDeviceAtXRNode(controllerNode);
        
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("PingPongBall"))
        {
            SendHaptic();
        }
    }

    void SendHaptic()
    {
        controllerNode = SessionMetricsManager.IsLeftHanded ? XRNode.LeftHand : XRNode.RightHand;
        device = InputDevices.GetDeviceAtXRNode(controllerNode);
        if (!device.isValid)
            device = InputDevices.GetDeviceAtXRNode(controllerNode);


        if (device.TryGetHapticCapabilities(out HapticCapabilities capabilities) &&
            capabilities.supportsImpulse)
        {
            device.SendHapticImpulse(0, amplitude, duration);
        }
    }
}
