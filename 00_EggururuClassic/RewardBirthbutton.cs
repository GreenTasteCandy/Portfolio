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
        
        //OnAdLoad : ���� �ε尡 �Ϸ�ɶ� ����
        rewardedAd.OnAdLoaded += (sender, args) =>
        {
            OnAdLoadedEvent.Invoke();
        };

        //OnAdFailedToLoad : ���� �ε忡 ������ �� ����
        rewardedAd.OnAdFailedToLoad += (sender, args) =>
        {
            OnAdFailedToLoadEvent.Invoke();
        };

        //OnAdOpening : ���� ǥ�õɶ� ����Ǹ� ��� ȭ���� �����ϴ�
        rewardedAd.OnAdOpening += (sender, args) =>
        {
            OnAdOpeningEvent.Invoke();
        };

        //OnAdFailedToShow : ���� ǥ�ÿ� ������ �� ����
        rewardedAd.OnAdFailedToShow += (sender, args) =>
        {
            OnAdFailedToShowEvent.Invoke();
        };

        //OnAdClosed : ����ڰ� �ݱ� �������� ���ϰų� �ڷ� ��ư�� ����Ͽ� ������ ������ ���� ���� �� ����
        rewardedAd.OnAdClosed += (sender, args) =>
        {
            OnAdClosedEvent.Invoke();
        };

        //OnUserEarnedReward : ����ڰ� ������ ��û�� ���� ������ �޾ƾ� �� �� ����
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
            PrintStatus("���������� ���� �غ���� �ʾҽ��ϴ�,��� �� �ٽ� �õ��� �ֽñ� �ٶ��ϴ�.");
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
