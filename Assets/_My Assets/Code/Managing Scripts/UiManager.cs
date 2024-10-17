using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    public static UiManager instance;
    EconomyManager economyManager;

    [Header("<size=15>SCRIPTS")]
    [SerializeField] private StoreManager storeManager;

    [Header("<size=15>SCRIPTABLE OBJECT")]
    [SerializeField] private CarDetailsData carDetailsData;

    [Header ("<size=15>UI COMPONENTS")]
    [SerializeField] private List<CanvasIdentity> uiCanvas;
    [SerializeField] private List<CanvasIdentity> popUpCanvas;
    [SerializeField] private GameObject shutter;
    [SerializeField] private TMP_Text buyInfoCarNameText;
    [SerializeField] private TMP_Text buyInfoCarPriceText;
    [SerializeField] private Image currencyImage;
    [SerializeField] private Sprite ticketSprite;
    [SerializeField] private Sprite oilSprite;
    [SerializeField] private Button buyButton;

    [Header ("<size=15>CAMERAS")]
    [SerializeField] private GameObject mainMenuCam;
    [SerializeField] private GameObject garageCam;

    WaitForSeconds shutterTime = new WaitForSeconds(1f);

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        economyManager = EconomyManager.instance;
    }

    public void OpenCanvasWithShutter(CanvasNames desireCanvas)
    {
        Debug.Log("Start canvas routine");
        StartCoroutine(nameof(OpenUICanvas), desireCanvas);
    }

    public void OpenCanvasWithoutShutter(CanvasNames desireName)
    {
        foreach (CanvasIdentity canvasInfo in uiCanvas)
        {
            if (canvasInfo.GetCanvasName() == desireName)
            {
                canvasInfo.GetComponent<ICanvasController>().EnableCanvas();
            }
            else
            {
                canvasInfo.GetComponent<ICanvasController>().DisableCanvas();
            }
        }
    }

    private IEnumerator OpenUICanvas(CanvasNames desireName)
    {
        float waitTime = 1;
        Debug.Log("Routine started");
        CloseShutter();
        
        while (waitTime > 0)
        {
            waitTime -= Time.unscaledDeltaTime;
            yield return null;  
        }
        foreach (CanvasIdentity canvasInfo in uiCanvas)
        {
            if (canvasInfo.GetCanvasName() == desireName)
            {
                canvasInfo.GetComponent<ICanvasController>().EnableCanvas();
            }
            else
            {
                canvasInfo.GetComponent<ICanvasController>().DisableCanvas();
            }
        }

        switch (desireName)
        {
            case CanvasNames.MAIN_MENU:
                mainMenuCam.SetActive(true);
                garageCam.SetActive(false);
                break;
            case CanvasNames.GARAGE:
                mainMenuCam.SetActive(false);
                garageCam.SetActive(true);
                break;
        }
        OpenShutter();
        yield return null;      
    }

    public void OpenPopUp(CanvasNames desireCanvas)
    {
        foreach (CanvasIdentity canvasInfo in popUpCanvas)
        {
            if (canvasInfo.GetCanvasName() == desireCanvas)
            {
                canvasInfo.GetComponent<ICanvasController>().EnableCanvas();
            }
        }
    }

    public void ClosePopUp(CanvasNames desireCanvas)
    {
        foreach (CanvasIdentity canvasInfo in popUpCanvas)
        {
            if (canvasInfo.GetCanvasName() == desireCanvas)
            {
                canvasInfo.GetComponent<ICanvasController>().DisableCanvas();
            }
        }
    }

    public void StartSceneChangeRoutine(string sceneName)
    {
        StartCoroutine(nameof(ChangeScene), sceneName);
    }

    private IEnumerator ChangeScene(string sceneName)
    {
        float waitTime = 1;
        Debug.Log("Routine started");
        CloseShutter();
        while (waitTime > 0)
        {
            waitTime -= Time.unscaledDeltaTime;
            yield return null;
        }

        Time.timeScale = 1;
        SceneManager.LoadScene(sceneName);
    }


    public void CloseShutter()
    {
        LeanTween.moveLocal(shutter, new Vector3(620.0f, 0,0), 0.5f).setIgnoreTimeScale(true);
    }

    public void OpenShutter()
    {
        LeanTween.moveLocal(shutter, new Vector3(-3000f, 0, 0), 0.5f).setIgnoreTimeScale(true);
    }

    public void UpdateBuyCarPanel(int carIndex)
    {
        string carName = carDetailsData.carDetail[carIndex].carName;
        int carPrice = carDetailsData.carDetail[carIndex].requriedTickets;
        PurchaseWay purchaseWay = carDetailsData.carDetail[carIndex].purchaseWay; 

        switch (purchaseWay)
        {
            case PurchaseWay.TICKETS:
                currencyImage.sprite = ticketSprite;
                break;
            case PurchaseWay.OIL:
                currencyImage.sprite = oilSprite;
                break;
        }

        buyInfoCarNameText.text = $"{carName}";
        buyInfoCarPriceText.text = $"{carPrice}";

        buyButton.onClick.RemoveAllListeners();
        buyButton.onClick.AddListener(() => BuyButton(carIndex));
    }

    private void BuyButton(int index)
    {
        ClosePopUp(CanvasNames.BUY_CAR_CANVAS);
        string carName = carDetailsData.carDetail[index].carName;
        int carPrice = carDetailsData.carDetail[index].requriedTickets;

        PurchaseWay purchaseWay = carDetailsData.carDetail[index].purchaseWay;

        switch (purchaseWay)
        {
            case PurchaseWay.TICKETS:
                economyManager.DebitTickets(carPrice);
                break;
            case PurchaseWay.OIL:
                economyManager.DebitOil(carPrice);
                break;
        }

        PlayerPrefs.SetInt(carName, 1);
        PlayerPrefs.SetInt(ConstantKeys.SELECTED_CAR, index);

        storeManager._SelectCarButton(index);
    }

    public void ShowAdsNotAvailalbe(bool check)
    {
        if (check) OpenPopUp(CanvasNames.TRY_AGAIN_LATER);
        else ClosePopUp(CanvasNames.TRY_AGAIN_LATER);
    }

    public void ThankYouForPurchase(bool check)
    {
        if (check) OpenPopUp(CanvasNames.PURCHASE_SUCESSFULL);
        else ClosePopUp(CanvasNames.PURCHASE_SUCESSFULL);
    }

}
