using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using GooglePlayGames;
using GooglePlayGames.BasicApi;

/*
 * ������ �⺻���� ���� ���� �� ������ ���� �ý��� ���۴ܰ迡 �ִ� ��ũ��Ʈ
 * ��ȭ,����/���� �� �� ����/���� ���� ������ ����ǰ�
 * ���� �� ó�� ���۽� GPGS �α��ΰ� Ŭ���� ������ �ε尡 ����ȴ�
 */
public class SaveData
{
    public int cash = 10;

    public float bgmSet = 1f;
    public float seSet = 1f;
    public int language = 0;

    public int[] achieve = new int[30];
    public int[] leaderboard = new int[30];

    public TowerTemplate[] unitSlot = new TowerTemplate[5];
    public TowerTemplate[] heroSlot = new TowerTemplate[3];

    public bool[] unitUnLock = new bool[64];
    public bool[] unitUnLock2 = new bool[64];
    public bool[] unitUnLock3 = new bool[64];
    public bool[] heroUnLock = new bool[64];
}

public class SettingVariable : MonoBehaviour
{
    public static SettingVariable instance;

    public static int cash = 10; //�׽�Ʈ�� ���� ����, �ʱⰪ : 10

    public static float bgmSet = 1f;
    public static float seSet = 1f;
    public static int language = 0;

    public static int[] achieve = new int[30];
    public static int[] leaderboard = new int[30];

    public static TowerTemplate[] unitSlot = new TowerTemplate[5];
    public static TowerTemplate[] heroSlot = new TowerTemplate[3];

    public static bool[] unitUnLock = new bool[64];
    public static bool[] unitUnLock2 = new bool[64];
    public static bool[] unitUnLock3 = new bool[64];
    public static bool[] heroUnLock = new bool[64];

    [SerializeField]
    TowerTemplate[] unitAttackSet;
    [SerializeField]
    TowerTemplate[] unitDefendSet;
    [SerializeField]
    TowerTemplate[] unitSupportSet;
    [SerializeField]
    TowerTemplate[] heroSet;

    List<TowerTemplate> unitAllSet = new List<TowerTemplate>();

    public bool gpgsLogin = false;
    public bool gameStart = false;

    public TowerTemplate[] UnitSet => unitAttackSet;
    public TowerTemplate[] UnitSet2 => unitDefendSet;
    public TowerTemplate[] UnitSet3 => unitSupportSet;
    public TowerTemplate[] HeroSet => heroSet;
    public List<TowerTemplate> UnitAllSet => unitAllSet;

    //���� ���� ��
    private void Awake()
    {
        instance = this;

        Application.targetFrameRate = 60; //������ 60 ����

        if (gpgsLogin == false)
        {
            PlayGamesPlatform.Activate(); //���� �÷��� �α���
            PlayGamesPlatform.Instance.Authenticate((status) =>
            {
                if (status == SignInStatus.Success)
                {
                    gpgsLogin = true;

                    //Ŭ���� ������ �ҷ�����
                    GPGSBinder.Inst.LoadCloud("SAVEFILE", (success, data) =>
                    {
                        if (success)
                        {
                            SaveData datafile = JsonUtility.FromJson<SaveData>(data);

                            cash = datafile.cash;
                            bgmSet = datafile.bgmSet;
                            seSet = datafile.seSet;
                            language = datafile.language;

                            for (int i = 0; i < 30; i++)
                            {
                                achieve[i] = datafile.achieve[i];
                                leaderboard[i] = datafile.leaderboard[i];
                            }

                            for (int h = 0; h < 5; h++)
                            {
                                unitSlot[h] = datafile.unitSlot[h];
                            }

                            for (int f = 0; f < 3; f++)
                            {
                                heroSlot[f] = datafile.heroSlot[f];
                            }

                            for (int j = 0; j < 64; j++)
                            {
                                unitUnLock[j] = datafile.unitUnLock[j];
                                unitUnLock2[j] = datafile.unitUnLock2[j];
                                unitUnLock3[j] = datafile.unitUnLock3[j];
                                heroUnLock[j] = datafile.heroUnLock[j];
                            }
                        }
                    });
                }
            });
        }
    }

    private void Start()
    {
        MobileAds.Initialize((InitializationStatus initStatus) => { });

        Invoke("FirstStartGame", 3.0f);
    }

    void FirstStartGame()
    {
        //����,���� �ʱ� �δ��ġ
        for (int i = 0; i < unitAttackSet.Length; i++)
            unitAllSet.Add(unitAttackSet[i]);

        for (int i = 0; i < unitDefendSet.Length; i++)
            unitAllSet.Add(unitDefendSet[i]);

        for (int i = 0; i < unitSupportSet.Length; i++)
            unitAllSet.Add(unitSupportSet[i]);

        unitSlot[0] = unitAttackSet[0];
        unitSlot[1] = unitAttackSet[1];
        unitSlot[2] = unitAttackSet[2];
        unitSlot[3] = unitDefendSet[0];
        unitSlot[4] = unitSupportSet[0];

        unitUnLock[0] = true;
        unitUnLock[1] = true;
        unitUnLock[2] = true;
        unitUnLock2[0] = true;
        unitUnLock3[0] = true;

        for (int j = 0; j < heroSlot.Length; j++)
        {
            heroSlot[j] = heroSet[j];
            heroUnLock[j] = true;
        }

        if (gameStart == false)
            gameStart = true;
    }
}
