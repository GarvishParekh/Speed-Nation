using System;
using UnityEngine;

public class ActionManager : MonoBehaviour
{
    public static Action PlayerOnLane;
    public static Action<Transform> PlayCarSpawned;
    public static Action<bool> PlayerBoosting;
    public static Action countDownCompleted;
    public static Action TurtiolInitilize;
    public static Action<Transform> CarCollided;
    public static Action TrafficKilled;
    public static Action<int> UpdateKillStreak;
    public static Action KillStreakCouterReset;

    // economy related actions
    public static Action NoEnoghtBalance;
    public static Action<int> rewardOils;
    public static Action<int> rewardTickets;
}
