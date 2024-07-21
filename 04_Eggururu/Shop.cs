using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Shop : MonoBehaviour
{
    public GameObject[] selectBt;
    public GameObject[] shopUI;

    public GameObject[] shopPopup;
    public TextMeshProUGUI popupText;

    public string buyitems;
    public int buyPrice;
    public int buyValue;
    public bool isCash;

    public void ShopSelect(int num)
    {
        foreach(GameObject bt in selectBt)
        {
            bt.SetActive(false);
        }

        foreach (GameObject shop in shopUI)
        {
            shop.SetActive(false);
        }

        selectBt[num].SetActive(true);
        shopUI[num].SetActive(true);
    }

    public void Buy()
    {
        bool isMoney = false;

        if (isCash)
        {
            isMoney = UserData.ins.data.cash >= buyPrice;
        }
        else
        {
            isMoney = UserData.ins.data.gold >= buyPrice;
        }

        if (isMoney)
        {
            string text = "";
            if (isCash)
            {
                UserData.ins.data.cash -= buyPrice;
                FirestoreManager.ins.AddGold("cash", -buyPrice);
            }
            else
            {
                UserData.ins.data.gold -= buyPrice;
                FirestoreManager.ins.AddGold("gold", -buyPrice);
            }

            if (buyitems == "skin")
            {
                UserData.ins.data.skinNum = buyValue;
                UserData.ins.data.skin.Add(DataTable.ins.skinList[buyValue].name, 0);
                text = "캐릭터를 구매하였습니다\n" + DataTable.ins.skinList[buyValue].name;
            }
            else if (buyitems == "ability")
            {
                UserData.ins.data.abilityNow = buyValue;
                UserData.ins.data.skin[UserData.ins.data.skinNow] = buyValue;

                text = "특성을 변경하였습니다\n" + DataTable.ins.abilityTable[buyValue].name;
            }
            else if (buyitems == "birdHair")
            {
                FirestoreManager.ins.BuyItems(buyitems, buyValue);
                UserData.ins.data.skinNum = buyValue;
                UserData.ins.data.birdHair.Add(buyValue);
                text = "장비를 구매하였습니다";
            }
            else if (buyitems == "birdFace")
            {
                FirestoreManager.ins.BuyItems(buyitems, buyValue);
                UserData.ins.data.birdFaceNow = buyValue;
                UserData.ins.data.birdFace.Add(buyValue);
                text = "장비를 구매하였습니다";
            }
            else if (buyitems == "birdShoes")
            {
                FirestoreManager.ins.BuyItems(buyitems, buyValue);
                UserData.ins.data.birdShoesNow = buyValue;
                UserData.ins.data.birdShoes.Add(buyValue);
                text = "장비를 구매하였습니다";
            }

            FirestoreManager.ins.NewFieldMerge();
            StartCoroutine(Popup(text));
        }
    }

    IEnumerator Popup(string text)
    {
        FirestoreManager.ins.LoadData();

        yield return null;

        shopPopup[0].SetActive(false);
        shopPopup[1].SetActive(true);
        popupText.text = text;

        yield return new WaitForSeconds(1.75f);

        shopPopup[1].SetActive(false);
    }
}
