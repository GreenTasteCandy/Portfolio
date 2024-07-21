
using UnityEngine;
using UnityEngine.SceneManagement;
using Newtonsoft.Json;
//CloudSave�� GPGS Ŭ���� ���̺� �ý����� ����Ҷ� �ʿ��� ��ɵ��Դϴ�

//���̺� ������ Ŭ����ȭ
public class Data
{
    //�÷��̾ �����ϰ� �ִ� ������
    public int money = 10;

    //�÷��̾� ĳ������ ���� ��Ų
    public int EggMesh = 0;
    public int EggChar = 0;

    //���� �ɼ� ����
    public float BgmSetting = 1.0f;
    public float SeSetting = 1.0f;

    //�÷��̾��� �ְ����� ���
    public int[] HighscoreRecord = new int[5];

    //�÷��̾��� ���� �ְ����� ���
    public int[] LocalHighScore1 = new int[100];
    public string[] LocalHighScoreTime1 = new string[100];

    //�÷��̾ �ر��� ĳ����
    public bool[] SkinNum = new bool[21];
    public bool[] SkinNum2 = new bool[21];
    public bool[] SkinNum3 = new bool[21];
    public bool[] SkinNum4 = new bool[21];

    //�÷��̾ �޼��� ����
    public bool[] AchieveOn = new bool[50];
    public int[] AchieveScore = new int[50];

    //ĳ�־� ��ų/ç���� ��� ���ã��
    public int[] bookmarkSkill = new int[5];
    public int[] bookmarkMode = new int[5];
}

public class CloudSave : MonoBehaviour
{

    //Ŭ���� �ν��Ͻ�ȭ
    static CloudSave instance = new CloudSave();
    public static CloudSave Instance => instance;

    //GPGS Ŭ���� ����
    public void SaveWithCloud(int saveMode)
    {
        Data data = new Data();

        data.money = StartGame.diamonds;
        data.EggChar = StartGame.CharSkin;
        data.EggMesh = StartGame.CharMesh;

        for (int j = 0; j < 21; j++)
        {
            data.SkinNum[j] = StartGame.EggSkinNum[j];
            data.SkinNum2[j] = StartGame.EggSkinNum2[j];
            data.SkinNum3[j] = StartGame.EggSkinNum3[j];
            data.SkinNum4[j] = StartGame.EggSkinNum4[j];
        }

        for (int l = 0; l < 50; l++)
        {
            data.LocalHighScore1[l] = StartGame.HighScoreLocal1[l];
            data.LocalHighScoreTime1[l] = StartGame.HighScoreLocalTime1[l];
        }

        for (int h = 0; h < 50; h++)
        {
            data.AchieveOn[h] = StartGame.AchieveClear[h];
            data.AchieveScore[h] = StartGame.AchievePoint[h];
        }

        data.BgmSetting = StartGame.BgmSet;
        data.SeSetting = StartGame.SeSet;

        for(int t = 0; t < 5; t++)
        {
            data.bookmarkSkill[t] = StartGame.skillBookmark[t];
            data.bookmarkMode[t] = StartGame.modeBookmark[t];
        }

        for (int l = 50; l < 100; l++)
        {
            data.LocalHighScore1[l] = StartGame.HighScoreLocal1[l];
            data.LocalHighScoreTime1[l] = StartGame.HighScoreLocalTime1[l];
        }

        string SaveData = JsonUtility.ToJson(data);

        GPGSBinder.Inst.SaveCloud("SAVEFILE", SaveData, (success) =>
        {
            if (success)
            {
                StartGame.GPGSsave = true;
                if (saveMode == 0)
                    Application.Quit();
            }
        });

    }

    public void LoadWithCloud()
    {
        GPGSBinder.Inst.LoadCloud("SAVEFILE", (success, data) =>
        {
            if (success)
            {
                Data datafile = JsonUtility.FromJson<Data>(data);

                StartGame.diamonds = datafile.money;
                StartGame.CharSkin = datafile.EggChar;
                StartGame.CharMesh = datafile.EggMesh;

                for (int j = 0; j < 21; j++)
                {
                    StartGame.EggSkinNum[j] = datafile.SkinNum[j];
                    StartGame.EggSkinNum2[j] = datafile.SkinNum2[j];
                    StartGame.EggSkinNum3[j] = datafile.SkinNum3[j];
                    StartGame.EggSkinNum4[j] = datafile.SkinNum4[j];
                }

                for (int l = 0; l < 50; l++)
                {
                    StartGame.HighScoreLocal1[l] = datafile.LocalHighScore1[l];
                    StartGame.HighScoreLocalTime1[l] = datafile.LocalHighScoreTime1[l];
                }

                for (int h = 0; h < 21; h++)
                {
                    StartGame.AchieveClear[h] = datafile.AchieveOn[h];
                    StartGame.AchievePoint[h] = datafile.AchieveScore[h];
                }

                StartGame.BgmSet = datafile.BgmSetting;
                StartGame.SeSet = datafile.SeSetting;
            }
        });

        StartGame.GPGSload = true;
    }

}