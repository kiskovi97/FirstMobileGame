using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Unity.Services.LevelPlay;

using UnityEngine;

public class AdsInitializer : MonoBehaviour
{
    private string appKey = "236e25a7d";
    private string bannerAdUnitId = "dyod6khuboe5l53g";
    private string interstitialAdUnitId = "8zf7d4bk7z2y80x5";
    private string rewardedAdUnitId = "3ncpjqjr78dz1atq";
    private LevelPlayBannerAd bannerAd;
    private LevelPlayInterstitialAd interstitialAd;
    private LevelPlayRewardedAd rewardedAd;
    private static TaskCompletionSource<bool> rewardTaskSource;

    private static AdsInitializer Instance { get; set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeAds();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    public void InitializeAds()
    {
        LevelPlay.OnInitSuccess += SdkInitializationCompletedEvent;
        LevelPlay.OnInitFailed += SdkInitializationFailedEvent;

        Debug.Log("---------- ValidateIntegration ------------");
        if (Debug.isDebugBuild)
        {
            LevelPlay.SetMetaData("is_test_suite", "enable");
        }
        LevelPlay.ValidateIntegration();
        Debug.Log("---------- ValidateIntegration End ------------");
        LevelPlay.Init(appKey);
    }

    public static Task<bool> LoadRewardAd()
    {
        rewardTaskSource = new TaskCompletionSource<bool>();

        Instance.rewardedAd.LoadAd();

        return rewardTaskSource.Task;
    }

    private static void RewardedAd_OnAdClosed(com.unity3d.mediation.LevelPlayAdInfo obj)
    {
        if (!rewardTaskSource.Task.IsCompleted)
        {
            Debug.Log("Ad closed without reward.");
            rewardTaskSource?.TrySetResult(false);
        }
    }


    private static void RewardedAd_OnAdRewarded(com.unity3d.mediation.LevelPlayAdInfo arg1, com.unity3d.mediation.LevelPlayReward arg2)
    {
        rewardTaskSource?.TrySetResult(true);
    }

    private void SdkInitializationFailedEvent(com.unity3d.mediation.LevelPlayInitError error)
    {
        Debug.LogError("SdkInitializationFailedEvent");
    }

    private void SdkInitializationCompletedEvent(com.unity3d.mediation.LevelPlayConfiguration configuration)
    {
        Debug.Log("SdkInitializationCompletedEvent");

        if (Debug.isDebugBuild)
        {
            LevelPlay.LaunchTestSuite();
        }

        var config = new LevelPlayBannerAd.Config.Builder()
            .SetPosition(com.unity3d.mediation.LevelPlayBannerPosition.TopCenter)
            .SetSize(com.unity3d.mediation.LevelPlayAdSize.BANNER);

        bannerAd = new LevelPlayBannerAd(bannerAdUnitId, config.Build());

        bannerAd.OnAdLoaded += BannerAd_OnAdLoaded;
        bannerAd.OnAdLoadFailed += BannerAd_OnAdLoadFailed;

        bannerAd.LoadAd();

        interstitialAd = new LevelPlayInterstitialAd(interstitialAdUnitId);

        interstitialAd.OnAdLoaded += InterstitialAd_OnAdLoaded;
        interstitialAd.OnAdLoadFailed += InterstitialAd_OnAdLoadFailed;

        rewardedAd = new LevelPlayRewardedAd(rewardedAdUnitId);

        rewardedAd.OnAdLoaded += RewardedAd_OnAdLoaded;
        rewardedAd.OnAdLoadFailed += RewardedAd_OnAdLoadFailed;
        rewardedAd.OnAdRewarded += RewardedAd_OnAdRewarded;
        rewardedAd.OnAdClosed += RewardedAd_OnAdClosed;
    }

    private void RewardedAd_OnAdLoadFailed(com.unity3d.mediation.LevelPlayAdError obj)
    {
        Debug.LogError("RewardedAd_OnAdLoadFailed: " + obj.ErrorMessage);
        rewardTaskSource?.TrySetResult(false);
    }
    private void InterstitialAd_OnAdLoadFailed(com.unity3d.mediation.LevelPlayAdError obj)
    {
        Debug.LogError("InterstitialAd_OnAdLoadFailed: " + obj.ErrorMessage);
    }

    private void BannerAd_OnAdLoadFailed(com.unity3d.mediation.LevelPlayAdError obj)
    {
        Debug.LogError("BannerAd_OnAdLoadFailed: " + obj.ErrorMessage);
        Invoke(nameof(ReloadBanner), 10f);
    }

    private void RewardedAd_OnAdLoaded(com.unity3d.mediation.LevelPlayAdInfo obj)
    {
        rewardedAd.ShowAd();
        Debug.Log("RewardedAd_OnAdLoaded");
    }

    private void InterstitialAd_OnAdLoaded(com.unity3d.mediation.LevelPlayAdInfo obj)
    {
        interstitialAd.ShowAd();
        Debug.Log("InterstitialAd_OnAdLoaded");
    }

    private void BannerAd_OnAdLoaded(com.unity3d.mediation.LevelPlayAdInfo obj)
    {
        Debug.Log("BannerAd_OnAdLoaded");
        bannerAd.ShowAd();
    }

    private void ReloadBanner()
    {
        if (bannerAd != null)
            bannerAd.LoadAd();
    }
}