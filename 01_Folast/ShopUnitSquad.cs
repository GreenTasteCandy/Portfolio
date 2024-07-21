using UnityEngine;
using UnityEngine.UI;

/*
 * 유닛/영웅 부대 편성에서 해당 유닛/영웅 슬롯을 표시하는 기능이 있는 스크립트
 * 해당 유닛/영웅을 터치하여 선택하는 기능도 존재한다
 */

public class ShopUnitSquad : MonoBehaviour
{
    [SerializeField]
    Image unitImage;
    [SerializeField]
    Image iconImage;

    [SerializeField]
    Sprite[] icons;

    ShopSetting squadSet;
    SettingVariable squadData;
    int unitNum;
    Sprite spriteSet;
    bool lockCheck;

    public void SetUp(ShopSetting shopSetting, SettingVariable settingVer, int num, bool unlock)
    {
        squadSet = shopSetting;
        squadData = settingVer;
        unitNum = num;
        lockCheck = unlock;

        if (squadSet.SetSquad == 4)
        {
            spriteSet = squadData.HeroSet[unitNum].weapon[0].sprite;

            if (squadData.HeroSet[unitNum].attackType == AttackType.Melee)
            {
                if (squadData.HeroSet[unitNum].equipType == EquipType.Attacker)
                    iconImage.sprite = icons[0];
                else if (squadData.HeroSet[unitNum].equipType == EquipType.Defender)
                    iconImage.sprite = icons[1];
                else if (squadData.HeroSet[unitNum].equipType == EquipType.Healer)
                    iconImage.sprite = icons[2];
            }

            else if (squadData.HeroSet[unitNum].attackType == AttackType.Range)
            {
                if (squadData.HeroSet[unitNum].equipType == EquipType.Attacker)
                    iconImage.sprite = icons[3];
                else if (squadData.HeroSet[unitNum].equipType == EquipType.Defender)
                    iconImage.sprite = icons[4];
                else if (squadData.HeroSet[unitNum].equipType == EquipType.Healer)
                    iconImage.sprite = icons[5];
            }

            else if (squadData.HeroSet[unitNum].attackType == AttackType.Support)
            {
                if (squadData.HeroSet[unitNum].equipType == EquipType.Attacker)
                    iconImage.sprite = icons[6];
                else if (squadData.HeroSet[unitNum].equipType == EquipType.Defender)
                    iconImage.sprite = icons[7];
                else if (squadData.HeroSet[unitNum].equipType == EquipType.Healer)
                    iconImage.sprite = icons[8];
            }
        }
        else
        {
            TowerTemplate tower;

            if (squadSet.SetSquad == 1)
            {
                spriteSet = squadData.UnitSet[unitNum].weapon[0].sprite;
                tower = squadData.UnitSet[unitNum];
            }
            else if (squadSet.SetSquad == 2)
            {
                spriteSet = squadData.UnitSet2[unitNum].weapon[0].sprite;
                tower = squadData.UnitSet2[unitNum];
            }
            else if (squadSet.SetSquad == 3)
            {
                spriteSet = squadData.UnitSet3[unitNum].weapon[0].sprite;
                tower = squadData.UnitSet3[unitNum];
            }
            else
            {
                spriteSet = squadData.UnitAllSet[unitNum].weapon[0].sprite;
                tower = squadData.UnitAllSet[unitNum];
            }

            if (tower.attackType == AttackType.Melee)
            {
                if (tower.equipType == EquipType.Attacker)
                    iconImage.sprite = icons[0];
                else if (tower.equipType == EquipType.Defender)
                    iconImage.sprite = icons[1];
                else if (tower.equipType == EquipType.Healer)
                    iconImage.sprite = icons[2];
            }

            else if(tower.attackType == AttackType.Range)
            {
                if (tower.equipType == EquipType.Attacker)
                    iconImage.sprite = icons[3];
                else if (tower.equipType == EquipType.Defender)
                    iconImage.sprite = icons[4];
                else if (tower.equipType == EquipType.Healer)
                    iconImage.sprite = icons[5];
            }

            else if (tower.attackType == AttackType.Support)
            {
                if (tower.equipType == EquipType.Attacker)
                    iconImage.sprite = icons[6];
                else if (tower.equipType == EquipType.Defender)
                    iconImage.sprite = icons[7];
                else if (tower.equipType == EquipType.Healer)
                    iconImage.sprite = icons[8];
            }
        }
    }

    private void LateUpdate()
    {
        unitImage.sprite = spriteSet;
    }

    public void SelectShopUnits()
    {
        if (squadSet.SetSquad == 4)
        {
            squadSet.OpenBuyUI(squadData.HeroSet[unitNum], lockCheck);
        }
        else if (squadSet.SetSquad == 1)
        {
            squadSet.OpenBuyUI(squadData.UnitSet[unitNum], lockCheck);
        }
        else if (squadSet.SetSquad == 2)
        {
            squadSet.OpenBuyUI(squadData.UnitSet2[unitNum], lockCheck);
        }
        else if (squadSet.SetSquad == 3)
        {
            squadSet.OpenBuyUI(squadData.UnitSet3[unitNum], lockCheck);
        }
        else
        {
            squadSet.OpenBuyUI(squadData.UnitAllSet[unitNum], lockCheck);
        }
    }
}
