using TMPro;
using UnityEngine;
using System.Collections;
using System;

public class GameoverController : MonoBehaviour
{
    public static Action Gameover;

    UiManager uiManager;
    AdsManager adsManager;

    [SerializeField] private RenderTexture camRenderTexture;
    [SerializeField] private TMP_Text gameoverReasonText;
    [SerializeField] private Camera ssCam;

    WaitForSeconds pointTwo = new WaitForSeconds(0.2f);

    bool isGameover = false;

    private void Start()
    {
        uiManager = UiManager.instance;
        adsManager = AdsManager.instance;
    }

    private void OnEnable()
    {
        CarStatsManager.NoTimeLeft += OnTimeComplete;
    }

    private void OnDisable()
    {
        CarStatsManager.NoTimeLeft -= OnTimeComplete;
    }

    private void Update()
    {
        if (isGameover)
        {
            if (Time.timeScale > 0)
            {
                Time.timeScale = 0;
            }
        }
    }

    private void OnTimeComplete()
    {
        Gameover?.Invoke();
        StartCoroutine(nameof(TakeScreenshotCoroutine));
    }

    private IEnumerator TakeScreenshotCoroutine()
    {
        yield return pointTwo;

        Time.timeScale = 0.0f;  
        // Create a RenderTexture with the desired resolution
        ssCam.targetTexture = camRenderTexture;
        ssCam.gameObject.SetActive(true);

        // Render the camera's view to the RenderTexture
        ssCam.Render();

        // Wait for the end of the frame to ensure the rendering is done
        yield return new WaitForEndOfFrame();

        // Set the active RenderTexture
        RenderTexture.active = camRenderTexture;

        // Create a Texture2D to store the screenshot
        Texture2D screenshot = new Texture2D(1080, 1920, TextureFormat.RGB24, false);
        screenshot.ReadPixels(new Rect(0, 0, 1080, 1920), 0, 0);
        screenshot.Apply();

        // Reset the active RenderTexture
        ssCam.targetTexture = null;
        RenderTexture.active = null;

        uiManager.OpenCanvasWithoutShutter(CanvasNames.GAMEOVER);
        ssCam.gameObject.SetActive(false);

        adsManager.ShowInterstitialAd();
        
        isGameover = true;
    }
}
