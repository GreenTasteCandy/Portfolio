using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SelectGameMode : MonoBehaviour
{
    public bool isQuick;
    public TextMeshProUGUI textDocs;

    private void LateUpdate()
    {
        string text = "";
        string text2 = GameMode.ins.gameMode == 0 ? "Ŭ���� ���" : "ĳ�־� ���";

        switch (GameMode.ins.selectMap)
        {
            case 0:
                text = "��";
                break;

            case 1:
                text = "����";
                break;

            case 2:
                text = "�غ�";
                break;
        }

        if (!isQuick)
        {
            textDocs.text = "������ ������ �÷��� �Ͻðڽ��ϱ�?\n������ �� : " + text; 
        }
        else
        {
            textDocs.text = "������ �÷����� �ʰ� ����\n�÷��� �Ͻðڽ��ϱ�?\n������ �� : " + text + "\n������ ��� : " + text2;
        }
    }

    public void StartGame()
    {
        switch (GameMode.ins.selectMap)
        {
            case 0:
                LoadingSystem.MoveScene("Play");
                break;

            case 1:
                LoadingSystem.MoveScene("City");
                break;

            case 2:
                LoadingSystem.MoveScene("Beach");
                break;
        }
    }

}
