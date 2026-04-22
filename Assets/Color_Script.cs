using UnityEngine;

/// <summary>
/// Provides colors to objects in the unity scene
/// </summary>
public class CubeColorChanger : MonoBehaviour
{
    // Public field to set color from the Inspector
    public Color cubeColor = Color.black;

    // Reference to the Renderer component
    private Renderer cubeRenderer;

    void Start()
    {
        // Get the Renderer component from the GameObject this script is attached to
        cubeRenderer = GetComponent<Renderer>();

        if (cubeRenderer == null)
        {
            Debug.LogError("No Renderer found on this GameObject. Attach this script to a cube with a Renderer.");
            return;
        }

        // Ensure we are not modifying the shared material (affects all objects using it)
        cubeRenderer.material = new Material(cubeRenderer.material);

        // Apply the chosen color
        cubeRenderer.material.color = cubeColor;
    }
}
