using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum CarsName
{
    SEDAN,
    HATCHBACK,
    MUSCLE
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
    [SerializeField] private TMP_Text carNameText;
    [SerializeField] private TMP_Text carDescriptionText;
    [SerializeField] private Image carIcon;
    [SerializeField] private Button selectedButton;
    [SerializeField] private TMP_Text selectedText;

    private void Start()
    {
        LoadCarDetails();
    }

    private void LoadCarDetails()
    {
        int carIndex = (int)carsName;
        carNameText.text = carDetailsData.carDetail[carIndex].carName;
        carDescriptionText.text = carDetailsData.carDetail[carIndex].carDescription;
        carIcon.sprite = carDetailsData.carDetail[carIndex].carIconSprite;

        if (carDetailsData.carDetail[carIndex].isSelected)
        {
            transform.localScale = Vector3.one * 1.1f;
            selectedButton.image.color = carDetailsData.selectedColor;
            selectedText.color = carDetailsData.unSelectedColor;
            selectedText.text = "Selected";
        }
        else
        {
            transform.localScale = Vector3.one;
            selectedButton.image.color = carDetailsData.unSelectedColor;
            selectedText.color = carDetailsData.selectedColor;
            selectedText.text = "Select";
        }
    }

    public void UpdateUi()
    {
        LoadCarDetails();
    }
}



