using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager ins;
    public AudioSource sfx;
    public AudioSource bgm;

    public AudioClip[] sfxList;

    private void Awake()
    {
        ins = this;
    }

    private void Start()
    {
        sfx.volume = UserData.ins.data.sfxVolume;
        bgm.volume = UserData.ins.data.bgmVolume;
    }

    public void ButtonSfx()
    {
        sfx.Play();
    }
}
