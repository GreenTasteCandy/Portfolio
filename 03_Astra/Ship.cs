using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Ship : MonoBehaviour
{
    [Header("Planet Info")]
    [SerializeField]
    TextMeshProUGUI textShipLevel;
    [SerializeField]
    TextMeshProUGUI textLimitedInventory;
    [SerializeField]
    TextMeshProUGUI textReinforceCost;

    private void LateUpdate()
    {
        int cost = GameSetting.ins.player.user.LoopShipLv + 1;
        textShipLevel.text = "Lv." + cost.ToString();
        textLimitedInventory.text = "탐사 인벤토리 수 : " + (cost + 4).ToString();
        textReinforceCost.text = (cost * 500).ToString();
    }

    public void LoopUpgrade()
    {
        int cost = (GameSetting.ins.player.user.LoopShipLv + 1) * 500;
        if (GameSetting.ins.player.Money >= cost)
        {
            GameSetting.ins.player.user.LoopShipLv += 1;

            GameSetting.ins.player.user.money -= cost;
        }
    }

    public void OnShipInfo()
    {
    }
}
