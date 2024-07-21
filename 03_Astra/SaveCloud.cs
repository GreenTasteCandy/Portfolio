using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

/*
 * ���� Ŭ���� ���̺� ��ũ��Ʈ
 * 0 : ��ȭ �� ���� ������
 * 1 : ������
 * 2 : �Ƿ�
 */

public class SaveCloud : MonoBehaviour
{
    static public SaveCloud Ins;

    private void Awake()
    {
        Ins = this;
    }

    public void SaveWithCloud()
    {
#if UNITY_ANDROID
        string saveData;

        UserStatus status = new UserStatus();
        Array.Resize(ref status.weaponLv, 4);
        Array.Resize(ref status.toolLv, 1);

        status.isFirst = GameSetting.ins.player.user.isFirst;

        status.money = GameSetting.ins.player.user.money;
        status.armorLv = GameSetting.ins.player.user.armorLv;
        status.gloveLv = GameSetting.ins.player.user.gloveLv;
        status.bootLv = GameSetting.ins.player.user.bootLv;

        status.weaponLv[0] = GameSetting.ins.player.user.weaponLv[0];
        status.weaponLv[1] = GameSetting.ins.player.user.weaponLv[1];
        status.weaponLv[2] = GameSetting.ins.player.user.weaponLv[2];
        status.weaponLv[3] = GameSetting.ins.player.user.weaponLv[3];

        status.toolLv[0] = GameSetting.ins.player.user.toolLv[0];

        saveData = JsonUtility.ToJson(status);

        GPGSBinder.Inst.SaveCloud("SaveUser", saveData);

        PlayerInven itemNum = new PlayerInven();

        for (int i = 0; i < GameSetting.ins.player.inven.itemNum.Length; i++)
        {
            itemNum.itemNum[i] = GameSetting.ins.player.inven.itemNum[i];
        }

        saveData = JsonUtility.ToJson(itemNum);

        GPGSBinder.Inst.SaveCloud("SaveItem", saveData);

        PlayerRequest requset = new PlayerRequest();

        for (int i = 0; i < GameSetting.ins.player.request.requestStatus.Length; i++)
        {
            requset.requestStatus[i] = GameSetting.ins.player.request.requestStatus[i];
        }

        saveData = JsonUtility.ToJson(requset);

        GPGSBinder.Inst.SaveCloud("SaveRequest", saveData);
#endif
    }

}
