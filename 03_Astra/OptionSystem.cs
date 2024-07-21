using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SickscoreGames.HUDNavigationSystem;

public class OptionSystem : MonoBehaviour
{
    public Slider bgmSlider;
    public Slider sfxSlider;
    public Slider lookSlider;
    public HUDNavigationSystem hudNav;

    private void Start()
    {
        if (hudNav != null)
        {
            hudNav.EnableMinimap(GameSetting.isMinimap);
            hudNav.EnableCompassBar(GameSetting.isCompass);
        }
    }
    public void BGMSet()
    {
        GameSetting.bgmValue = bgmSlider.value;
    }

    public void SFXSet()
    {
        GameSetting.sfxValue = sfxSlider.value;
    }

    public void LookSet()
    {
        GameSetting.lookValue = lookSlider.value;
    }

    public void MinimapSet()
    {
        if (GameSetting.isMinimap)
            GameSetting.isMinimap = false;
        else
            GameSetting.isMinimap = true;
    }

    public void CompassSet()
    {
        if (GameSetting.isCompass)
            GameSetting.isCompass = false;
        else
            GameSetting.isCompass = true;
    }

    public void GameExit()
    {
        SaveCloud.Ins.SaveWithCloud();

        Application.Quit();
    }
}
