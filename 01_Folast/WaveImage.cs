using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveImage : MonoBehaviour
{
    [SerializeField]
    Sprite defense;
    [SerializeField]
    Sprite offense;
    [SerializeField]
    WaveSystem waveSystem;

    Image waveImage;

    private void Start()
    {
        waveImage = GetComponent<Image>();
    }

    void LateUpdate()
    {
        if (waveSystem.WaveType == WaveType.ReadyWave || waveSystem.WaveType == WaveType.Defense)
            waveImage.sprite = defense;
        else
            waveImage.sprite = offense;
    }
}
