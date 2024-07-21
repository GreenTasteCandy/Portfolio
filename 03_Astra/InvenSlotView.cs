using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/*
 * 필드 인벤토리 표시 스크립트
 */

public class InvenSlotView : MonoBehaviour
{
    [Header("Slot set")]
    public InvenUI invenUI;
    public Inventory inven;
    public Image icon;
    public Image slot;
    public TextMeshProUGUI text;
    public int slotNum;

    [Header("Rank type")]
    public Sprite[] slots;

    private void Start()
    {
        inven = GameManager.ins.inven;

        if (slotNum > GameSetting.ins.player.user.LoopShipLv + 4)
            gameObject.SetActive(false);
    }

    private void LateUpdate()
    {
        slot.sprite = slots[(int)inven.inven.items[slotNum].rank];

        if (inven.inven.items[slotNum].name == "" || inven.inven.items[slotNum].count == 0)
        {
            icon.color = new Color(1, 1, 1, 0);
            text.text = "";
        }
        else
        {
            icon.sprite = inven.inven.items[slotNum].sprite;
            icon.color = Color.white;
            text.text = inven.inven.items[slotNum].count.ToString();
        }
    }

    public void InvenShow()
    {
        if (inven.inven.items[slotNum] == null || inven.inven.items[slotNum].count == 0)
        {
            invenUI.InvenOpen();
            return;
        }

        invenUI.icon.color = Color.white;
        invenUI.icon.sprite = inven.inven.items[slotNum].sprite;
        invenUI.itemName.text = inven.inven.items[slotNum].name;
        invenUI.itemType.text = inven.inven.items[slotNum].type.ToString();
        invenUI.itemPrice.text = "가격 : " + inven.inven.items[slotNum].price.ToString();
    }

}
