using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/*
 * 아이템 슬롯 표시 스크립트
 */

public class TradeSlotView : MonoBehaviour
{
    public GameObject itemSlot;
    public GameObject tradeButton;
    public TextMeshProUGUI itemName;
    public TextMeshProUGUI itemPrice;
    public Trade trade;
    public int merchantNum;
    public int slotNum;
    public int itemNum;

    private void LateUpdate()
    {
        if (GameSetting.ins.itemTable.items[itemNum].name == "")
        {
            itemName.text = "";
            itemPrice.text = "";
            itemSlot.SetActive(false);
        }
        else
        {
            itemSlot.SetActive(true);
            itemName.text = GameSetting.ins.itemTable.items[itemNum].name.ToString();
            itemPrice.text = GameSetting.ins.merchantTable.merchants[merchantNum].merchantItems[slotNum].tradeCost.ToString();
            itemSlot.GetComponent<ItemSlotView>().slotNum = itemNum;

            if (trade.isSell)
            {
                if (GameSetting.ins.itemTable.items[itemNum].count >= 1)
                {
                    tradeButton.GetComponent<Image>().color = new Color(1, 1, 1, 1);
                    tradeButton.GetComponent<Button>().interactable = true;
                }
                else
                {
                    tradeButton.GetComponent<Image>().color = new Color(0.4f, 0.4f, 0.4f, 1);
                    tradeButton.GetComponent<Button>().interactable = false;
                }
            }
            else
            {
                if (GameSetting.ins.player.user.money >= GameSetting.ins.merchantTable.merchants[merchantNum].merchantItems[slotNum].tradeCost)
                {
                    tradeButton.GetComponent<Image>().color = new Color(1, 1, 1, 1);
                    tradeButton.GetComponent<Button>().interactable = true;
                }
                else
                {
                    tradeButton.GetComponent<Image>().color = new Color(0.4f, 0.4f, 0.4f, 1);
                    tradeButton.GetComponent<Button>().interactable = false;
                }
            }
        }
    }
}
