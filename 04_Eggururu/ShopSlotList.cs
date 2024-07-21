using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopSlotList : MonoBehaviour
{
    public Shop shop;
    public GameObject skinshopList;
    public GameObject skinSlot;
    public List<GameObject> shopList;

    public bool isInven = true;

    private void OnEnable()
    {
        if (isInven)
            InvenOpen();
        else
            SkinShopOpen();
    }

    private void OnDisable()
    {
        SkinShopClose();
    }
    public void InvenOpen()
    {
        Debug.Log("ÀÎº¥");
        int i = 0;
        for (int num = 0; num < DataTable.ins.skinList.Count; num++)
        {
            ShopSkins data = DataTable.ins.skinList[num];
            if ((shopList.Count - 1) < (i + 1))
            {
                GameObject obj = Instantiate(skinSlot, skinshopList.transform.position, Quaternion.identity);
                obj.transform.parent = skinshopList.transform;
                obj.GetComponent<SkinShop>().init(data, shop);
                shopList.Add(obj);
            }
            else
            {
                shopList[i].SetActive(true);
                shopList[i].GetComponent<SkinShop>().init(data, shop);
            }

            i += 1;
        }

    }

    public void SkinShopOpen()
    {

        Debug.Log("»óÁ¡ ¿ÀÇÂ");
        int i = 1;

        foreach (ShopSkins data in DataTable.ins.skinList)
        {
            if (data.name == "´Þ°¿")
                continue;

            Debug.Log(data.name);
            if ((shopList.Count - 1) < i)
            {
                GameObject obj = Instantiate(skinSlot, skinshopList.transform.position, Quaternion.identity);
                obj.transform.parent = skinshopList.transform;
                obj.GetComponent<SkinShop>().init(data, shop);
                shopList.Add(obj);
            }
            else
            {
                shopList[i].SetActive(true);
                shopList[i].GetComponent<SkinShop>().init(data, shop);
            }

            i += 1;
        }

    }

    public void SkinShopClose()
    {
        foreach (GameObject obj in shopList)
        {
            obj.SetActive(false);
        }
    }

}
