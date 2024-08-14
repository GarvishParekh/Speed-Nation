using TMPro;
using UnityEngine;

public class CarStatsManager : MonoBehaviour
{
    [SerializeField] private TMP_Text fuelCountText;
    [SerializeField] private float fuelCount = 100;
    [SerializeField] private float fuelConsuptionRate = 0.5f;

    [SerializeField] private Transform fuelHolder;

    private void Update()
    {
        FuelConsuption(fuelConsuptionRate);
    }

    private void FuelConsuption(float rate)
    {
        fuelCount -= rate * Time.deltaTime;
        fuelCountText.text = fuelCount.ToString("0") + "%";

        if (fuelCount < 90)
        {
            fuelHolder.localPosition = Vector3.zero + Random.insideUnitSphere * 4f;
        }
        else
        {
            fuelHolder.localPosition = Vector3.zero;
        }
    }
}
