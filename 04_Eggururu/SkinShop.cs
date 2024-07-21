using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UI.ThreeDimensional;

public class SkinShop : MonoBehaviour
{
    public Shop shop;
    public TextMeshProUGUI textName;
    public TextMeshProUGUI textPrice;
    public TextMeshProUGUI textAbility;

    public int num;
    public int price;

    public bool isInven;

    public Sprite[] slotImage;
    
    public Sprite[] icons;
    public Image skinImage;
    public Image slot;
    
    public void init(ShopSkins skins, Shop shop)
    {
        this.shop = shop;
        num = skins.ind;
        price = skins.price;
        textPrice.text = skins.price.ToString();
        textName.text = skins.name;

        if (isInven == false)
        {
            if (UserData.ins.data.skin.ContainsKey(DataTable.ins.skinList[num].name) == true)
            {
                gameObject.SetActive(false);
                return;
            }
        }
        else if (isInven == true)
        {
            if (UserData.ins.data.skin.ContainsKey(DataTable.ins.skinList[num].name) == false)
            {
                gameObject.SetActive(false);
                return;
            }

            int ind = 0;
            UserData.ins.data.skin.TryGetValue(DataTable.ins.skinList[num].name, out ind);

            textAbility.text = DataTable.ins.abilityTable[ind].name + "\n" + DataTable.ins.abilityTable[ind].docs;
        }

        int slot = 0;
        if (skins.type == ItemType.rare)
            slot = 1;
        else if (skins.type == ItemType.epic)
            slot = 2;
        else if (skins.type == ItemType.legend)
            slot = 3;

        this.slot.sprite = slotImage[slot];
        skinImage.sprite = icons[num];
    }

    public void SkinBuy()
    {
        SoundManager.ins.ButtonSfx();

        if (isInven)
        {
            foreach (KeyValuePair<string, int> pair in UserData.ins.data.skin)
            {
                if (pair.Key == DataTable.ins.skinList[num].name)
                {
                    UserData.ins.data.skinNow = pair.Key;
                    UserData.ins.data.abilityNow = pair.Value;
                    UserData.ins.data.skinNum = DataTable.ins.skinList[num].ind;
                    break;
                }
            }
        }
        else
        {
            if (!UserData.ins.data.skin.ContainsKey(DataTable.ins.skinList[num].name))
            {
                shop.buyitems = "skin";
                shop.buyValue = num;
                shop.buyPrice = price;
                shop.isCash = false;
                shop.shopPopup[0].SetActive(true);
            }
        }
    }

}

public enum ItemType { normal = 0, rare, epic, legend }
[System.Serializable]
public class ShopSkins
{
    public int ind;
    public int price;
    public string name;
    public ItemType type;

    public ShopSkins(int ind, string name, ItemType type, int price)
    {
        this.ind = ind;
        this.price = price;
        this.name = name;
        this.type = type;
    }
}
