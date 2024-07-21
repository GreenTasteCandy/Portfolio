using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopSetting : MonoBehaviour
{
    [SerializeField]
    GameObject scrollView;
    [SerializeField]
    GameObject unitSelectUI;
    [SerializeField]
    GameObject unitTypeUI;
    [SerializeField]
    GameObject textComplete;
    [SerializeField]
    GameObject buyUI;
    [SerializeField]
    Button buttonBuy;
    [SerializeField]
    GameObject buttonLobby;
    [SerializeField]
    GameObject buttonReturn;
    [SerializeField]
    GameObject unitSlotPrefab;

    [SerializeField]
    Image imageTower;
    [SerializeField]
    Image imageType;
    [SerializeField]
    TextMeshProUGUI textDamage;
    [SerializeField]
    TextMeshProUGUI textRate;
    [SerializeField]
    TextMeshProUGUI textRange;
    [SerializeField]
    TextMeshProUGUI textHp;
    [SerializeField]
    TextMeshProUGUI textName;
    [SerializeField]
    TextMeshProUGUI textUnitNotice;
    [SerializeField]
    TextMeshProUGUI textCost;
    [SerializeField]
    RectTransform textCostPosition;
    [SerializeField]
    GameObject imageCost;

    [SerializeField]
    Sprite[] changeUnitImages;
    [SerializeField]
    Sprite[] changeTypeImages;

    [SerializeField]
    SystemTextViewer systemTextViewer;

    int setSquad;
    bool isUnlock;
    bool completeCheck;
    TowerTemplate selectUnit;
    List<GameObject> unitNotice;

    public int SetSquad => setSquad;
    public TowerTemplate SelectUnit => selectUnit;

    private void Start()
    {
        unitNotice = new List<GameObject>();
    }

    private void LateUpdate()
    {
        if (selectUnit == null)
        {
            if (isUnlock == true)
            {
                textCost.text = "구매 완료";
                imageCost.SetActive(false);
                buttonBuy.interactable = false;
            }
        }
        else
        {
            if (selectUnit.unitTypeData == UnitTypeData.Unit && isUnlock == false)
            {
                textCost.text = "20";
                imageCost.SetActive(true);
                buttonBuy.interactable = true;
            }
            else if (selectUnit.unitTypeData == UnitTypeData.Hero && isUnlock == false)
            {
                textCost.text = "45";
                imageCost.SetActive(true);
                buttonBuy.interactable = true;
            }

            imageTower.sprite = selectUnit.weapon[0].sprite;
            imageType.sprite = selectUnit.typeIcon;
            textName.text = selectUnit.nameTag;
            textDamage.text = string.Format("{0:n1}", "공격력 : " + selectUnit.weapon[0].damage);
            textRate.text = string.Format("{0:n1}", "공격 속도 : " + selectUnit.weapon[0].rate);
            textRange.text = string.Format("{0:n1}", "공격 범위 : " + selectUnit.weapon[0].range);
            textHp.text = string.Format("{0:n1}", "체력 : " + selectUnit.weapon[0].maxHp);
            textUnitNotice.text = string.Format("{0:n1}", selectUnit.notice);
        }
    }

    public void SelectShopSquad(int num)
    {
        completeCheck = true;

        if (unitNotice != null)
        {
            for (int i = 0; i < unitNotice.Count; i++)
            {
                Destroy(unitNotice[i]);
            }
        }
        unitNotice.Clear();

        setSquad = num;

        int squadNum = 0;

        if (setSquad == 0)
        {
            squadNum = SettingVariable.instance.UnitAllSet.Count;
        }
        else if (setSquad == 1)
        {
            squadNum = SettingVariable.instance.UnitSet.Length;
        }
        else if (setSquad == 2)
        {
            squadNum = SettingVariable.instance.UnitSet2.Length;
        }
        else if (setSquad == 3)
        {
            squadNum = SettingVariable.instance.UnitSet3.Length;
        }
        else if (setSquad == 4)
        {
            squadNum = SettingVariable.instance.HeroSet.Length;
        }

        for (int i = 0; i < squadNum; i++)
        {
            bool unlock = true;

            if (setSquad == 0)
            {
                if (i < SettingVariable.instance.UnitSet.Length)
                    unlock = SettingVariable.unitUnLock[i];
                else if (i < SettingVariable.instance.UnitSet.Length + SettingVariable.instance.UnitSet2.Length)
                    unlock = SettingVariable.unitUnLock2[i - SettingVariable.instance.UnitSet.Length];
                else if (i < SettingVariable.instance.UnitSet.Length + SettingVariable.instance.UnitSet2.Length + SettingVariable.instance.UnitSet3.Length)
                    unlock = SettingVariable.unitUnLock3[i - (SettingVariable.instance.UnitSet.Length + SettingVariable.instance.UnitSet2.Length)];
            }
            else if (setSquad == 1)
                unlock = SettingVariable.unitUnLock[i];
            else if (setSquad == 2)
                unlock = SettingVariable.unitUnLock2[i];
            else if (setSquad == 3)
                unlock = SettingVariable.unitUnLock3[i];
            else if (setSquad == 4)
                unlock = SettingVariable.heroUnLock[i];

            if (unlock == false)
            {
                GameObject unitSlot = Instantiate(unitSlotPrefab);

                unitSlot.GetComponent<ShopUnitSquad>().SetUp(this, SettingVariable.instance, i, unlock);
                unitSlot.transform.parent = scrollView.transform;
                unitNotice.Add(unitSlot);
                completeCheck = false;
            }
        }

        if(completeCheck == true)
        {
            textComplete.SetActive(true);
        }
        else
        {
            textComplete.SetActive(false);
        }    
    }

    public void OpenBuyUI(TowerTemplate template, bool lockCheck)
    {
        selectUnit = template;
        isUnlock = lockCheck;

        unitSelectUI.SetActive(false);
        unitTypeUI.SetActive(false);
        buyUI.SetActive(true);
        buttonLobby.SetActive(false);
        buttonReturn.SetActive(true);
    }

    public void ShopBuy()
    {
        if (SettingVariable.cash >= 20 && selectUnit.unitTypeData == UnitTypeData.Unit && isUnlock == false)
        {
            if (selectUnit.nameTag == "포인세티아" && SettingVariable.unitUnLock[3] == false)
            {
                SettingVariable.unitUnLock[3] = true;
                isUnlock = true;
            }
            else if (selectUnit.nameTag == "옥수수" && SettingVariable.unitUnLock[4] == false)
            {
                SettingVariable.unitUnLock[4] = true;
                isUnlock = true;
            }
            else if (selectUnit.nameTag == "소나무" && SettingVariable.unitUnLock[5] == false)
            {
                SettingVariable.unitUnLock[5] = true;
                isUnlock = true;
            }
            else if (selectUnit.nameTag == "용혈수" && SettingVariable.unitUnLock2[1] == false)
            {
                SettingVariable.unitUnLock2[1] = true;
                isUnlock = true;
            }
            else if (selectUnit.nameTag == "튤립" && SettingVariable.unitUnLock3[1] == false)
            {
                SettingVariable.unitUnLock3[1] = true;
                isUnlock = true;
            }

            if (isUnlock == true)
                SettingVariable.cash -= 20;
        }

        else if (SettingVariable.cash >= 45 && selectUnit.unitTypeData == UnitTypeData.Hero && isUnlock == false)
        {
            if (selectUnit.nameTag == "맹그로브" && SettingVariable.heroUnLock[3] == false)
            { 
                SettingVariable.heroUnLock[3] = true;
                isUnlock = true;
            }
            else if (selectUnit.nameTag == "사과" && SettingVariable.heroUnLock[4] == false)
            {
                SettingVariable.heroUnLock[4] = true;
                isUnlock = true;
            }
            else if (selectUnit.nameTag == "두리안" && SettingVariable.heroUnLock[5] == false)
            {
                SettingVariable.heroUnLock[5] = true;
                isUnlock = true;
            }

            if (isUnlock == true)
                SettingVariable.cash -= 45;
        }

        else { systemTextViewer.PrintText(SystemType.Money); }
    }

    public void ShopReturn()
    {
        unitSelectUI.SetActive(true);
        unitTypeUI.SetActive(true);
        buyUI.SetActive(false);
        buttonLobby.SetActive(true);
        buttonReturn.SetActive(false);

        SelectShopSquad(setSquad);
    }
}
