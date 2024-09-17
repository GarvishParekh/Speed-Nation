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
    [SerializeField] private Button selectedButton;

    private void Start()
    {
        LoadCarDetails();
    }

    private void LoadCarDetails()
    {
        int carIndex = (int)carsName;
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




