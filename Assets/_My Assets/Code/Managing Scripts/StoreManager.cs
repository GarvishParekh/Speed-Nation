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

    int selectedCar = 0;
    int garageDisplayCar = 0;

    private void Start()
    {
        selectedCar = PlayerPrefs.GetInt(ConstantKeys.SELECTED_CAR);
        DisplayCar(selectedCar);
    }

    private void DisplayCar(int _selectedCar)
    {
        foreach (CarsData carData in carsData)
        {
            carData.carModel.SetActive(false);   
        }
        carsData[_selectedCar].carModel.SetActive(true);
    }

    public void _NextButton()
    {
        garageDisplayCar += 1;
        if (garageDisplayCar >= carsData.Length)
            garageDisplayCar = 0;

        PlayerPrefs.SetInt(ConstantKeys.SELECTED_CAR, garageDisplayCar);
        DisplayCar(garageDisplayCar);
    }

    public void _PreviousButton()
    {
        garageDisplayCar -= 1;
        if (garageDisplayCar < 0)
            garageDisplayCar = carsData.Length-1;

        PlayerPrefs.SetInt(ConstantKeys.SELECTED_CAR, garageDisplayCar);
        DisplayCar(garageDisplayCar);
    }
}

[System.Serializable]
public class CarsData
{
    public GameObject carModel;
}
