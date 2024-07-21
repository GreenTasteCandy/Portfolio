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
        textStat[0].text = string.Format("{0:n}", "ü�� : " + InventorySet.instance.ItemMaxHP);
        textStat[1].text = string.Format("{0:n}", "���ݷ� : " + InventorySet.instance.ItemDamage);
        textStat[2].text = string.Format("{0:n}", "���ݼӵ� : " + InventorySet.instance.ItemRate);
        textStat[3].text = string.Format("{0:n}", "���ݹ��� : " + InventorySet.instance.ItemRange);
        textStat[4].text = string.Format("{0:n}", "�̵��ӵ� : " + InventorySet.instance.ItemSpeed);
        textStat[5].text = string.Format("{0:n}", "���ġ ���� : " + InventorySet.instance.ItemCoolDown+"��");
        textStat[6].text = string.Format("{0:n}", "���� ��� ���� : " + InventorySet.instance.UnitCostDown);
    }
}
