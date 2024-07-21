using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InvenSlotView : MonoBehaviour
{
    [SerializeField]
    InventorySet inven;

    [SerializeField]
    Image invenSlotImage;
    [SerializeField]
    TextMeshProUGUI invenSlotNum;

    [SerializeField]
    int invenNum;

    private void LateUpdate()
    {
        if (inven.itemSlot.item[invenNum].sprite != null)
        {
            invenSlotImage.GetComponent<Image>().enabled = true;
            invenSlotNum.GetComponent<TextMeshProUGUI>().enabled = true;

            invenSlotImage.sprite = inven.itemSlot.item[invenNum].sprite;
            invenSlotNum.text = inven.itemSlot.item[invenNum].level.ToString();
        }
    }
}
