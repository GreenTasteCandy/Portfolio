using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Equipment : MonoBehaviour
{
    public TextMeshProUGUI upgradeCost;

    public Image weaponUi;
    public TextMeshProUGUI upgradeBefore;
    public TextMeshProUGUI upgradeAfter;

    public Sprite[] weaponIcon;

    [TextArea]
    public string[] upgradeDesc;

    public int equipNum;
    public int equipCost;

    public void SelectEquip(int num)
    {
        equipNum = num;
        if (equipNum < 99)
        {
            equipCost = GameSetting.ins.weaponTable.weapon[equipNum].upgradeCost * (GameSetting.ins.player.user.weaponLv[equipNum] + 1);
        }
        else if (equipNum == 100)
        {
            equipCost = (500 * (GameSetting.ins.player.user.helmetLv + 1));
        }
        else if (equipNum == 101)
        {
            equipCost = (500 * (GameSetting.ins.player.user.armorLv + 1));
        }
        else if (equipNum == 102)
        {
            equipCost = (500 * (GameSetting.ins.player.user.bootLv + 1));
        }
        else if (equipNum == 103)
        {
            equipCost = (500 * (GameSetting.ins.player.user.gloveLv + 1));
        }
    }

    public void UpgradeEquip()
    {
        if (GameSetting.ins.player.Money >= equipCost)
        {
            if (equipNum < 99)
            {
                GameSetting.ins.player.user.weaponLv[equipNum] += 1;
            }
            else if (equipNum == 100)
            {
                GameSetting.ins.player.user.helmetLv += 1;
            }
            else if (equipNum == 101)
            {
                GameSetting.ins.player.user.armorLv += 1;
            }
            else if (equipNum == 102)
            {
                GameSetting.ins.player.user.bootLv += 1;
            }
            else if (equipNum == 103)
            {
                GameSetting.ins.player.user.gloveLv += 1;
            }

            GameSetting.ins.player.user.money -= equipCost;
        }

    }

    private void LateUpdate()
    {
        if (equipNum < 99)
        {
            int cost = (GameSetting.ins.weaponTable.weapon[equipNum].upgradeCost * (GameSetting.ins.player.user.weaponLv[equipNum] + 1));
            upgradeCost.text = cost.ToString();

            float increaseDMG = GameSetting.ins.weaponTable.weapon[equipNum].dmgIncrease * (GameSetting.ins.player.user.weaponLv[equipNum] + 1);
            float increaseRange = GameSetting.ins.weaponTable.weapon[equipNum].rangeIncreace * (GameSetting.ins.player.user.weaponLv[equipNum] + 1);
            float increaseRate = GameSetting.ins.weaponTable.weapon[equipNum].rateIncrease * (GameSetting.ins.player.user.weaponLv[equipNum] + 1);

            upgradeBefore.text = string.Format(upgradeDesc[0], increaseDMG, increaseRange, increaseRate);
            upgradeAfter.text = string.Format(upgradeDesc[0], increaseDMG + GameSetting.ins.weaponTable.weapon[equipNum].dmgIncrease, increaseRange + GameSetting.ins.weaponTable.weapon[equipNum].rangeIncreace, increaseRate + GameSetting.ins.weaponTable.weapon[equipNum].rateIncrease);

            weaponUi.sprite = weaponIcon[equipNum];
        }
        else if (equipNum == 100)
        {
            int cost = (500 * (GameSetting.ins.player.user.helmetLv + 1));
            upgradeCost.text = cost.ToString();

            float increase1 = 3 * (GameSetting.ins.player.user.helmetLv + 1);
            float increase2 = 0.3f * (GameSetting.ins.player.user.helmetLv + 1);
            upgradeBefore.text = string.Format(upgradeDesc[1], increase1, increase2);
            upgradeAfter.text = string.Format(upgradeDesc[1], increase1 + 3, increase2 + 0.3f);

            weaponUi.sprite = weaponIcon[weaponIcon.Length - 5];
        }
        else if (equipNum == 101)
        {
            int cost = (500 * (GameSetting.ins.player.user.armorLv + 1));
            upgradeCost.text = cost.ToString();

            float increase1 = 10 * (GameSetting.ins.player.user.armorLv + 1);
            float increase2 = 0.2f * (GameSetting.ins.player.user.armorLv + 1);
            upgradeBefore.text = string.Format(upgradeDesc[2], increase1, increase2);
            upgradeAfter.text = string.Format(upgradeDesc[2], increase1 + 10, increase2 + 0.2f);

            weaponUi.sprite = weaponIcon[weaponIcon.Length - 4];
        }
        else if (equipNum == 102)
        {
            int cost = (500 * (GameSetting.ins.player.user.bootLv + 1));
            upgradeCost.text = cost.ToString();

            float increase2 = 0.3f * (GameSetting.ins.player.user.bootLv + 1);
            upgradeBefore.text = string.Format(upgradeDesc[3], increase2, increase2);
            upgradeAfter.text = string.Format(upgradeDesc[3], increase2 + 0.3f, increase2 + 0.3f);

            weaponUi.sprite = weaponIcon[weaponIcon.Length - 3];
        }
        else if (equipNum == 103)
        {
            int cost = (500 * (GameSetting.ins.player.user.gloveLv + 1));
            upgradeCost.text = cost.ToString();

            float increase1 = 0.1f * (GameSetting.ins.player.user.gloveLv + 1);
            float increase2 = 0.2f * (GameSetting.ins.player.user.gloveLv + 1);
            upgradeBefore.text = string.Format(upgradeDesc[4], increase1, increase2);
            upgradeAfter.text = string.Format(upgradeDesc[4], increase1 + 0.1f, increase2 + 0.2f);

            weaponUi.sprite = weaponIcon[weaponIcon.Length - 2];
        }

    }
}
