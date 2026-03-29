using Unity.VisualScripting;
using UnityEngine;

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
