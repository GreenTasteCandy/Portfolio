using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/*
 * 아이템 슬롯 표시 스크립트
 */

public class ItemSlotView : MonoBehaviour
{
    public Image imageSlot;
    public Image icon;
    public int slotNum;

    private void LateUpdate()
    {
        if (GameSetting.ins.itemTable.items[slotNum].name == "")
        {
            icon.color = new Color(1, 1, 1, 0);
        }
        else
        {
            if (GameSetting.ins.itemTable.items[slotNum].rank == ItemRank.Normal) { imageSlot.sprite = Resources.Load<Sprite>("ItemSlot/Normal"); }
            else if (GameSetting.ins.itemTable.items[slotNum].rank == ItemRank.Rare) { imageSlot.sprite = Resources.Load<Sprite>("ItemSlot/Rare"); }
            else if (GameSetting.ins.itemTable.items[slotNum].rank == ItemRank.Epic) { imageSlot.sprite = Resources.Load<Sprite>("ItemSlot/Epic"); }
            else if (GameSetting.ins.itemTable.items[slotNum].rank == ItemRank.Legendary) { imageSlot.sprite = Resources.Load<Sprite>("ItemSlot/Legendary"); }
            else if (GameSetting.ins.itemTable.items[slotNum].rank == ItemRank.Special) { imageSlot.sprite = Resources.Load<Sprite>("ItemSlot/Special"); }

            icon.sprite = GameSetting.ins.itemTable.items[slotNum].sprite;
            icon.color = Color.white;
        }
    }
}
