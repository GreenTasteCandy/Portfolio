using UnityEngine.Events;
using UnityEngine;
using GoogleMobileAds.Api;
using GoogleMobileAds.Common;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class RewardBirthbutton : MonoBehaviour
{
    private RewardedAd rewardedAd;
    public UnityEvent OnAdLoadedEvent;
    public UnityEvent OnAdFailedToLoadEvent;
    public UnityEvent OnAdOpeningEvent;
    public UnityEvent OnAdFailedToShowEvent;
    public UnityEvent OnUserEarnedRewardEvent;
    public UnityEvent OnAdClosedEvent;
    public bool showFpsMeter = true;
    public Text fpsMeter;
    public Text statusText;


    public void Start()
    {
        string adUnitId;
#if UNITY_ANDROID
        adUnitId = "ca-app-pub-5468550742272911/1024772362";
#elif UNITY_IPHONE
            adUnitId = "ca-app-pub-3940256099942544/1712485313";
#else
            adUnitId = "unexpected_platform";
#endif

        // create new rewarded ad instance
        rewardedAd = new RewardedAd(adUnitId);

        // Add Event Handlers
        
        //OnAdLoad : 광고 로드가 완료될때 실행
        rewardedAd.OnAdLoaded += (sender, args) =>
        {
            OnAdLoadedEvent.Invoke();
        };

        //OnAdFailedToLoad : 광고 로드에 실패할 때 실행
        rewardedAd.OnAdFailedToLoad += (sender, args) =>
        {
            OnAdFailedToLoadEvent.Invoke();
        };

        //OnAdOpening : 광고가 표시될때 실행되며 기기 화면을 덮습니다
        rewardedAd.OnAdOpening += (sender, args) =>
        {
            OnAdOpeningEvent.Invoke();
        };

        //OnAdFailedToShow : 광고 표시에 실패할 때 실행
        rewardedAd.OnAdFailedToShow += (sender, args) =>
        {
            OnAdFailedToShowEvent.Invoke();
        };

        //OnAdClosed : 사용자가 닫기 아이콘을 탭하거나 뒤로 버튼을 사용하여 보상형 동영상 광고를 닫을 때 실행
        rewardedAd.OnAdClosed += (sender, args) =>
        {
            OnAdClosedEvent.Invoke();
        };

        //OnUserEarnedReward : 사용자가 동영상 시청에 대한 보상을 받아야 할 때 실행
        rewardedAd.OnUserEarnedReward += (sender, args) =>
        {
            OnUserEarnedRewardEvent.Invoke();
        };

        //
        rewardedAd.OnAdDidRecordImpression += (sender, args) =>
        {
            PrintStatus("Reward ad recorded an impression.");
        };

        //
        rewardedAd.OnPaidEvent += (sender, args) =>
        {
            string msg = string.Format("{0} (currency: {1}, value: {2}",
                                        "Rewarded ad received a paid event.",
                                        args.AdValue.CurrencyCode,
                                        args.AdValue.Value);
            PrintStatus(msg);
        };

        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();
        // Load the rewarded ad with the request.
        rewardedAd.LoadAd(request);
    }

    public void ShowRewardedAd()
    {
        if (rewardedAd != null)
        {
            if (StartGame.seletMode != 2)
            {
                rewardedAd.Show();
            }
        }
        else
        {
            PrintStatus("보상형광고가 아직 준비되지 않았습니다,잠시 뒤 다시 시도해 주시기 바랍니다.");
        }
    }

    private void PrintStatus(string message)
    {
        Debug.Log(message);
        MobileAdsEventExecutor.ExecuteInUpdate(() =>
        {
            statusText.text = message;
        });
    }
}
