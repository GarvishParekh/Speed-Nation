using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum CarsName
{
    SEDAN,
    HATCHBACK,
    MUSCLE,
    SUPER
}

public enum LockStatus
{
    LOCKED,
    UNLOCKED
}
public class CarCardIdentity : MonoBehaviour
{
    [SerializeField] private CarsName carsName;
    [SerializeField] private CarDetailsData carDetailsData;

    [Header ("<size=15>USER INTERFACE")]
    [SerializeField] private Button selectedButton;
    [SerializeField] private GameObject lockScreen;
    [SerializeField] private TMP_Text requriedScoreText;

    private void Start()
    {
        LoadCarDetails();
    }

    private void LoadCarDetails()
    {
        int carIndex = (int)carsName;

        int currentScore = PlayerPrefs.GetInt(ConstantKeys.HIGHSCORE, 0);
        int requriedScoreCount = carDetailsData.carDetail[carIndex].requriedScore;

        if (requriedScoreCount > currentScore)
        {
            selectedButton.interactable = false;
            lockScreen.SetActive(true);
            requriedScoreText.text = $"Score <color=#E26E20>{requriedScoreCount} PTS</color> to unlock";
        }
        else lockScreen.SetActive(false);


        if (carDetailsData.carDetail[carIndex].isSelected)
        {
            transform.localScale = Vector3.one * 1.1f;
            selectedButton.image.sprite = carDetailsData.carDetail[carIndex].selectedSprite;
        }
        else
        {
            transform.localScale = Vector3.one;
            selectedButton.image.sprite = carDetailsData.carDetail[carIndex].unSelectedSprite;
        }
    }

    public void UpdateUi()
    {
        LoadCarDetails();
    }
}




