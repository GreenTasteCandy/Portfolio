using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

/*
 * ������ ����ȭ���� ǥ���ϴ� ��ũ��Ʈ
 */

public class ItemSeletViewer : MonoBehaviour
{
    [SerializeField]
    ItemSpawn itemSpawn;

    [SerializeField]
    Image[] itemSprite;
    [SerializeField]
    Image[] itemRanksprite;
    [SerializeField]
    TextMeshProUGUI[] itemText;
    [SerializeField]
    TextMeshProUGUI rerollText;

    private void LateUpdate()
    {
        for (int i = 0; i < 3; i++)
        {
            //�Ϲ� ������ ǥ��
            if (itemSpawn.ItemRare[i] == ItemRank.Common)
            {
                int itemNum = itemSpawn.ItemNum[i];
                itemSprite[i].sprite = itemSpawn.ItemCommon.item[itemNum].sprite;
                itemRanksprite[i].color = Color.white;
                itemText[i].text = string.Format("{0:n}", "<color=white>" + itemSpawn.ItemCommon.item[itemNum].notice + "</color>");
            }
            //���� ������ ǥ��
            else if (itemSpawn.ItemRare[i] == ItemRank.Cursed)
            {
                int itemNum = itemSpawn.ItemNum[i];
                itemSprite[i].sprite = itemSpawn.ItemCursed.item[itemNum].sprite;
                itemRanksprite[i].color = Color.red;
                itemText[i].text = string.Format("{0:n}", "<color=red>" + itemSpawn.ItemCursed.item[itemNum].notice + "</color>");
            }
            //���� ������ ǥ��
            else if (itemSpawn.ItemRare[i] == ItemRank.Epic)
            {
                int itemNum = itemSpawn.ItemNum[i];
                itemSprite[i].sprite = itemSpawn.ItemEpic.item[itemNum].sprite;
                itemRanksprite[i].color = Color.magenta;
                itemText[i].text = string.Format("{0:n}", "<color=purple>" + itemSpawn.ItemEpic.item[itemNum].notice + "</color>");
            }
            //���� ������ ǥ��
            else if (itemSpawn.ItemRare[i] == ItemRank.Legendary)
            {
                int itemNum = itemSpawn.ItemNum[i];
                itemSprite[i].sprite = itemSpawn.ItemLegendary.item[itemNum].sprite;
                itemRanksprite[i].color = Color.yellow;
                itemText[i].text = string.Format("{0:n}", "<color=yellow>" + itemSpawn.ItemLegendary.item[itemNum].notice + "</color>");
            }
        }

        rerollText.text = string.Format("{0:n}", "������ ��迭" + "\n" + "-$" +itemSpawn.RerollCost);
    }
}
