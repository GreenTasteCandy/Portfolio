using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class Option : MonoBehaviour
{
    public Slider sfx;
    public Slider bgm;

    public Toggle[] frameRate;

    public GameObject[] returnButton;

    public bool isLobby;

    private void OnEnable()
    {
        sfx.value = UserData.ins.data.sfxVolume;
        bgm.value = UserData.ins.data.bgmVolume;
    }

    public void ExitOption()
    {
        Lobby.lobby.isOption = false;
    }

    public void ReturnLobby()
    {
        LoadingSystem.MoveScene("MainLobby");
    }

    public void OnSfxChange()
    {
        UserData.ins.data.sfxVolume = sfx.value;
        SoundManager.ins.sfx.volume = sfx.value;
    }

    public void OnBgmChange()
    {
        UserData.ins.data.bgmVolume = bgm.value;
        SoundManager.ins.bgm.volume = bgm.value;
    }

    public void OnToggleFrame(int num)
    {
        foreach(Toggle t in frameRate)
        {
            if (t != this)
                t.isOn = false;
        }

        if (num == 0)
        {
            Application.targetFrameRate = 30;
            UserData.ins.data.frameRate = 30;
        }
        else if (num == 1)
        {
            Application.targetFrameRate = 60;
            UserData.ins.data.frameRate = 60;
        }
        else if (num == 2)
        {
            Application.targetFrameRate = 120;
            UserData.ins.data.frameRate = 120;
        }
    }

    
}
