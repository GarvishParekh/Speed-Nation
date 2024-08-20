using TMPro;
using System;
using UnityEngine;

public class CarStatsManager : MonoBehaviour
{
    public static Action NoHealthLeft;
    public static Action NoFuelLeft;
    [Header ("<size=15>User interface")]
    [SerializeField] private TMP_Text fuelCountText;
    [SerializeField] private TMP_Text resultFuelCountText;
    [SerializeField] private TMP_Text healthCountText;
    [SerializeField] private TMP_Text totalCarSmashedText;

    [Header ("<size=15>Animation")]
    [SerializeField] private Transform fuelHolder;
    [SerializeField] private Transform healthHolder;
    [Header ("Values")]
    [SerializeField] private float fuelCount = 100;
    [SerializeField] private float fuelConsuptionRate = 0.5f;
    [SerializeField] private float healthCount = 100;
    [SerializeField] private float healthShakeCounter = 0;
    [SerializeField] private float totalCarSmashedCount = 0;

    bool isNotified = false;

    private void OnEnable()
    {
        TrafficCarController.CarCollided += OnCarCollided;
    }

    private void OnDisable()
    {
        TrafficCarController.CarCollided -= OnCarCollided;
    }

    private void Update()
    {
        FuelConsuption(fuelConsuptionRate);
        HealthHolderShake();
    }

    private void FuelConsuption(float rate)
    {
        if (isNotified) return;

        fuelCount -= rate * Time.deltaTime;
        if (fuelCount < 0)
        {
            fuelCount = 0;
            isNotified = true;
            NoFuelLeft?.Invoke();
        }
        fuelCountText.text = fuelCount.ToString("0") + "%";
        resultFuelCountText.text = "Fuel left: " + fuelCount.ToString("0") + "%";

        if (fuelCount < 90)
        {
            fuelHolder.localPosition = Vector3.zero + UnityEngine. Random.insideUnitSphere * 4f;
        }
        else
        {
            fuelHolder.localPosition = Vector3.zero;
        }
    }

    private void LoseHealth(float loseAmount)
    {
        if (isNotified) return;

        healthCount -= loseAmount;
        if (healthCount <= 0) healthCount = 0;  

        if (healthCount <= 0)
        {
            NoHealthLeft?.Invoke();
            isNotified = true; 
        }

        healthCountText.text = healthCount.ToString("0") + "%";
        healthShakeCounter = 0.5f;
    }

    private void HealthHolderShake()
    {
        if (healthShakeCounter > 0)
        {
            healthHolder.localPosition = Vector3.zero + UnityEngine.Random.insideUnitSphere * 15f;
            healthShakeCounter -= Time.deltaTime;
            return;
        }
        healthHolder.localPosition = Vector3.zero;
    }

    private void OnCarCollided()
    {
        LoseHealth(10);
        totalCarSmashedCount += 1;
        totalCarSmashedText.text = "Car smashed: " + totalCarSmashedCount.ToString("0");
    }

    public float GetFuelScore()
    {
        return fuelCount * 10;
    }

    public float GetCarSmashedScore()
    {
        return totalCarSmashedCount * 10;
    }
}
