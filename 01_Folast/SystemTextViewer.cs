using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/*
 * �ý��� �ؽ�Ʈ�� ���� �ϴ� ��ũ��Ʈ
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
                textSystem.text = "System : �������� �����մϴ�.";
                break;
            case SystemType.Build:
                textSystem.text = "System : �̹� Ÿ���� ��ġ�� �ڸ��Դϴ�.";
                break;
            case SystemType.Limit:
                textSystem.text = "System : ���� ���׷��̵带 �� �� �����ϴ�.";
                break;
        }

        tmpAlpha.FadeOut();
    }
}
