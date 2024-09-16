using TMPro;
using System;
using UnityEngine;

public class CarStatsManager : MonoBehaviour
{
    public static Action NoTimeLeft;

    [Header ("<size=15>User interface")]
    [SerializeField] private TMP_Text totalCarSmashedText;
    [SerializeField] private TMP_Text totalTimeSpentText;
    [SerializeField] private TMP_Text timerText;

    [Header ("<size=15>Animation")]
    [SerializeField] private Transform timerHolder;

    [Header("Values")]
    [SerializeField] private float clockTimer = 50;
    [SerializeField] private float totalTimePlayed = 0;
    [SerializeField] private float healthShakeCounter = 0;
    [SerializeField] private float totalCarSmashedCount = 0;

    bool isNotified = false;

    private void OnEnable()
    {
        TrafficCarController.CarCollided += OnCarCollided;
        CarController.TollHit += OnTollHit;
    }

    private void OnDisable()
    {
        TrafficCarController.CarCollided -= OnCarCollided;
        CarController.TollHit -= OnTollHit;
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
        //LoseTime(5.0f);
        totalCarSmashedCount += 1;
        totalCarSmashedText.text = "Car smashed: " + totalCarSmashedCount.ToString("0");
    }

    public float GetCarSmashedScore()
    {
        return totalCarSmashedCount * 10;
    }
    
    private void ClockFunction()
    {
        return;

        if (isNotified) return;

        if (clockTimer > 0)
        {
            clockTimer -= Time.deltaTime;
            totalTimePlayed += Time.deltaTime;
            timerText.text = clockTimer.ToString("0") + "s";
        }
        else
        {
            isNotified = true;
            NoTimeLeft?.Invoke();
            clockTimer = 0;
        }
    }

    private void LoseTime (float _timeToLose)
    {
        clockTimer -= _timeToLose;
        if (clockTimer < 0) clockTimer = 0;
    }

    public float GetTotalTimePlayed()
    {
        return totalTimePlayed;
    }

    private void OnTollHit()
    {
        clockTimer += 100;
    }
}
