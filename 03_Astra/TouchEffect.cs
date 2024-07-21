using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TouchEffect : MonoBehaviour
{
    public GameObject touchEffect;
    public AudioClip touchAudioClip;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            AudioSource.PlayClipAtPoint(touchAudioClip, Vector3.back, GameSetting.sfxValue);
            Instantiate(touchEffect, Camera.main.ScreenToWorldPoint(Input.mousePosition) + Vector3.forward, Quaternion.identity);
        }
    }

}
