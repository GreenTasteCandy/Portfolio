using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/*
 * 로비화면에서 작동되는 다양한 기능들이 동작하는 스크립트
 * 보유 재화,로비 버튼,옵션창 등의 기능들이 들어 있다
 */
public class Lobby : MonoBehaviour
{
    [Header("Money")]
    [SerializeField]
    TextMeshProUGUI cashText;

    [Header("Lobby")]
    [SerializeField]
    GameObject mainLobby;
    [SerializeField]
    GameObject squadUI;
    [SerializeField]
    GameObject optionUI;
    [SerializeField]
    GameObject shopUI;
    [SerializeField]
    GameObject cashUI;

    [Header("Options")]
    [SerializeField]
    Slider bgmSlider;
    [SerializeField]
    Slider seSlider;
    [SerializeField]
    AudioSource lobbyBGM;

    private void Awake()
    {
        bgmSlider.value = SettingVariable.bgmSet;
        seSlider.value = SettingVariable.seSet;
        lobbyBGM.volume = bgmSlider.value;
    }

    private void LateUpdate()
    {
        cashText.text = string.Format("{0:n0}", SettingVariable.cash);
    }

    public void OptionSlider(int num)
    {
        if (num == 0)
        {
            SettingVariable.bgmSet = bgmSlider.value;
            lobbyBGM.volume = bgmSlider.value;
        }
        else if (num == 1)
        {
            SettingVariable.seSet = seSlider.value;
        }
    }

    public void EndGame()
    {
        CloudSave.Instance.SaveWithCloud(0);
    }

    public void RankBt() => Social.ShowLeaderboardUI();

    public void AchieveBt() => Social.ShowAchievementsUI();

    public void StartGameButton()
    {
        LoadingSceneManager.MoveScene("IngameMap1");
    }    

    public void SquadButton()
    {
        squadUI.SetActive(true);
        mainLobby.SetActive(false);
        cashUI.SetActive(false);
    }

    public void ShopButton()
    {
        shopUI.SetActive(true);
        mainLobby.SetActive(false);
        cashUI.SetActive(true);
    }

    public void OptionButton()
    {
        optionUI.SetActive(true);
        mainLobby.SetActive(false);
        cashUI.SetActive(false);
    }

    public void ReturnLobby()
    {
        mainLobby.SetActive(true);
        shopUI.SetActive(false);
        optionUI.SetActive(false);
        squadUI.SetActive(false);
        cashUI.SetActive(true);
    }
}
