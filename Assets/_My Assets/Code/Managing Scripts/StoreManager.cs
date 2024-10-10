using System.Collections.Generic;
using UnityEngine;


public class StoreManager : MonoBehaviour
{
    UiManager uiManager;
    [SerializeField] private CarsData[] carsData;
    [SerializeField] private CarDetailsData carDetailData;
    [SerializeField] private List<CarCardIdentity> carCards = new List<CarCardIdentity>();

    int selectedCar = 0;
    int garageDisplayCar = 0;

    private void Awake()
    {
        DisplayAndUpdateSelectedCar();
    }

    private void Start()
    {
        uiManager = UiManager.instance;
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
        string carName = carDetailData.carDetail[index].carName;
        int buyStatus = PlayerPrefs.GetInt(carName, 0);

        if (buyStatus == 0)
        {
            uiManager.OpenPopUp(CanvasNames.BUY_CAR_CANVAS);
            uiManager.UpdateBuyCarPanel(index);
        }
        else
        {
            PlayerPrefs.SetInt(ConstantKeys.SELECTED_CAR, index);
            DisplayAndUpdateSelectedCar();

            foreach (CarCardIdentity carIdentity in carCards)
            {
                carIdentity.UpdateUi();
            }
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
