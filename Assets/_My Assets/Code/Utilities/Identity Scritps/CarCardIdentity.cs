using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class CarCardIdentity : MonoBehaviour
{
    [SerializeField] private CarsName carsName;
    [SerializeField] private CarDetailsData carDetailsData;

    [Header("<size=15>SHINE ANIMATION")]
    [SerializeField] private GameObject shineObj;
    [SerializeField] private Transform endPositionObj;

    [Header ("<size=15>USER INTERFACE")]
    [SerializeField] private Button selectedButton;
    [SerializeField] private RawImage displayImage;
    [SerializeField] private TMP_Text requriedTicketsText;
    [SerializeField] private GameObject ticketImage;
    [SerializeField] private GameObject oilImage;

    int carIndex;
    int requriedTicketsCount;
    Vector3 shineStartPosition;
    string playerPrefTag;

    private void Awake()
    {
        ticketImage.gameObject.SetActive(false);
        oilImage.gameObject.SetActive(false);

        playerPrefTag = carDetailsData.carDetail[(int)carsName].carName;
        if (carDetailsData.carDetail[(int)carsName].purchaseWay == PurchaseWay.FREE)
        {
            PlayerPrefs.SetInt(playerPrefTag, 1);
        }
        else if (carDetailsData.carDetail[(int)carsName].purchaseWay == PurchaseWay.OIL)
        {
            oilImage.gameObject.SetActive(true);
        }
        else if (carDetailsData.carDetail[(int)carsName].purchaseWay == PurchaseWay.TICKETS)
        {
            ticketImage.gameObject.SetActive(true);
        }
    }

    private void Start()
    {
        shineStartPosition = shineObj.transform.localPosition;
        displayImage = GetComponent<RawImage>();
        LoadCarDetails();
    }

    private void LoadCarDetails()
    {
        carIndex = (int)carsName;

        int carUnlockStatus = PlayerPrefs.GetInt(playerPrefTag, 0);
        PurchaseWay purchaseWay = carDetailsData.carDetail[carIndex].purchaseWay;
        requriedTicketsCount = carDetailsData.carDetail[carIndex].requriedTickets;
        
        switch (carUnlockStatus)
        {
            case 0:
                requriedTicketsText.text = requriedTicketsCount.ToString();
                switch (purchaseWay)
                {
                    case PurchaseWay.TICKETS:
                        ticketImage.SetActive (true);   
                        break;
                    case PurchaseWay.OIL:
                        oilImage.SetActive (true);   
                        break;
                }
            break;
            case 1:
                requriedTicketsText.text = "OWNED";
                ticketImage.SetActive (false);   
                oilImage.SetActive (false);   
            break;
        }

        if (carDetailsData.carDetail[carIndex].isSelected)
        {
            requriedTicketsText.text = "SELECTED";
            ticketImage.SetActive(false);
            oilImage.SetActive(false);
            transform.localScale = Vector3.one * 1.1f;
            displayImage.texture = carDetailsData.carDetail[carIndex].selectedSprite;

            ShineAnimation();
        }
        else
        {
            transform.localScale = Vector3.one;
            displayImage.texture = carDetailsData.carDetail[carIndex].unSelectedSprite;
        }
    }

    public void UpdateUi()
    {
        LoadCarDetails();
    }

    private void ShineAnimation()
    {
        shineObj.transform.localPosition = shineStartPosition;
        LeanTween.moveLocal(shineObj, endPositionObj.localPosition, 0.35f);
    }
}




