using System.Collections.Generic;
using UnityEngine;

public enum GarageCars
{
    SEDEAN,
    MUSCLE,
    HATCHBACK
}

public class StoreManager : MonoBehaviour
{
    [SerializeField] private CarsData[] carsData;
    [SerializeField] private CarDetailsData carDetailData;
    [SerializeField] private List<CarCardIdentity> carCards = new List<CarCardIdentity>();

    int selectedCar = 0;
    int garageDisplayCar = 0;

    private void Awake()
    {
        DisplayAndUpdateSelectedCar();
    }

    private void DisplayCar(int _selectedCar)
    {
        foreach (CarsData carData in carsData)
        {
            carData.carModel.SetActive(false);   
        }
        carsData[_selectedCar].carModel.SetActive(true);
    }

    public void _SelectCarButton(int index)
    {
        PlayerPrefs.SetInt(ConstantKeys.SELECTED_CAR, index);
        DisplayAndUpdateSelectedCar();

        foreach (CarCardIdentity carIdentity in carCards)
        {
            carIdentity.UpdateUi();
        }
    }

    private void DisplayAndUpdateSelectedCar()
    {
        selectedCar = PlayerPrefs.GetInt(ConstantKeys.SELECTED_CAR);
        foreach (CarDetail carDetail in carDetailData.carDetail)
        {
            if (carDetail.carIndex == selectedCar) carDetail.isSelected = true;
            else carDetail.isSelected = false;
        }
        DisplayCar(selectedCar);
    }
}

[System.Serializable]
public class CarsData
{
    public GameObject carModel;
}
