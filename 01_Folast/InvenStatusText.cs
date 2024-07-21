using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InvenStatusText : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI[] textStat;

    private void LateUpdate()
    {
        textStat[0].text = string.Format("{0:n}", "체력 : " + InventorySet.instance.ItemMaxHP);
        textStat[1].text = string.Format("{0:n}", "공격력 : " + InventorySet.instance.ItemDamage);
        textStat[2].text = string.Format("{0:n}", "공격속도 : " + InventorySet.instance.ItemRate);
        textStat[3].text = string.Format("{0:n}", "공격범위 : " + InventorySet.instance.ItemRange);
        textStat[4].text = string.Format("{0:n}", "이동속도 : " + InventorySet.instance.ItemSpeed);
        textStat[5].text = string.Format("{0:n}", "재배치 감소 : " + InventorySet.instance.ItemCoolDown+"초");
        textStat[6].text = string.Format("{0:n}", "유닛 비용 감소 : " + InventorySet.instance.UnitCostDown);
    }
}
