using TMPro;
using System;
using UnityEngine;
using System.Collections.Generic;

public class CarStatsManager : MonoBehaviour
{
    public static Action NoTimeLeft;

    [Header ("<size=15>User interface")]
    [SerializeField] private TMP_Text totalCarSmashedText;
    [SerializeField] private TMP_Text totalTimeSpentText;
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private List<GameObject> healthBar = new List<GameObject>();

    [Header ("<size=15>Animation")]
    [SerializeField] private Transform timerHolder;

    [Header("Values")]
    [SerializeField] private float totalTimePlayed = 0;
    [SerializeField] private float healthShakeCounter = 0;
    [SerializeField] private float totalCarSmashedCount = 0;

    bool isNotified = false;

    private void OnEnable()
    {
        TrafficCarController.CarCollided += OnCarCollided;
        ActionManager.PlayerBoosting += OnPlayerBoost;
    }

    private void OnDisable()
    {
        TrafficCarController.CarCollided -= OnCarCollided;
        ActionManager.PlayerBoosting -= OnPlayerBoost;
    }

    private void Update()
    {
        ClockFunction();
        HealthHolderShake();
    }


    private void HealthHolderShake()
    {
        if (healthShakeCounter > 0)
        {
            timerHolder.localPosition = Vector3.zero + UnityEngine.Random.insideUnitSphere * 15f;
            healthShakeCounter -= Time.deltaTime;
            return;
        }
        timerHolder.localPosition = Vector3.zero;
    }

    private void OnCarCollided()
    {
        LoseHealth();
        //LoseTime(5.0f);
        totalCarSmashedCount += 1;
        totalCarSmashedText.text = "Car smashed: " + totalCarSmashedCount.ToString("0");
    }

    int healtCount = 2;
    private void LoseHealth()
    {
        if (isBoosting) return;

        healthBar[healtCount].SetActive(false);
        healtCount--;
        if (healtCount < 0)
        {
            NoTimeLeft?.Invoke();
        }
    }

    public float GetCarSmashedScore()
    {
        return totalCarSmashedCount * 10;
    }
    
    private void ClockFunction()
    {
        if (isNotified) return;

        totalTimePlayed += Time.deltaTime;
    }


    public float GetTotalTimePlayed()
    {
        return totalTimePlayed;
    }

    bool isBoosting = false;
    private void OnPlayerBoost (bool check)
    {
        isBoosting = check; 
    }
}
