using System;
using UnityEngine;

public enum RewardAvailability
{
    AVAILABLE,
    UNAVAILABLE
}

public enum NoAdsCard
{
    ACTIVE,
    IN_ACTIVE
}

[CreateAssetMenu(fileName = "Ads Data", menuName = "AdsData")]
public class AdsData : ScriptableObject
{

    [Space]
    public NoAdsCard noAdsCard;
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

