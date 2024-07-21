using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudSave : MonoBehaviour
{
    //클래스 인스턴스화
    static CloudSave instance = new CloudSave();
    public static CloudSave Instance => instance;

    //GPGS 클라우드 저장
    public void SaveWithCloud(int saveMode)
    {
        SaveData data = new SaveData();

        data.cash = SettingVariable.cash;
        data.bgmSet = SettingVariable.bgmSet;
        data.seSet = SettingVariable.seSet;
        data.language = SettingVariable.language;

        for(int i = 0; i < 30; i++)
        {
            data.achieve[i] = SettingVariable.achieve[i];
            data.leaderboard[i] = SettingVariable.leaderboard[i];
        }

        for(int h = 0; h < 5; h++)
        {
            data.unitSlot[h] = SettingVariable.unitSlot[h];
        }

        for (int f = 0; f < 3; f++)
        {
            data.heroSlot[f] = SettingVariable.heroSlot[f];
        }

        for (int j = 0; j < 64; j++)
        {
            data.unitUnLock[j] = SettingVariable.unitUnLock[j];
            data.unitUnLock2[j] = SettingVariable.unitUnLock2[j];
            data.unitUnLock3[j] = SettingVariable.unitUnLock3[j];
            data.heroUnLock[j] = SettingVariable.heroUnLock[j];
        }

        string SaveData = JsonUtility.ToJson(data);

        GPGSBinder.Inst.SaveCloud("SAVEFILE", SaveData, (success) =>
        {
            if (success)
            {
                //SettingVariable.instance.gameStart = true;
                if (saveMode == 0)
                    Application.Quit();
            }
        });

    }

}
