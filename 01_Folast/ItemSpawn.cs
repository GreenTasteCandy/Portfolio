using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * 아이템 시스템을 담당하는 스크립트
 * 아이템 랜덤 생성,아이템 획득,아이템창 실행
 */
public class ItemSpawn : MonoBehaviour
{
    //인벤토리 지정
    [SerializeField]
    InventorySet inven;
    [SerializeField]
    GameObject inventory;

    //아이템 등급별 테이블 지정
    [SerializeField]
    ItemTemplate itemCommon;
    [SerializeField]
    ItemTemplate itemCursed;
    [SerializeField]
    ItemTemplate itemEpic;
    [SerializeField]
    ItemTemplate itemLegendary;

    [SerializeField]
    GameObject inGameUI;
    [SerializeField]
    GameObject itemSeletUI;
    [SerializeField]
    GameObject menuUI;

    [SerializeField]
    OptionMenu soundEffect;

    public int itemGet;
    public float gameSpeed;

    ItemRank[] itemRank = new ItemRank[3];
    int[] itemNum = new int[3];

    int rerollCost = 10;
    bool isInvenOn = false;

    public ItemRank[] ItemRare => itemRank;
    public int[] ItemNum => itemNum;

    public int RerollCost => rerollCost;
    public ItemTemplate ItemCommon => itemCommon;
    public ItemTemplate ItemCursed => itemCursed;
    public ItemTemplate ItemEpic => itemEpic;
    public ItemTemplate ItemLegendary => itemLegendary;

    public void InventoryOn()
    {
        if (isInvenOn == false)
        {
            inventory.SetActive(true);
            isInvenOn = true;
        }
        else if (isInvenOn == true)
        {
            inventory.SetActive(false);
            isInvenOn = false;
        }
    }

    //랜덤한 아이템 생성
    public void ItemRandomGet()
    {
        //기존 hud 종료 이후 아이템 선택 hud 실행
        inGameUI.SetActive(false);
        itemSeletUI.SetActive(true);

        ItemReRoll(false);
    }

    public void ItemReRoll(bool button) //아이템 재배열
    {
        if (button == true)
        {
            if (GameManager.instance.playerGold.CurrentGold >= rerollCost)
            {
                GameManager.instance.playerGold.PayGold(rerollCost);
                rerollCost += 10;
            }
            else
                return;
        }

        for (int i = 0; i < 3; i++)
        {
            int[] itemList = new int[100];
            for (int j = 0; j < itemList.Length; j++)
                itemList[j] = 1;
            itemGet = Choose(itemList);

            //일반 등급
            if (itemGet >= 0 && itemGet <= 50)
            {
                int[] itemNumList = new int[ItemCommon.item.Length];
                for (int k = 0; k < itemNumList.Length; k++)
                    itemNumList[k] = 1;

                itemNum[i] = Choose(itemNumList);
                itemRank[i] = ItemRank.Common;
            }
            //저주 등급
            else if (itemGet >= 51 && itemGet <= 75)
            {
                int[] itemNumList = new int[ItemCursed.item.Length];
                for (int k = 0; k < itemNumList.Length; k++)
                    itemNumList[k] = 1;

                itemNum[i] = Choose(itemNumList);
                itemRank[i] = ItemRank.Cursed;
            }
            //영웅 등급
            else if (itemGet >= 76 && itemGet <= 90)
            {
                int[] itemNumList = new int[ItemEpic.item.Length];
                for (int k = 0; k < itemNumList.Length; k++)
                    itemNumList[k] = 1;

                itemNum[i] = Choose(itemNumList);
                itemRank[i] = ItemRank.Epic;
            }
            //전설 등급
            else if (itemGet >= 91)
            {
                int[] itemNumList = new int[ItemLegendary.item.Length];
                for (int k = 0; k < itemNumList.Length; k++)
                    itemNumList[k] = 1;

                itemNum[i] = Choose(itemNumList);
                itemRank[i] = ItemRank.Legendary;
            }
        }
    }

