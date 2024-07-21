using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/*
 * 필드 인벤토리 스크립트
 */

public class Inventory : MonoBehaviour
{
    public ItemData supply;
    public ItemData inven;
    public GameObject invenNotice;

    //아이템 획득
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

    //아이템 획득시 안내 문구 표시
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

    //아이템 획득시 안내 문구 표시
    IEnumerator NoticeInvenFull()
    {
        invenNotice.SetActive(true);
        invenNotice.GetComponent<Image>().color = new Color(0, 0, 0, 0);
        invenNotice.GetComponentInChildren<TextMeshProUGUI>().text = "아이템창 슬롯이 부족합니다.";

        yield return new WaitForSeconds(3f);

        invenNotice.SetActive(false);
    }

    //아이템 창고로 이동
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

        //인벤 초기화
        inven = new ItemData();
        inven.items = new itemStruct[50];
    }
}
