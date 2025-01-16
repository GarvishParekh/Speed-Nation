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
    public static Action<Transform> crossedMidPoint;
    public static Action Gameover;
    public static Action HealthCompleted;

    // login test 
    public static Action InitiateSignIn;

    // economy related actions
    public static Action NoEnoghtBalance;
    public static Action<int> rewardOils;
    public static Action<int> rewardTickets;

    // gameover reward ads watched
    public static Action GameoverRewardAdsWatched;

    // networking
    public static Action<SignInType, bool> SignedInStatus;
}
