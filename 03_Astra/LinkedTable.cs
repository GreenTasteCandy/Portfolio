using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * 데이터테이블 연동 스크립트
 * 데이터테이블의 값들을 스크립터블 오브젝트로 옮기는 역할을 수행한다
 */
public class LinkedTable : MonoBehaviour
{

    public void LinkedTableItem(string[] column, int i)
    {
        itemStruct targetItem = GameSetting.ins.itemTable.items[i];

        targetItem.name = column[0];
        targetItem.type = column[1].ToEnum<ItemType>();
        targetItem.rank = column[2].ToEnum<ItemRank>();
        targetItem.desc = column[4];
        targetItem.appearingPlanet = (AppearingPlanet)Enum.Parse(typeof(AppearingPlanet), column[5]);

        targetItem.price = int.Parse(column[3]);
        targetItem.priceAverage = targetItem.price;

        if (targetItem.rank == ItemRank.Normal)
        {
            targetItem.priceMin = targetItem.price - (int)(targetItem.price * GameSetting.ins.normalRange);
            targetItem.priceMax = targetItem.price + (int)(targetItem.price * GameSetting.ins.normalRange);
        }
        else if (targetItem.rank == ItemRank.Rare)
        {
            targetItem.priceMin = targetItem.price - (int)(targetItem.price * GameSetting.ins.rareRange);
            targetItem.priceMax = targetItem.price + (int)(targetItem.price * GameSetting.ins.rareRange);
        }
        else if (targetItem.rank == ItemRank.Epic)
        {
            targetItem.priceMin = targetItem.price - (int)(targetItem.price * GameSetting.ins.epicRange);
            targetItem.priceMax = targetItem.price + (int)(targetItem.price * GameSetting.ins.epicRange);
        }
        else if (targetItem.rank == ItemRank.Legendary)
        {
            targetItem.priceMin = targetItem.price - (int)(targetItem.price * GameSetting.ins.legendRange);
            targetItem.priceMax = targetItem.price + (int)(targetItem.price * GameSetting.ins.legendRange);
        }

    }

    public void LinkedTableEnemy(string[] column, int i)
    {
        EnemyStruct enemy = GameSetting.ins.enemyTable.enemyTable[i];

        enemy.name = column[0];
        enemy.type = column[1].ToEnum<EnemyType>();
        enemy.health = float.Parse(column[2]);
        enemy.damage = float.Parse(column[3]);
        enemy.speed = float.Parse(column[4]);
        enemy.rate = float.Parse(column[5]);
        enemy.range = float.Parse(column[6]);
        enemy.returnRange = float.Parse(column[7]);
    }

    public void LinkedTableWeapon(string[] column, int i)
    {
        weaponStruct weapon = GameSetting.ins.weaponTable.weapon[i];

        weapon.name = column[0];
        weapon.type = column[1].ToEnum<WeaponType>();
        weapon.damage = float.Parse(column[2]);
        weapon.range = float.Parse(column[3]);
        weapon.rate = float.Parse(column[4]);

        weapon.upgradeCost = int.Parse(column[6]);
        weapon.dmgIncrease = float.Parse(column[7]);
        weapon.rateIncrease = float.Parse(column[8]);
        weapon.rangeIncreace = float.Parse(column[9]);

        weapon.skillObj = int.Parse(column[10]);
        weapon.skillType = column[11].ToEnum<SkillType>();
        weapon.skillDmg = float.Parse(column[12]);
        weapon.skillCooldown = float.Parse(column[13]);
        weapon.skillCost = float.Parse(column[14]);
    }

    public void LinkedTableMerchant(string[] column, int i)
    {
        MerchantStruct merchant = GameSetting.ins.merchantTable.merchants[i];

        merchant.name = column[0];
        merchant.type = column[1].ToEnum<MerchantType>();
        merchant.merchantOpenLevel = int.Parse(column[2]);

        merchant.merchantItems = new MerchantItemStruct[8];

        for (int ind = 0; ind < 8; ind++)
        {
            int num = int.Parse(column[3 + ind]);
            merchant.merchantItems[ind] = new MerchantItemStruct(int.Parse(column[3 + ind]));
        }
    }

    public void LinkedTableDialog(string[] column, int i)
    {

        DialogStruct dialog = GameSetting.ins.mainDialog.dialogs[i];
        dialog.dialogIndex = column[1];
        Array.Resize(ref dialog.contents, int.Parse(column[2]));

    }

    public ContentStruct LinkedTableDialogData(string[] column)
    {
        ContentStruct dialog = new ContentStruct();
        dialog.speakerType = (SpeakerType)Enum.Parse(typeof(SpeakerType), column[2]);

        string cName = column[3].Replace("[플레이어]", GameSetting.ins.nickName);
        dialog.speakerName = cName;

        string chatLog = column[4].Replace("[플레이어]", GameSetting.ins.nickName);
        string chatLog2 = chatLog.Replace("[/]", ",");
        dialog.chat = chatLog2;

        dialog.dialogType = (DialogType)Enum.Parse(typeof(DialogType), column[5]);

        dialog.choices = new ChoiceStruct[4];

        for (int ind = 0; ind < 4; ind++)
        {
            int j;
            int.TryParse(column[ind + 10], out j);
            dialog.choices[ind] = new ChoiceStruct(column[ind + 6], j);
        }

        dialog.endDialog = bool.Parse(column[14]);

        return dialog;
    }

    public void LinkedTableRequest(string[] column, int i)
    {
        RequestStruct request = new RequestStruct();
        request.requestTargets = new RequestTargetStruct[5];
        RequestTargetStruct[] target = new RequestTargetStruct[5];

        request.requestIndex = column[0];
        request.bigType = column[1].ToEnum<RequestBigType>();
        request.normalType = column[2].ToEnum<RequestNormalType>();
        request.smallType = column[3].ToEnum<RequestSmallType>();
        request.requestName = column[4];

        request.clientName = column[5];
        request.requestOpenLevel = int.Parse(column[6]);
        request.requestDifficulty = column[7].ToEnum<RequestDifficulty>();
        string desc = column[8].Replace("[/]", ",");
        request.desc = desc;

        if (i == 0)
            request.requestStatus = RequestStatus.Performable;
        else
            request.requestStatus = RequestStatus.Unperformable;

        for (int ind = 0; ind < 5; ind++)
        {
            int num = 10 + (ind * 3);
            int targetInd, targetCount;
            int.TryParse(column[num + 1], out targetInd);
            int.TryParse(column[num + 2], out targetCount);

            target[ind] = new RequestTargetStruct(column[num].ToEnum<RequestTargetType>(), targetInd, targetCount);

            request.requestTargets[ind] = target[ind];
        }

        request.openRequestIndex = column[26];
        request.rewardGold = int.Parse(column[27]);

        GameSetting.ins.requestTable.requests[i] = request;
    }

}
