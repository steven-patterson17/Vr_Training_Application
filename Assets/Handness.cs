using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Keeps track of which hand the user
/// is training
/// </summary>
public class HandednessUI : MonoBehaviour
{
    public void SetRightHanded()
    {
        SessionMetricsManager.IsLeftHanded = false;
    }

    public void SetLeftHanded()
    {
        SessionMetricsManager.IsLeftHanded = true;

    }

}
