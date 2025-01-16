using TMPro;
using UnityEngine;
using System.Collections;
using System;

public class GameoverController : MonoBehaviour
{
    

    UiManager uiManager;
    AdsManager adsManager;

    [Header("<size=15>SCRIPTABLE")]
    [SerializeField] private AdsData adsData;

    [Space]
    [SerializeField] private RenderTexture camRenderTexture;
    [SerializeField] private TMP_Text gameoverReasonText;
    [SerializeField] private Camera ssCam;
    [SerializeField] private GameObject adsNotAvailableObject;

    WaitForSeconds pointTwo = new WaitForSeconds(0.2f);
    WaitForSeconds pointFive = new WaitForSeconds(0.5f);

    bool isGameover = false;

    private void Start()
    {
        uiManager = UiManager.instance;
        adsManager = AdsManager.instance;
    }

    private void OnEnable()
    {
        ActionManager.HealthCompleted += OnHealthCompleted;
    }

    private void OnDisable()
    {
        ActionManager.HealthCompleted -= OnHealthCompleted;
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

    private void OnHealthCompleted()
    {
        if (adsManager.AdsAvailable()) adsNotAvailableObject.SetActive(false);
        else adsNotAvailableObject.SetActive(true);

        ActionManager.Gameover?.Invoke();
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

        isGameover = true;

        yield return new WaitForSecondsRealtime(0.5f);
        ShowAds();
    }

    private void ShowAds()
    {
        if (adsData.noAdsCard == NoAdsCard.IN_ACTIVE)
            adsManager.ShowInterstitialAd();
    }
}
