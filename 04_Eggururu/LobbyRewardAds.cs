using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using UnityEngine.Events;
using UnityEditor;

public class LobbyRewardAds : MonoBehaviour
{
    public bool isLobby = true;
    public UnityEvent OnAdPaidEvent;
    public GameManager gm;

    private string _adUnitId = FirebaseAuthManager.ins.isBuild ? "ca-app-pub-3940256099942544/5224354917" : "ca-app-pub-5468550742272911/2673037789";
    private string _adUnitId2 = FirebaseAuthManager.ins.isBuild ? "ca-app-pub-3940256099942544/5224354917" : "ca-app-pub-5468550742272911/2532096625";
    private RewardedAd _rewardedAd = null;

    public void Init()
    {
        MobileAds.Initialize((InitializationStatus initStatus) =>
        {
            if (initStatus != null)
                Debug.Log("Google Admob On");
        });
    }

    private void Start()
    {
        LoadRewardedAd();
    }

    public void LoadRewardedAd()
    {
        // Clean up the old ad before loading a new one.
        if (_rewardedAd != null)
        {
            _rewardedAd.Destroy();
            _rewardedAd = null; 
        }

        Debug.Log("Loading the rewarded ad.");

        // create our request used to load the ad.
        var adRequest = new AdRequest();

        // send the request to load the ad.
        RewardedAd.Load(isLobby ? _adUnitId : _adUnitId2, adRequest, (RewardedAd ad, LoadAdError error) =>
            {
                // if error is not null, the load request failed.
                if (error != null || ad == null)
                {
                    Debug.LogError("Rewarded ad failed to load an ad " + "with error : " + error);
                    return;
                }

                Debug.Log("Rewarded ad loaded with response : " + ad.GetResponseInfo());

                _rewardedAd = ad;

                RegisterEventHandlers(_rewardedAd);
            });
    }

    public void ShowRewardedAd()
    {
        const string rewardMsg = "Rewarded ad rewarded the user. Type: {0}, amount: {1}.";

        if (_rewardedAd != null && _rewardedAd.CanShowAd())
        {
            _rewardedAd.Show((Reward reward) =>
            {
                RewardGain();
                // TODO: Reward the user.
                Debug.Log(String.Format(rewardMsg, reward.Type, reward.Amount));
            });
        }

        LoadRewardedAd();
    }
    public void RewardGain()
    {
        Debug.Log("������ ���� ǥ�õǾ���,������ �־������ϴ�.");

        if (isLobby == true)
        {
            Debug.Log("�κ� ���� ���� �Ϸ�");
            UserData.ins.data.gold += 50;
            UserData.ins.data.cash += 1;
            FirestoreManager.ins.NewFieldMerge();
        }
        else
        {
            gm.isRevive = true;
            Debug.Log("�ΰ��� ���� ���� �Ϸ�");
        }
    }


    public void RewardGainLobby()
    {
        Debug.Log("������ ���� ǥ�õǾ���,������ �־������ϴ�.");

        Debug.Log("�κ� ���� ���� �Ϸ�");
        UserData.ins.data.gold += 50;
        UserData.ins.data.cash += 1;
        FirestoreManager.ins.NewFieldMerge();
    }

    public void RewardGainPlay()
    {
        Debug.Log("������ ���� ǥ�õǾ���,������ �־������ϴ�.");

        Debug.Log("�ΰ��� ���� ���� �Ϸ�");
        gm.Revive();
    }

    private void RegisterEventHandlers(RewardedAd ad)
    {
        // Raised when the ad is estimated to have earned money. ���� ���� �� ������ ������ �� ��ݵ˴ϴ�.
        ad.OnAdPaid += (AdValue adValue) =>
        {
            Debug.Log(String.Format("Rewarded ad paid {0} {1}.", adValue.Value, adValue.CurrencyCode));
        };
        // Raised when an impression is recorded for an ad. ���� ���� �λ��� ��ϵ� �� �λ�˴ϴ�.
        ad.OnAdImpressionRecorded += () =>
        {
            Debug.Log("Rewarded ad recorded an impression.");
        };
        // Raised when a click is recorded for an ad. Ŭ���� ���� ���� ��ϵ� �� �߻��մϴ�.
        ad.OnAdClicked += () =>
        {
            Debug.Log("Rewarded ad was clicked.");
        };
        // Raised when an ad opened full screen content. ���� ��ü ȭ�� �������� ������ �� �߻��մϴ�.
        ad.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("Rewarded ad full screen content opened.");
        };
        // Raised when the ad closed full screen content. ���� ��ü ȭ�� �������� �ݾ��� �� ����մϴ�.
        ad.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Rewarded ad full screen content closed.");
        };
        // Raised when the ad failed to open full screen content. ���� ��ü ȭ�� �������� ���� ������ �� �߻��մϴ�.
        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("Rewarded ad failed to open full screen content " + "with error : " + error);
        };
    }

    private void RegisterReloadHandler(RewardedAd ad)
    {
        // Raised when the ad closed full screen content. ���� ��ü ȭ�� �������� �ݾ��� �� ����մϴ�.
        ad.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Rewarded Ad full screen content closed.");

            // Reload the ad so that we can show another as soon as possible.
            LoadRewardedAd();
        };
        // Raised when the ad failed to open full screen content. ���� ��ü ȭ�� �������� ���� ������ �� �߻��մϴ�.
        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("Rewarded ad failed to open full screen content " + "with error : " + error);

            // Reload the ad so that we can show another as soon as possible.
            LoadRewardedAd();
        };
    }
}
