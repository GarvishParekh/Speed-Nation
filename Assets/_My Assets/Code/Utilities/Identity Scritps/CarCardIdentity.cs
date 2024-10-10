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

    int carIndex;
    int requriedTicketsCount;
    Vector3 shineStartPosition;
    string playerPrefTag;

    private void Awake()
    {
        playerPrefTag = carDetailsData.carDetail[(int)carsName].carName;
        if (carDetailsData.carDetail[(int)carsName].purchaseWay == PurchaseWay.FREE)
        {
            PlayerPrefs.SetInt(playerPrefTag, 1);
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
        requriedTicketsCount = carDetailsData.carDetail[carIndex].requriedTickets;
        
        switch (carUnlockStatus)
        {
            case 0:
                requriedTicketsText.text = requriedTicketsCount.ToString();
                ticketImage.SetActive (true);   
            break;
            case 1:
                requriedTicketsText.text = "OWNED";
                ticketImage.SetActive (false);   
            break;
        }

        if (carDetailsData.carDetail[carIndex].isSelected)
        {
            requriedTicketsText.text = "SELECTED";
            ticketImage.SetActive(false);
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




