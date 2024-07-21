using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/*
 * �ʵ� �κ��丮 ��ũ��Ʈ
 */

public class Inventory : MonoBehaviour
{
    public ItemData supply;
    public ItemData inven;
    public GameObject invenNotice;

    //������ ȹ��
    public void GetItem(itemStruct item)
    {
        bool isItem = false;
        int loopLV = GameSetting.ins.player.user.LoopShipLv + 5;

        for (int i = 0; i < loopLV; i++)
        {
            if (inven.items[i].name == item.name)
            {
                int num = item.count;
                inven.items[i].count += num;
                isItem = true;
                break;
            }
        }

        if (!isItem)
        {
            for (int i = 0; i < loopLV; i++)
            {
                if (inven.items[i].name == "" && inven.items[i].count == 0)
                {
                    int num = item.count;
                    inven.items[i] = item;
                    inven.items[i].count = num;
                    isItem = true;
                    break;
                }
            }
        }

        if (isItem)
            StartCoroutine(NoticeItem(item));
        else
            StartCoroutine(NoticeInvenFull());

    }

    //������ ȹ��� �ȳ� ���� ǥ��
    IEnumerator NoticeItem(itemStruct item)
    {
        int num = item.count;

        invenNotice.SetActive(true);
        invenNotice.GetComponent<Image>().color = Color.white;
        invenNotice.GetComponent<Image>().sprite = item.sprite;
        invenNotice.GetComponentInChildren<TextMeshProUGUI>().text = item.name + "+" + num.ToString();

        yield return new WaitForSeconds(3f);

        invenNotice.SetActive(false);
    }

    //������ ȹ��� �ȳ� ���� ǥ��
    IEnumerator NoticeInvenFull()
    {
        invenNotice.SetActive(true);
        invenNotice.GetComponent<Image>().color = new Color(0, 0, 0, 0);
        invenNotice.GetComponentInChildren<TextMeshProUGUI>().text = "������â ������ �����մϴ�.";

        yield return new WaitForSeconds(3f);

        invenNotice.SetActive(false);
    }

    //������ â��� �̵�
    public void GetSupply()
    {

        for (int j = 0; j < inven.items.Length; j++)
        {
            if (inven.items[j].name != "" && inven.items[j].count > 0)
            {

                for (int i = 0; i < GameManager.ins.itemTable.items.Length; i++)
                {
                    if (GameManager.ins.itemTable.items[i].name != "" && GameManager.ins.itemTable.items[i].name == inven.items[j].name)
                    {
                        GameManager.ins.itemTable.items[i].count += inven.items[j].count;
                    }
                }

            }
        }

        //�κ� �ʱ�ȭ
        inven = new ItemData();
        inven.items = new itemStruct[50];
    }
}
