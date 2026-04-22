using System.IO;
using UnityEngine;

/// <summary>
/// Allows screenshots to be taken while using the 
/// vr device in the unity scence
/// </summary>
public class Screenshot : MonoBehaviour
{
    public Camera cam;
    public int width = 4000;
    public int height = 1500;

    void Start()
    {
        if (cam == null)
        {
            cam = Camera.main;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            TakeShot();
        }
    }

    void TakeShot()
    {
        if (cam == null)
        {
            Debug.LogError("No camera assigned to Screenshot script.");
            return;
        }

        RenderTexture rt = new RenderTexture(width, height, 24);
        Texture2D image = new Texture2D(width, height, TextureFormat.RGB24, false);

        cam.targetTexture = rt;
        RenderTexture.active = rt;

        cam.Render();

        image.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        image.Apply();

        cam.targetTexture = null;
        RenderTexture.active = null;

        byte[] bytes = image.EncodeToPNG();

        string fileName = "screenshot_" + System.DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".png";
        string path = Path.Combine(Application.persistentDataPath, fileName);
        File.WriteAllBytes(path, bytes);

        Destroy(rt);
        Destroy(image);

        Debug.Log("Saved clean screenshot to: " + path);
    }
}