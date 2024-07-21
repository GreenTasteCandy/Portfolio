using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/*
 * 시스템 텍스트를 띄우게 하는 스크립트
 */
public enum SystemType { Money = 0, Build, Limit }

public class SystemTextViewer : MonoBehaviour
{
    TextMeshProUGUI textSystem;
    TMPAlpha tmpAlpha;

    private void Awake()
    {
        textSystem = GetComponent<TextMeshProUGUI>();
        tmpAlpha = GetComponent<TMPAlpha>();
    }

    public void PrintText(SystemType type)
    {
        switch (type)
        {
            case SystemType.Money:
                textSystem.text = "System : 소지금이 부족합니다.";
                break;
            case SystemType.Build:
                textSystem.text = "System : 이미 타워가 설치된 자리입니다.";
                break;
            case SystemType.Limit:
                textSystem.text = "System : 아직 업그레이드를 할 수 없습니다.";
                break;
        }

        tmpAlpha.FadeOut();
    }
}
