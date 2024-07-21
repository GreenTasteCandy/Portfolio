using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class OptionMenu : MonoBehaviour
{
    [SerializeField]
    Slider bgmSetting;
    [SerializeField]
    Slider seSetting;

    [SerializeField]
    AudioSource bgmSpeaker;
    [SerializeField]
    AudioSource[] seSpeaker;

    [SerializeField]
    AudioClip[] soundEffect;
    [SerializeField]
    AudioClip[] hitSound;
    [SerializeField]
    AudioClip[] attackSound;
    [SerializeField]
    AudioClip[] BackGroundMusic;

    private void Start()
    {
        bgmSpeaker.volume = SettingVariable.bgmSet;

        seSpeaker[0].volume = SettingVariable.seSet;
        seSpeaker[1].volume = SettingVariable.seSet;
        seSpeaker[2].volume = SettingVariable.seSet;

        bgmSetting.value = SettingVariable.bgmSet;
        seSetting.value = SettingVariable.seSet;
    }

    public void BGMset()
    {
        bgmSpeaker.volume = bgmSetting.value;
    }

    public void SEset()
    {
        seSpeaker[0].volume = seSetting.value;
        seSpeaker[1].volume = seSetting.value;
        seSpeaker[2].volume = seSetting.value;
    }

    public void SEPlay(int num)
    {
        if (num == 0)
            seSpeaker[num].clip = soundEffect[0];
        if (num == 1)
            seSpeaker[num].clip = hitSound[Random.Range(0,hitSound.Length)];
        if (num == 2)
            seSpeaker[num].clip = attackSound[Random.Range(0, attackSound.Length)];

        seSpeaker[num].Play();
    }

    public void ReturnLobby()
    {
        InventorySet.instance.InvenReset();
        GameManager.instance.enemySpawner.GameEndEnemyStatus();
        Time.timeScale = 1.0f;

        LoadingSceneManager.MoveScene("LobbyScene");
    }

}
