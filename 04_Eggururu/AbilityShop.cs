using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AbilityShop : MonoBehaviour
{
    public Shop shop;
    public TextMeshProUGUI textName;
    public TextMeshProUGUI textDocs;
    public bool isSpecial;

    private void LateUpdate()
    {
        textName.text = DataTable.ins.abilityTable[UserData.ins.data.abilityNow].name;
        textDocs.text = DataTable.ins.abilityTable[UserData.ins.data.abilityNow].docs;
    }

    public void AbilityBuy(bool isCash)
    {
        AbilityType type = isSpecial ? AbilityType.Special : AbilityType.Normal;
        List<int> list = new List<int>();

        foreach(Ability ab in DataTable.ins.abilityTable)
        {
            if (ab.num == 0)
                continue;

            if (ab.num != UserData.ins.data.abilityNow)
            {
                if (ab.type == type)
                {
                    list.Add(ab.num);
                }
            }
        }

        shop.buyitems = "ability";
        shop.buyValue = list[Random.Range(0, list.Count)];

        if (isSpecial)
        {
            if (isCash)
            {
                shop.buyPrice = 1;
                shop.isCash = true;
            }
            else
            {
                shop.buyPrice = 100;
                shop.isCash = false;
            }
        }
        else
        {
            shop.buyPrice = 25;
            shop.isCash = false;
        }

        shop.shopPopup[0].SetActive(true);
    }
}
