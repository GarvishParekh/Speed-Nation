using System;
using UnityEngine;
using GoogleMobileAds.Api;

public enum AdType
{
    interstitialAd,
    rewardedAd
}

public enum RewardAdPlacement
{
    UserReward,
    DoubleUpReward
}


public class AdsManager : MonoBehaviour
{
    public static Action GetReward;
    public static AdsManager instance;

    [Header (" [SCRIPTABLE OBJECT] ")]
    [SerializeField] private AdsData adsData;

    private string interstitialAdID;
    private string rewardedAdID;
    
    InterstitialAd interstitialAd;
    RewardedAd rewardedAd;

    void Awake()
    {
        CreateSingleton();
    }

    private void CreateSingleton()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);  
        }
    }

    private void Start()
    {
        MobileAds.RaiseAdEventsOnUnityMainThread = true;
        FetchID();
        MobileAds.Initialize(initStatus => 
        {
            LoadInterstitialAd();
            LoadRewardedAd();
        });
    }

    private void FetchID()
    {
        switch (adsData.deviceType)
        {
            case DeviceType.ANDROID:
                Debug.Log("Android ID fetched");
                interstitialAdID = adsData.androidAdsID.interstitialID;
                rewardedAdID = adsData.androidAdsID.rewardedID;
            break;
            
            case DeviceType.iOS:
                Debug.Log("iOS ID fetched");
                interstitialAdID = adsData.iOSAdsID.interstitialID;
                rewardedAdID = adsData.iOSAdsID.rewardedID;
            break;
        }
    }

    #region Show Ads
    // without placement
    private void ShoawAds(AdType adType)
    {
        switch (adType)
        {
            case AdType.interstitialAd:
                #region Show ad
                if (interstitialAd != null && interstitialAd.CanShowAd())
                {
                    Debug.Log("Showing interstitial ad.");
                    interstitialAd.Show();
                }
                else
                {
                    Debug.LogError("Interstitial ad is not ready yet.");
                }
                #endregion
                break;
            case AdType.rewardedAd:
                #region Show ad
                if (rewardedAd != null && rewardedAd.CanShowAd())
                {
                    rewardedAd.Show((AdRequest) =>
                    {
                        Debug.Log("Showing rewarded ads");
                    });
                }
                #endregion
                break;
        }
    }

    // with placement
    private void ShoawAds(AdType adType, RewardAdPlacement rewardAdPlacement)
    {
        switch (adType)
        {
            case AdType.interstitialAd:
                #region Show ad
                if (interstitialAd != null && interstitialAd.CanShowAd())
                {
                    Debug.Log("Showing interstitial ad.");
                    interstitialAd.Show();
                }
                else
                {
                    Debug.LogError("Interstitial ad is not ready yet.");
                }
                #endregion
                break;
            case AdType.rewardedAd:
                #region Show ad
                if (rewardedAd != null && rewardedAd.CanShowAd())
                {
                    rewardedAd.Show((AdRequest) =>
                    {
                        GetReward?.Invoke();
                        Debug.Log("Showing rewarded ads");
                    });
                }
                #endregion
                break;
        }
    }

    #endregion

    #region Load Interstitial Ads
    public void LoadInterstitialAd()
    {
        // Clean up the old ad before loading a new one.
        if (interstitialAd != null)
        {
            interstitialAd.Destroy();
            interstitialAd = null;
        }

        Debug.Log("Loading the interstitial ad.");

        // create our request used to load the ad.
        var adRequest = new AdRequest();

        // send the request to load the ad.
        InterstitialAd.Load(interstitialAdID, adRequest, (InterstitialAd ad, LoadAdError error) =>
        {
            // if error is not null, the load request failed.
            if (error != null || ad == null)
            {
                Debug.LogError("interstitial ad failed to load an ad " + "with error : " + error);
                return;
            }

            Debug.Log("Interstitial ad loaded with response : " + ad.GetResponseInfo());

            interstitialAd = ad;
            RegisterEventHandlers(interstitialAd);
        });
    }

    private void RegisterEventHandlers(InterstitialAd interstitialAd)
    {
        // Raised when the ad is estimated to have earned money.
        interstitialAd.OnAdPaid += (AdValue adValue) =>
        {
            Debug.Log(String.Format("Interstitial ad paid {0} {1}.",
                adValue.Value,
                adValue.CurrencyCode));
        };
        // Raised when an impression is recorded for an ad.
        interstitialAd.OnAdImpressionRecorded += () =>
        {
            Debug.Log("Interstitial ad recorded an impression.");
        };
        // Raised when a click is recorded for an ad.
        interstitialAd.OnAdClicked += () =>
        {
            Debug.Log("Interstitial ad was clicked.");
        };
        // Raised when an ad opened full screen content.
        interstitialAd.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("Interstitial ad full screen content opened.");
        };
        // Raised when the ad closed full screen content.
        interstitialAd.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Interstitial ad full screen content closed.");
            LoadInterstitialAd();
        };
        // Raised when the ad failed to open full screen content.
        interstitialAd.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("Interstitial ad failed to open full screen content " + "with error : " + error);
            LoadInterstitialAd();
        };
    }

    public void ShowInterstitialAd()
    {
        ShoawAds(AdType.interstitialAd);
    }
    #endregion

    #region Load Rewarded Ads
    public void LoadRewardedAd()
    {
        // Clean up the old ad before loading a new one.
        if (rewardedAd != null)
        {
            rewardedAd.Destroy();
            rewardedAd = null;
        }

        Debug.Log("Loading the rewarded ad.");

        // create our request used to load the ad.
        var adRequest = new AdRequest();

        // send the request to load the ad.
        RewardedAd.Load(rewardedAdID, adRequest,
            (RewardedAd ad, LoadAdError error) =>
            {
                // if error is not null, the load request failed.
                if (error != null || ad == null)
                {
                    Debug.LogError("Rewarded ad failed to load an ad " +
                                   "with error : " + error);
                    return;
                }

                Debug.Log("Rewarded ad loaded with response : "
                          + ad.GetResponseInfo());

                rewardedAd = ad;
                RegisterEventHandlers(rewardedAd);
            });
    }

    private void RegisterEventHandlers(RewardedAd ad)
    {
        // Raised when the ad is estimated to have earned money.
        ad.OnAdPaid += (AdValue adValue) =>
        {
            Debug.Log(String.Format("Rewarded ad paid {0} {1}.", adValue.Value, adValue.CurrencyCode));
        };
        // Raised when an impression is recorded for an ad.
        ad.OnAdImpressionRecorded += () =>
        {
            Debug.Log("Rewarded ad recorded an impression.");
        };
        // Raised when a click is recorded for an ad.
        ad.OnAdClicked += () =>
        {
            Debug.Log("Rewarded ad was clicked.");
        };
        // Raised when an ad opened full screen content.
        ad.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("Rewarded ad full screen content opened.");
        };
        // Raised when the ad closed full screen content.
        ad.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Rewarded ad full screen content closed.");
            LoadRewardedAd();
        };
        // Raised when the ad failed to open full screen content.
        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("Rewarded ad failed to open full screen content " + "with error : " + error);
            LoadRewardedAd();
        };
    }

    public void ShowRewardedAdUerReward()
        => ShoawAds(AdType.rewardedAd, RewardAdPlacement.UserReward);

    public void ShowRewardedAdDoubleUp()
        => ShoawAds(AdType.rewardedAd, RewardAdPlacement.DoubleUpReward);

    public void CanShowRewardedAd()
    {
        if (rewardedAd == null || rewardedAd.CanShowAd() == false)
        {
            adsData.rewardAvailability = RewardAvailability.UNAVAILABLE;
        }
        else
        {
            adsData.rewardAvailability = RewardAvailability.AVAILABLE;
        }
    }
    #endregion
}