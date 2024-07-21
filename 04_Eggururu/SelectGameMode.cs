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
        string text2 = GameMode.ins.gameMode == 0 ? "클래식 모드" : "캐주얼 모드";

        switch (GameMode.ins.selectMap)
        {
            case 0:
                text = "숲";
                break;

            case 1:
                text = "도시";
                break;

            case 2:
                text = "해변";
                break;
        }

        if (!isQuick)
        {
            textDocs.text = "선택한 맵으로 플레이 하시겠습니까?\n선택한 맵 : " + text; 
        }
        else
        {
            textDocs.text = "이전에 플레이한 맵과 모드로\n플레이 하시겠습니까?\n선택한 맵 : " + text + "\n선택한 모드 : " + text2;
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
