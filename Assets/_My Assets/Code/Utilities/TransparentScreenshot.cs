using UnityEngine;
using System.IO;

public class TransparentScreenshot : MonoBehaviour
{
    public Camera renderCamera; // Assign the camera you want to take the screenshot from
    public int imageWidth = 1920;
    public int imageHeight = 1080;

    [ContextMenu("TAKE SCREENSHOT")]
    public void TakeScreenshot()
    {
        // Create a RenderTexture with RGBA32 format to preserve transparency
        RenderTexture renderTexture = new RenderTexture(imageWidth, imageHeight, 24, RenderTextureFormat.ARGB32);
        renderCamera.targetTexture = renderTexture;

        // Render the camera's view to the RenderTexture
        RenderTexture.active = renderTexture;
        renderCamera.Render();

        // Create a new Texture2D with RGBA32 format
        Texture2D screenshot = new Texture2D(imageWidth, imageHeight, TextureFormat.RGBA32, false);
        screenshot.ReadPixels(new Rect(0, 0, imageWidth, imageHeight), 0, 0);
        screenshot.Apply();

        // Reset the camera's target texture and active RenderTexture
        renderCamera.targetTexture = null;
        RenderTexture.active = null;

        // Encode texture to PNG (which supports transparency)
        byte[] bytes = screenshot.EncodeToPNG();

        // Save the PNG to the persistent data path
        string filePath = Path.Combine(Application.persistentDataPath, "TransparentScreenshot.png");
        File.WriteAllBytes(filePath, bytes);

        Debug.Log("Transparent screenshot saved to: " + filePath);

        // Clean up
        Destroy(renderTexture);
        Destroy(screenshot);
    }
}
