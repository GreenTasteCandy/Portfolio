using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/*
 * �κ��丮 â�� ǥ���ϴ� ��ũ��Ʈ
 */

public class inventoryView : MonoBehaviour
{
    [SerializeField]
    InventorySet inventory;

    [SerializeField]
    Image itemSprite;
    [SerializeField]
    TextMeshProUGUI itemName;
    [SerializeField]
    TextMeshProUGUI itemRank;
    [SerializeField]
    TextMeshProUGUI itemNotice;

    int itemSeletNum;
    
    public void InvenSelet(int num)
    {
        itemSeletNum = num;
    }

    private void LateUpdate()
    {
        if (inventory.itemSlot.item[itemSeletNum].sprite != null)
        {
            itemSprite.sprite = inventory.itemSlot.item[itemSeletNum].sprite;
            itemName.text = inventory.itemSlot.item[itemSeletNum].name;

            if (inventory.itemSlot.item[itemSeletNum].rank == ItemRank.Common)
                itemRank.text = string.Format("{0:n}", inventory.itemSlot.item[itemSeletNum].rank.ToString());
            else if (inventory.itemSlot.item[itemSeletNum].rank == ItemRank.Cursed)
                itemRank.text = string.Format("{0:n}", "<color=red>" + inventory.itemSlot.item[itemSeletNum].rank.ToString() + "</color>");
            else if (inventory.itemSlot.item[itemSeletNum].rank == ItemRank.Epic)
                itemRank.text = string.Format("{0:n}", "<color=purple>" + inventory.itemSlot.item[itemSeletNum].rank.ToString() + "</color>");
            else if (inventory.itemSlot.item[itemSeletNum].rank == ItemRank.Legendary)
                itemRank.text = string.Format("{0:n}", "<color=yellow>" + inventory.itemSlot.item[itemSeletNum].rank.ToString() + "</color>");

            itemNotice.text = inventory.itemSlot.item[itemSeletNum].notice;
        }
        else
        {
            itemSprite.sprite = null;
            itemName.text = "";
            itemRank.text = "";
            itemNotice.text = "";
        }
    }
}
