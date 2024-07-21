using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TargetSlotView : MonoBehaviour
{
    public Request request;
    public Image imageTarget;
    public TextMeshProUGUI textTargetName;
    public TextMeshProUGUI textTargetCount;
    public Image imagePlanet;
    public int slotNum;
    public int targetNum;
    int storageNum;

    private void LateUpdate()
    {
        if (GameSetting.ins.requestTable.requests[slotNum].requestName == "")
        {
            textTargetName.text = "";
            textTargetCount.text = "";
        }
        else
        {
            if (GameSetting.ins.requestTable.requests[slotNum].requestTargets[targetNum].targetType == RequestTargetType.Collect)
            {
                int itemNum = GameSetting.ins.requestTable.requests[slotNum].requestTargets[targetNum].targetItemIndex;
                string targetItemName = GameSetting.ins.itemTable.items[itemNum].name;

                for (int i = 0; i < GameSetting.ins.supply.items.Length; i++)
                {
                    if (GameSetting.ins.supply.items[i].name == targetItemName)
                    {
                        storageNum = i;
                        break;
                    }
                    else
                    {
                        storageNum = -1;
                    }
                }

                imageTarget.color = new Color(1, 1, 1, 1);
                imageTarget.sprite = GameSetting.ins.itemTable.items[itemNum].sprite;
                textTargetName.text = targetItemName.ToString() + " 구해오기";
                if (storageNum > -1)
                {
                    textTargetCount.text = GameSetting.ins.supply.items[storageNum].count.ToString() + " / " + GameSetting.ins.requestTable.requests[slotNum].requestTargets[targetNum].targetItemCountGoal.ToString();
                }
                else
                {
                    textTargetCount.text = "0 / " + GameSetting.ins.requestTable.requests[slotNum].requestTargets[targetNum].targetItemCountGoal.ToString();
                }

                //아이템 출현 행성 출력
                imagePlanet.color = new Color(1, 1, 1, 1);
                if (GameSetting.ins.itemTable.items[itemNum].appearingPlanet == AppearingPlanet.None)
                {
                    imagePlanet.sprite = Resources.Load<Sprite>("Planets/None");
                }
                else if (GameSetting.ins.itemTable.items[itemNum].appearingPlanet == AppearingPlanet.Meter)
                {
                    imagePlanet.sprite = Resources.Load<Sprite>("Planets/Metre");
                }
                else if (GameSetting.ins.itemTable.items[itemNum].appearingPlanet == AppearingPlanet.Def)
                {
                    imagePlanet.sprite = Resources.Load<Sprite>("Planets/Def");
                }
                else if (GameSetting.ins.itemTable.items[itemNum].appearingPlanet == AppearingPlanet.Cocytus)
                {
                    imagePlanet.sprite = Resources.Load<Sprite>("Planets/Cocytus");
                }
            }

            if (GameSetting.ins.requestTable.requests[slotNum].requestTargets[targetNum].targetType == RequestTargetType.Hunt)
            {
                int enemyNum = GameSetting.ins.requestTable.requests[slotNum].requestTargets[targetNum].targetEnemyIndex;
                string targetEnemyName = GameSetting.ins.enemyTable.enemyTable[enemyNum].name;

                //적 이미지 출력
                if (enemyNum == 0) { imageTarget.sprite = Resources.Load<Sprite>("Enemy/Mebit"); }
                if (enemyNum == 1) { imageTarget.sprite = Resources.Load<Sprite>("Enemy/Brown Bear"); }
                if (enemyNum == 2) { imageTarget.sprite = Resources.Load<Sprite>("Enemy/Uruslano"); }
                if (enemyNum == 3) { imageTarget.sprite = Resources.Load<Sprite>("Enemy/Ancient Golem"); }
                if (enemyNum == 4) { imageTarget.sprite = Resources.Load<Sprite>("Enemy/Runaway Golem"); }
                if (enemyNum == 5) { imageTarget.sprite = Resources.Load<Sprite>("Enemy/Beginning Golem"); }
                if (enemyNum == 6) { imageTarget.sprite = Resources.Load<Sprite>("Enemy/Cocytus Penguin"); }
                if (enemyNum == 7) { imageTarget.sprite = Resources.Load<Sprite>("Enemy/X-24"); }
                if (enemyNum == 8) { imageTarget.sprite = Resources.Load<Sprite>("Enemy/Albus Ursus"); }

                textTargetName.text = targetEnemyName.ToString() + " 처치하기";
                textTargetCount.text = GameSetting.ins.requestTable.requests[slotNum].requestTargets[targetNum].targetEnemyCount.ToString() + " / " + GameSetting.ins.requestTable.requests[slotNum].requestTargets[targetNum].targetEnemyCountGoal.ToString();

                //적 출현 행성 출력
                imagePlanet.color = new Color(1, 1, 1, 1);
                if (enemyNum >= 0 && enemyNum < 3)
                {
                    imagePlanet.sprite = Resources.Load<Sprite>("Planets/Metre");
                }
                else if (enemyNum >= 3 && enemyNum < 6)
                {
                    imagePlanet.sprite = Resources.Load<Sprite>("Planets/Def");
                }
                else if (enemyNum >= 6 && enemyNum < 9)
                {
                    imagePlanet.sprite = Resources.Load<Sprite>("Planets/Cocytus");
                }
                else
                {
                    imagePlanet.sprite = Resources.Load<Sprite>("Planets/None");
                }
            }
        }
    }
}