    //원하는 아이템 선택
    public void ItemSeletGet(int num)
    {
        bool isInvenNon = false;

        for (int i = 0; i < 30; i++)
        {
            if (inven.itemSlot.item[i].sprite != null && isInvenNon == false)
            {
                if (itemRank[num] == ItemRank.Common && inven.itemSlot.item[i].name == itemCommon.item[itemNum[num]].name)
                {
                    ItemStatusGet(0, i, itemNum[num]);
                    break;
                }
                else if (itemRank[num] == ItemRank.Cursed && inven.itemSlot.item[i].name == itemCursed.item[itemNum[num]].name)
                {
                    ItemStatusGet(1, i, itemNum[num]);
                    break;
                }
                else if (itemRank[num] == ItemRank.Epic && inven.itemSlot.item[i].name == itemEpic.item[itemNum[num]].name)
                {
                    ItemStatusGet(2, i, itemNum[num]);
                    break;
                }
                else if (itemRank[num] == ItemRank.Legendary && inven.itemSlot.item[i].name == itemLegendary.item[itemNum[num]].name)
                {
                    ItemStatusGet(3, i, itemNum[num]);
                    break;
                }
            }
            else if (inven.itemSlot.item[i].sprite == null && isInvenNon == true)
            {
                if (itemRank[num] == ItemRank.Common)
                {
                    inven.itemSlot.item[i] = itemCommon.item[itemNum[num]];
                    ItemStatusGet(0, i, itemNum[num]);
                    break;
                }
                else if (itemRank[num] == ItemRank.Cursed)
                {
                    inven.itemSlot.item[i] = itemCursed.item[itemNum[num]];
                    ItemStatusGet(1, i, itemNum[num]);
                    break;
                }
                else if (itemRank[num] == ItemRank.Epic)
                {
                    inven.itemSlot.item[i] = itemEpic.item[itemNum[num]];
                    ItemStatusGet(2, i, itemNum[num]);
                    break;
                }
                else if (itemRank[num] == ItemRank.Legendary)
                {
                    inven.itemSlot.item[i] = itemLegendary.item[itemNum[num]];
                    ItemStatusGet(3, i, itemNum[num]);
                    break;
                }
            }

            if (i >= 29)
            {
                i = -1;
                isInvenNon = true;
                continue;
            }
        }

        itemSeletUI.SetActive(false);
        inGameUI.SetActive(true);
        GameManager.instance.waveSystem.StartWave();
    }    

    //랜덤 확률 생성
    int Choose(int[] probs)
    {
        float total = 0;

        foreach (float elem in probs)
        {
            total += elem;
        }

        float randomPoint = Random.value * total;

        for (int i = 0; i < probs.Length; i++)
        {
            if (randomPoint < probs[i])
                return i;
            else
                randomPoint -= probs[i];
        }
        return probs.Length - 1;
    }

