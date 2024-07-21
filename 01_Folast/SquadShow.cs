using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * 현재 편성된 유닛/영웅을 보여주고 유닛/영웅의 편성을 변경하는 스크립트
 */
public class SquadShow : MonoBehaviour
{

    [SerializeField]
    SquadSetting squadSet;
    [SerializeField]
    GameObject seletImage;
    [SerializeField]
    Image squadImage;
    [SerializeField]
    Image squadIcon;
    [SerializeField]
    int isNum;
    [SerializeField]
    bool isHero;

    bool isSelet;


    private void LateUpdate()
    {
        if (isHero == true)
        {
            if (SettingVariable.heroSlot[isNum] != null)
            {
                squadImage.color = Color.white;
                squadIcon.color = Color.white;

                squadImage.sprite = SettingVariable.heroSlot[isNum].weapon[0].sprite;
                squadIcon.sprite = SettingVariable.heroSlot[isNum].typeIcon;
            }
            else
            {
                squadImage.color = Color.clear;
                squadIcon.color = Color.clear;
            }
        }
        else
        {
            if (SettingVariable.unitSlot[isNum] != null)
            {
                squadImage.color = Color.white;
                squadIcon.color = Color.white;

                squadImage.sprite = SettingVariable.unitSlot[isNum].weapon[0].sprite;
                squadIcon.sprite = SettingVariable.unitSlot[isNum].typeIcon;
            }
            else
            {
                squadImage.color = Color.clear;
                squadIcon.color = Color.clear;
            }
        }
    }

    public void CheckSquad()
    {
        if (squadSet.SeletUnit != null)
        {
            if (isHero == true)
            {
                for (int i = 0; i < SettingVariable.heroSlot.Length; i++)
                {
                    if (SettingVariable.heroSlot[i] == squadSet.SeletUnit)
                        return;
                }

                if (squadSet.SeletUnit.unitTypeData == UnitTypeData.Hero)
                    SettingVariable.heroSlot[isNum] = squadSet.SeletUnit;
            }
            else
            {
                for (int i = 0; i < SettingVariable.unitSlot.Length; i++)
                {
                    if (SettingVariable.unitSlot[i] == squadSet.SeletUnit)
                        return;
                }

                if (squadSet.SeletUnit.unitTypeData == UnitTypeData.Unit)
                    SettingVariable.unitSlot[isNum] = squadSet.SeletUnit;
            }
        }
    }
}
