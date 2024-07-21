using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InvenUI : MonoBehaviour
{
    public Image icon;
    public TextMeshProUGUI itemName;
    public TextMeshProUGUI itemType;
    public TextMeshProUGUI itemPrice;

    public void InvenOpen()
    {
        icon.color = new Color(0, 0, 0, 0);
        itemName.text = "";
        itemType.text = "";
        itemPrice.text = "";
    }
}
