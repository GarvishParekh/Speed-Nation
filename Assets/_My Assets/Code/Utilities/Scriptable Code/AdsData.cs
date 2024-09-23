using System;
using UnityEngine;

public enum DeviceType
{
    ANDROID,
    iOS
}

public enum RewardAvailability
{
    AVAILABLE,
    UNAVAILABLE
}

[CreateAssetMenu(fileName = "Ads Data", menuName = "AdsData")]
public class AdsData : ScriptableObject
{
    public DeviceType deviceType;
    public RewardAvailability rewardAvailability;

    [Space]
    public AdsID androidAdsID;
    public AdsID iOSAdsID;
}

[Serializable]
public class AdsID
{
    public string interstitialID;
    public string rewardedID;
}