    //아이템 능력치 증가
    void ItemStatusGet(int rank, int slotNum, int itemNum)
    {
        inven.itemSlot.item[slotNum].level += 1;

        if (rank == 0)
        {
            inven.itemStatus.item[0].hp += itemCommon.item[itemNum].hp;
            inven.itemStatus.item[0].damage += itemCommon.item[itemNum].damage;
            inven.itemStatus.item[0].rate += itemCommon.item[itemNum].rate;
            inven.itemStatus.item[0].range += itemCommon.item[itemNum].range;
            inven.itemStatus.item[0].cooldown += itemCommon.item[itemNum].cooldown;
            inven.itemStatus.item[0].speed += itemCommon.item[itemNum].speed;

            inven.itemStatus.item[0].unitCostDown += itemCommon.item[itemNum].unitCostDown;
            inven.itemStatus.item[0].clearReward += itemCommon.item[itemNum].clearReward;
            inven.itemStatus.item[0].destroyRock += itemCommon.item[itemNum].destroyRock;
            inven.itemStatus.item[0].offenBounsTime += itemCommon.item[itemNum].offenBounsTime;
            inven.itemStatus.item[0].persentReward += itemCommon.item[itemNum].persentReward;

            if (itemNum == 12 && inven.itemSlot.item[slotNum].level >= 8)
            {
                inven.itemStatus.item[0].hp += 1.5f;
                inven.itemStatus.item[0].damage += 1.5f;
                inven.itemStatus.item[0].rate += 1.5f;
                inven.itemStatus.item[0].range += 1.5f;
                inven.itemStatus.item[0].cooldown += 1.5f;
                inven.itemStatus.item[0].speed += 1.5f;

                inven.itemSlot.item[slotNum].level = 1;
            }
            else if (itemNum == 14)
            {
                GameManager.instance.playerGold.CurrentGold += 20;
            }
            else if (itemNum == 18)
            {
                GameManager.instance.playerGold.CurrentGold += 5;
            }

            GameManager.instance.enemySpawner.gamePoint += 350;
        }
        else if (rank == 1)
        {
            inven.itemStatus.item[1].hp += itemCursed.item[itemNum].hp;
            inven.itemStatus.item[1].damage += itemCursed.item[itemNum].damage;
            inven.itemStatus.item[1].rate += itemCursed.item[itemNum].rate;
            inven.itemStatus.item[1].range += itemCursed.item[itemNum].range;
            inven.itemStatus.item[1].cooldown += itemCursed.item[itemNum].cooldown;
            inven.itemStatus.item[1].speed += itemCursed.item[itemNum].speed;

            inven.itemStatus.item[1].unitCostDown += itemCursed.item[itemNum].unitCostDown;
            inven.itemStatus.item[1].clearReward += itemCursed.item[itemNum].clearReward;
            inven.itemStatus.item[1].destroyRock += itemCursed.item[itemNum].destroyRock;
            inven.itemStatus.item[1].offenBounsTime += itemCursed.item[itemNum].offenBounsTime;
            inven.itemStatus.item[1].persentReward += itemCursed.item[itemNum].persentReward;

            if (itemNum == 0)
                GameManager.instance.playerGold.CurrentGold += 100;
            else if (itemNum == 1)
                GameManager.instance.playerHP.TakeDamage(2.0f);
            else if (itemNum == 3)
            {
                GameManager.instance.playerHP.GainHP(1.0f);
            }
            else if (itemNum == 8)
            {
                inven.itemStatus.item[1].hp += Random.Range(-1f, 1f);
                inven.itemStatus.item[1].damage += Random.Range(-1f, 1f);
                inven.itemStatus.item[1].rate += Random.Range(-1f, 1f);
                inven.itemStatus.item[1].range += Random.Range(-1f, 1f);
                inven.itemStatus.item[1].cooldown += Random.Range(-1f, 1f);
                inven.itemStatus.item[1].speed += Random.Range(-1f, 1f);
            }

            GameManager.instance.enemySpawner.gamePoint += 770;

        }
        else if (rank == 2)
        {
            inven.itemStatus.item[2].hp += itemEpic.item[itemNum].hp;
            inven.itemStatus.item[2].damage += itemEpic.item[itemNum].damage;
            inven.itemStatus.item[2].rate += itemEpic.item[itemNum].rate;
            inven.itemStatus.item[2].range += itemEpic.item[itemNum].range;
            inven.itemStatus.item[2].cooldown += itemEpic.item[itemNum].cooldown;
            inven.itemStatus.item[2].speed += itemEpic.item[itemNum].speed;

            inven.itemStatus.item[2].unitCostDown += itemEpic.item[itemNum].unitCostDown;
            inven.itemStatus.item[2].clearReward += itemEpic.item[itemNum].clearReward;
            inven.itemStatus.item[2].destroyRock += itemEpic.item[itemNum].destroyRock;
            inven.itemStatus.item[2].offenBounsTime += itemEpic.item[itemNum].offenBounsTime;
            inven.itemStatus.item[2].persentReward += itemEpic.item[itemNum].persentReward;

            if (itemNum == 10)
            {
                inven.itemStatus.item[0].hp += inven.itemStatus.item[0].hp;
                inven.itemStatus.item[0].damage += inven.itemStatus.item[0].damage;
                inven.itemStatus.item[0].rate += inven.itemStatus.item[0].rate;
                inven.itemStatus.item[0].range += inven.itemStatus.item[0].range;
                inven.itemStatus.item[0].cooldown += inven.itemStatus.item[0].cooldown;
                inven.itemStatus.item[0].speed += inven.itemStatus.item[0].speed;

                inven.itemStatus.item[0].unitCostDown += inven.itemStatus.item[0].unitCostDown;
                inven.itemStatus.item[0].clearReward += inven.itemStatus.item[0].clearReward;
                inven.itemStatus.item[0].destroyRock += inven.itemStatus.item[0].destroyRock;
                inven.itemStatus.item[0].offenBounsTime += inven.itemStatus.item[0].offenBounsTime;
                inven.itemStatus.item[0].persentReward += inven.itemStatus.item[0].persentReward;
            }

            GameManager.instance.enemySpawner.gamePoint += 950;
        }
        else if (rank == 3)
        {
            inven.itemStatus.item[3].hp += itemLegendary.item[itemNum].hp;
            inven.itemStatus.item[3].damage += itemLegendary.item[itemNum].damage;
            inven.itemStatus.item[3].rate += itemLegendary.item[itemNum].rate;
            inven.itemStatus.item[3].range += itemLegendary.item[itemNum].range;
            inven.itemStatus.item[3].cooldown += itemLegendary.item[itemNum].cooldown;
            inven.itemStatus.item[3].speed += itemLegendary.item[itemNum].speed;

            inven.itemStatus.item[3].unitCostDown += itemLegendary.item[itemNum].unitCostDown;
            inven.itemStatus.item[3].clearReward += itemLegendary.item[itemNum].clearReward;
            inven.itemStatus.item[3].destroyRock += itemLegendary.item[itemNum].destroyRock;
            inven.itemStatus.item[3].offenBounsTime += itemLegendary.item[itemNum].offenBounsTime;
            inven.itemStatus.item[3].persentReward += itemLegendary.item[itemNum].persentReward;

            if (itemNum == 7)
            {
                inven.itemStatus.item[0].hp += inven.itemStatus.item[0].hp;
                inven.itemStatus.item[0].damage += inven.itemStatus.item[0].damage;
                inven.itemStatus.item[0].rate += inven.itemStatus.item[0].rate;
                inven.itemStatus.item[0].range += inven.itemStatus.item[0].range;
                inven.itemStatus.item[0].cooldown += inven.itemStatus.item[0].cooldown;
                inven.itemStatus.item[0].speed += inven.itemStatus.item[0].speed;

                inven.itemStatus.item[0].unitCostDown += inven.itemStatus.item[0].unitCostDown;
                inven.itemStatus.item[0].clearReward += inven.itemStatus.item[0].clearReward;
                inven.itemStatus.item[0].destroyRock += inven.itemStatus.item[0].destroyRock;
                inven.itemStatus.item[0].offenBounsTime += inven.itemStatus.item[0].offenBounsTime;
                inven.itemStatus.item[0].persentReward += inven.itemStatus.item[0].persentReward;

                inven.itemStatus.item[2].hp += inven.itemStatus.item[2].hp;
                inven.itemStatus.item[2].damage += inven.itemStatus.item[0].damage;
                inven.itemStatus.item[2].rate += inven.itemStatus.item[0].rate;
                inven.itemStatus.item[2].range += inven.itemStatus.item[0].range;
                inven.itemStatus.item[2].cooldown += inven.itemStatus.item[0].cooldown;
                inven.itemStatus.item[2].speed += inven.itemStatus.item[0].speed;

                inven.itemStatus.item[2].unitCostDown += inven.itemStatus.item[2].unitCostDown;
                inven.itemStatus.item[2].clearReward += inven.itemStatus.item[2].clearReward;
                inven.itemStatus.item[2].destroyRock += inven.itemStatus.item[2].destroyRock;
                inven.itemStatus.item[2].offenBounsTime += inven.itemStatus.item[2].offenBounsTime;
                inven.itemStatus.item[2].persentReward += inven.itemStatus.item[2].persentReward;
            }

            GameManager.instance.enemySpawner.gamePoint += 2000;
        }

        GameManager.instance.playerGold.MaxGold();

    }


    //메뉴화면 켜기
    public void MenuOn()
    {
        soundEffect.SEPlay(0);
        gameSpeed = Time.timeScale;
        Time.timeScale = 0.0f;

        inGameUI.SetActive(false);
        inventory.SetActive(false);
        isInvenOn = false;

        menuUI.SetActive(true);
    }

    //메뉴화면 끄기
    public void MenuOff()
    {
        soundEffect.SEPlay(0);
        Time.timeScale = gameSpeed;
        menuUI.SetActive(false);

        inGameUI.SetActive(true);
    }
}
