using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gathering : MonoBehaviour
{
    public ItemData item;
    public ItemData itemSpecial;

    [Header("채집 가능한 아이템 설정")]
    public float randPer = 60;
    public float randPer2 = 94;
    public float randPer3 = 99;
    public bool isActive = true;

    public int normalNum;
    public int rareNum;
    public int epicNum;
    public int legendNum;
    public int specialNum;

    public itemStruct GatherItem()
    {
        float rand = Random.Range(0f, 100.0f);

        itemStruct getItem;

        if (rand >= randPer3)
        {
            isActive = false;
            getItem = new itemStruct(GameManager.ins.itemTable.items[legendNum]);
        }
        else if (rand >= randPer2 - GameManager.ins.luck && rand < randPer3)
        {
            isActive = false;
            getItem = new itemStruct(GameManager.ins.itemTable.items[epicNum]);
        }
        else if (rand >= randPer + GameManager.ins.luck && rand < randPer2 - GameManager.ins.luck)
        {
            isActive = false;
            getItem = new itemStruct(GameManager.ins.itemTable.items[rareNum]);
        }
        else
        {
            isActive = false;
            getItem = new itemStruct(GameManager.ins.itemTable.items[normalNum]);
        }

        return getItem;
    }
}
