using UnityEngine;
using UnityEngine.UI;

/*
 * ����/���� �δ� ������ �ش� ����/���� ������ ǥ���ϴ� ����� �ִ� ��ũ��Ʈ
 * �ش� ����/������ ��ġ�Ͽ� �����ϴ� ��ɵ� �����Ѵ�
 */

public class UnitSquad : MonoBehaviour
{
    [SerializeField]
    Image unitImage;
    [SerializeField]
    Image iconImage;

    [SerializeField]
    Sprite[] icons;

    SquadSetting squadSet;
    SettingVariable squadData;
    int unitNum;
    Sprite spriteSet;
    bool lockCheck;

    public void SetUp(SquadSetting squadSetting, SettingVariable settingVer, int num, bool unlock)
    {
        squadSet = squadSetting;
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
        if (lockCheck == true)
        {
            unitImage.color = Color.white;
            iconImage.color = Color.white;
            GetComponent<Button>().interactable = true;
        }
        else
        {
            unitImage.color = new Color(1, 1, 1, 0.5f);
            iconImage.color = new Color(1, 1, 1, 0.5f);
            GetComponent<Button>().interactable = false;
        }

        unitImage.sprite = spriteSet;
    }

    //���� 
    public void SeletUnits()
    {
        //���� ������
        if (lockCheck == true)
        {
            if (squadSet.SetSquad == 4)
            {
                squadSet.SeletUnitTemplate(squadData.HeroSet[unitNum]);
            }
            else if (squadSet.SetSquad == 1)
            {
                squadSet.SeletUnitTemplate(squadData.UnitSet[unitNum]);
            }
            else if (squadSet.SetSquad == 2)
            {
                squadSet.SeletUnitTemplate(squadData.UnitSet2[unitNum]);
            }
            else if (squadSet.SetSquad == 3)
            {
                squadSet.SeletUnitTemplate(squadData.UnitSet3[unitNum]);
            }
            else
            {
                squadSet.SeletUnitTemplate(squadData.UnitAllSet[unitNum]);
            }
        }
    }
}
