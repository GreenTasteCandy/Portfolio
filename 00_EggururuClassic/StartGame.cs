using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using TMPro;
using GoogleMobileAds.Api;
using GooglePlayGames;
using GooglePlayGames.BasicApi;

//StartGame�� ������ �����Ҷ� ���ʷ� �������־�� �� ������ �����ϰų� Ÿ��Ʋ �� ���� ȭ�鿡 ���� �ý����Դϴ�

public class StartGame : MonoBehaviour
{
    //���� �ʱ�ȭ �κ�

    //������ �ʿ��� ����

    //������
    public static int diamonds = 10;

    //���� ĳ����
    public static int CharMesh = 0;
    public static int CharSkin = 0;

    //�������� ���� �ְ���
    public static int[] HighScoreLocal1 = new int[100];
    public static string[] HighScoreLocalTime1 = new string[100];

    //ĳ���� �ر� ����
    public static bool[] EggSkinNum = new bool[21];
    public static bool[] EggSkinNum2 = new bool[21];
    public static bool[] EggSkinNum3 = new bool[21];
    public static bool[] EggSkinNum4 = new bool[21];

    //��������
    public static bool[] AchieveClear = new bool[50];
    public static int[] AchievePoint = new int[50];
    /*
     * 0~4:���� ��庰 �÷��� Ƚ��
     * 11:��ȭ ���귮
     * 12:��� Ƚ��
     * 13:���� ��Ȱ Ƚ��
     * 14:���� ��Ȱ Ƚ��
     * 15~18:��Ų ���� Ƚ��
     */

    //ĳ�־� ��ų/ç���� ��� ���ã��
    public static int[] skillBookmark = new int[5];
    public static int[] modeBookmark = new int[5];

    //������ �ʿ����� �ʴ� �ʱ�ȭ �κ�

    //Ÿ��Ʋ ȭ�鿡�� Ŭ���ߴ��� ���� Ȯ��
    public bool titleClick = false;
    public static int titleClickCheck = 0;

    //���� ������ ���,��ų
    public static int seletMode = 0;
    public static int seletSkill = 0;

    //BGM/SE ����
    public static float BgmSet = 1.0f;
    public static float SeSet = 1.0f;

    //Ÿ��Ʋ �ΰ�� ����
    public RectTransform titleLogo;
    public TextMeshProUGUI titleText;

    //���� ȭ��,���ȭ��,ç�������,ĳ�־���,�ɼ�â
    public GameObject titleScreen;
    public GameObject mainScreen;
    public GameObject modeScreen;
    public GameObject ChallangeScreen;
    public GameObject CasualSkillScreen;
    public GameObject ShopScreen;
    public GameObject OptionScreen;
    public GameObject LocalRankScreen;
    public GameObject GameEndScreen;
    public GameObject creditScreen;

    //BGM/SE ������ư
    public GameObject BgmBT;
    public GameObject SeBT;

    //ĳ�־��� ��ų �̹���
    public Sprite[] skillNum;

    //���� �ɼ� ����
    int optionOn;
    public AudioSource SEsound;
    public AudioSource BGMsound;

    //������ UI
    public GameObject MoneyUI;
    public TextMeshProUGUI MoneyCheck;
    public GameObject MoneyIcon;

    //���� �ְ���
    public int LocalRankNum;
    public TextMeshProUGUI LocalRankMode;
    public TextMeshProUGUI[] LocalRankText;

    //GPGS �α��� ���� Ȯ��
    public static bool GPGSLogin = false;
    public static bool GPGSsave = false;
    public static bool GPGSload = false;

    string log;

    private void Awake()
    {
        //���� ���۽� ������ 60 ����
        Application.targetFrameRate = 60;

        if (GPGSLogin == false)
        {
            //GPGS �α���,Ŭ���� ������ �ҷ�����
            PlayGamesPlatform.Activate();
            PlayGamesPlatform.Instance.Authenticate((status) =>
            {
                if (status == SignInStatus.Success)
                {
                    GPGSLogin = true;

                    GPGSBinder.Inst.LoadCloud("SAVEFILE", (success, data) =>
                    {
                        if (success)
                        {
                            Data datafile = JsonUtility.FromJson<Data>(data);

                            diamonds = datafile.money;
                            CharSkin = datafile.EggChar;
                            CharMesh = datafile.EggMesh;

                            for (int j = 0; j < 21; j++)
                            {
                                EggSkinNum[j] = datafile.SkinNum[j];
                                EggSkinNum2[j] = datafile.SkinNum2[j];
                                EggSkinNum3[j] = datafile.SkinNum3[j];
                                EggSkinNum4[j] = datafile.SkinNum4[j];
                            }

                            for (int l = 0; l < 50; l++)
                            {
                                HighScoreLocal1[l] = datafile.LocalHighScore1[l];
                                HighScoreLocalTime1[l] = datafile.LocalHighScoreTime1[l];
                            }

                            for (int h = 0; h < 50; h++)
                            {
                                AchieveClear[h] = datafile.AchieveOn[h];
                                AchievePoint[h] = datafile.AchieveScore[h];
                            }

                            BgmSet = datafile.BgmSetting;
                            SeSet = datafile.SeSetting;

                            for(int t = 0; t < 5; t++)
                            {
                                skillBookmark[t] = datafile.bookmarkSkill[t];
                                modeBookmark[t] = datafile.bookmarkMode[t];
                            }

                            for (int l = 50; l < 100; l++)
                            {
                                HighScoreLocal1[l] = datafile.LocalHighScore1[l];
                                HighScoreLocalTime1[l] = datafile.LocalHighScoreTime1[l];
                            }

                            BgmBT.GetComponent<Slider>().value = BgmSet;
                            SeBT.GetComponent<Slider>().value = SeSet;

                            BGMsound.volume = BgmSet;
                            SEsound.volume = SeSet;
                        }
                    });
                }
            });

            //Admob �ʱ�ȭ
            MobileAds.Initialize(initStatus => { });
        }

        //Ÿ��Ʋ ���۽� bgm ���� �� �ɼ� ����
        BgmBT.GetComponent<Slider>().value = BgmSet;
        SeBT.GetComponent<Slider>().value = SeSet;

        BGMsound.volume = BgmSet;
        SEsound.volume = SeSet;

        GPGSsave = false;
    }

    private void Start()
    {
        Invoke("GPGSloadEnd", 3f);
    }

    void GPGSloadEnd()
    {
        if (GPGSload == false)
        {
            GPGSload = true;
        }
        BGMsound.Play();
    }

    private void Update()
    {
        if (GPGSload == true && titleClick == false)
        {
            //ȭ�� ��ġ�� Ÿ��Ʋ - ����ȭ�� �Ѿ�� �ڵ�
            if (Input.GetMouseButtonDown(0))
            {
                ClickTitle();
            }

            if (titleClickCheck == 1)
            {
                ClickTitle();
                titleClickCheck = 0;
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (optionOn == 0)
                {
                    ClickOption();
                }
                else if (optionOn == 1)
                {
                    GameExit();
                }
            }
        }

    }

    private void LateUpdate()
    {
        //Ÿ��Ʋ �ȳ���
        if (titleClick == false)
        {
            if (GPGSLogin == false)
                titleText.text = "�α����� �����ϴ� ��....";
            else if (GPGSLogin == true)
            {
                if (GPGSload == false)
                    titleText.text = "����� �����͸� �ҷ����� ��....";
                else
                    titleText.text = "ȭ���� ��ġ�ϸ� ������ ���۵˴ϴ�";
            }
        }
        else if (titleClick == true)
            titleText.text = "";

        //��ȭ ui ǥ��
        MoneyCheck.text = string.Format("{0:n0}", diamonds);

        //���� �ְ��� ǥ��
        if (LocalRankNum == 0)
            LocalRankMode.text = string.Format("{0:n0}", "Ŭ���� ��� ���");
        else if (LocalRankNum == 1)
            LocalRankMode.text = string.Format("{0:n0}", "ĳ�־� ��� ���");
        else if (LocalRankNum == 2)
            LocalRankMode.text = string.Format("{0:n0}", "ç���� : Ÿ�Ӿ��� ���");
        else if (LocalRankNum == 3)
            LocalRankMode.text = string.Format("{0:n0}", "ç���� : �� ��å ���");
        else if (LocalRankNum == 4)
            LocalRankMode.text = string.Format("{0:n0}", "ç���� : ��������Ʈ ���");
        else if (LocalRankNum == 5)
            LocalRankMode.text = string.Format("{0:n0}", "ç���� : ���⿪������ ���");

        for (int i = LocalRankNum*10; i < (LocalRankNum*10) + 10; i++)
        {
            int lRankNum = i - (LocalRankNum * 10);
            LocalRankText[lRankNum].text = string.Format("{0:n0}", (lRankNum + 1) + " : " + HighScoreLocal1[i] + " / ") + HighScoreLocalTime1[i];
        }
    }

    //�������� �ǵ��ư���
    public void ReturnTitle()
    {
        SEsound.Play();

        mainScreen.SetActive(true);

        modeScreen.SetActive(false);
        ChallangeScreen.SetActive(false);
        CasualSkillScreen.SetActive(false);
        OptionScreen.SetActive(false);
        ShopScreen.SetActive(false);
        LocalRankScreen.SetActive(false);
        creditScreen.SetActive(false);
    }

    //Ÿ��Ʋ���� Ŭ���� ��������
    public void ClickTitle()
    {
        SEsound.Play();
        titleScreen.SetActive(false);
        mainScreen.SetActive(true);

        MoneyUI.GetComponent<Image>().enabled = true;
        MoneyCheck.GetComponent<TMP_Text>().enabled = true;
        MoneyIcon.GetComponent<Image>().enabled = true;

        titleClick = true;
    }

    //��� ���� ȭ������
    public void ClickMode()
    {
        SEsound.Play();
        mainScreen.SetActive(false);
        modeScreen.SetActive(true);
    }

    //Ŭ���ĸ�� ����
    public void ClickModeClassic()
    {
        SEsound.Play();
        AchieveUnlock();

        seletMode = 0;
        LoadingSceneManager.LoadScene("InGame");
    }

    //ĳ�־� ��� ����
    public void ClickModeCasual()
    {
        SEsound.Play();

        modeScreen.SetActive(false);
        CasualSkillScreen.SetActive(true);
    }

    //ĳ�־� ��� ��ų 1�� ����
    public void ClickCasualSkill()
    {
        SEsound.Play();
        AchieveUnlock();

        seletMode = 1;
        seletSkill = 0;

        LoadingSceneManager.LoadScene("InGame");
    }

    //ĳ�־� ��� ��ų 2�� ����
    public void ClickCasualSkill2()
    {
        SEsound.Play();
        AchieveUnlock();

        seletMode = 1;
        seletSkill = 1;

        LoadingSceneManager.LoadScene("InGame");
    }

    //ĳ�־� ��� ��ų 3�� ����
    public void ClickCasualSkill3()
    {
        SEsound.Play();
        AchieveUnlock();

        seletMode = 1;
        seletSkill = 2;

        LoadingSceneManager.LoadScene("InGame");
    }

    //ĳ�־� ��� ��ų 4�� ����
    public void ClickCasualSkill4()
    {
        SEsound.Play();
        AchieveUnlock();

        seletMode = 1;
        seletSkill = 3;

        LoadingSceneManager.LoadScene("InGame");
    }

    //ç������� ���� ȭ������
    public void ClickModeTimeAttack()
    {
        SEsound.Play();

        ChallangeScreen.SetActive(true);
        modeScreen.SetActive(false);
    }

    //ç������� : Ÿ�Ӿ��� ����
    public void ClickChallangeMode1()
    {
        SEsound.Play();
        AchieveUnlock();

        seletMode = 2;
        LoadingSceneManager.LoadScene("InGame");
    }

    //ç������� : ���å ����
    public void ClickChallangeMode2()
    {
        SEsound.Play();
        AchieveUnlock();

        seletMode = 3;
        LoadingSceneManager.LoadScene("InGame");
    }

    //ç������� : ��������Ʈ ����
    public void ClickChallangeMode3()
    {
        SEsound.Play();
        AchieveUnlock();

        seletMode = 4;
        LoadingSceneManager.LoadScene("InGame");
    }

    //ç������� : ���⿪������
    public void ClickChallangeMode4()
    {
        SEsound.Play();
        AchieveUnlock();

        seletMode = 5;
        LoadingSceneManager.LoadScene("InGame");
    }

    //���� ȭ������
    public void ClickShop()
    {
        SEsound.Play();

        ShopScreen.SetActive(true);
        mainScreen.SetActive(false);
    }

    //���ӿɼ� ȭ������
    public void ClickOption()
    {
        SEsound.Play();

        OptionScreen.SetActive(true);
        mainScreen.SetActive(true);

        optionOn = 1;
    }

    //���η�ŷ ȭ��
    public void ClickLocalRank()
    {
        SEsound.Play();

        LocalRankScreen.SetActive(true);
        mainScreen.SetActive(false);
    }

    //���α�� ����
    public void LocalRankLeft()
    {
        SEsound.Play();

        LocalRankNum -= 1;
        if (LocalRankNum < 0)
            LocalRankNum = 5;
    }

    //���α�� ������
    public void LocalRankRight()
    {
        SEsound.Play();

        LocalRankNum += 1;
        if (LocalRankNum > 5)
            LocalRankNum = 0;
    }

    //��ŷ ȭ�� ǥ��
    public void ClickRank() => Social.ShowLeaderboardUI();

    //���� ȭ�� ǥ��
    public void ClickAchieve() => Social.ShowAchievementsUI();

    //Ÿ��Ʋ bgm ����
    public void TitleBGMplay()
    {
        BgmSet = BgmBT.GetComponent<Slider>().value;
        BGMsound.volume = BgmSet;
    }

    //Ÿ��Ʋ se ����
    public void TitleSEplay()
    {
        SeSet = SeBT.GetComponent<Slider>().value;
        SEsound.volume = SeSet;
    }

    //���� �÷��� ���� ���� �ر�
    public void AchieveUnlock()
    {
        //���� ��庰 �÷��� Ƚ��
        AchievePoint[seletMode] += 1;

        if (AchievePoint[0] >= 5)
            GPGSBinder.Inst.UnlockAchievement(GPGSIds.achievement_4, success => log = $"{success}");
        if (AchievePoint[0] >= 50)
            GPGSBinder.Inst.UnlockAchievement(GPGSIds.achievement_5, success => log = $"{success}");

        if (AchievePoint[1] >= 5)
            GPGSBinder.Inst.UnlockAchievement(GPGSIds.achievement_9, success => log = $"{success}");
        if (AchievePoint[1] >= 50)
            GPGSBinder.Inst.UnlockAchievement(GPGSIds.achievement_10, success => log = $"{success}");

        if (AchievePoint[2] >= 10)
            GPGSBinder.Inst.UnlockAchievement(GPGSIds.achievement_13, success => log = $"{success}");
        if (AchievePoint[2] >= 100)
            GPGSBinder.Inst.UnlockAchievement(GPGSIds.achievement_14, success => log = $"{success}");
        if (AchievePoint[2] >= 1000)
            GPGSBinder.Inst.UnlockAchievement(GPGSIds.achievement_15, success => log = $"{success}");

        if (AchievePoint[3] >= 10)
            GPGSBinder.Inst.UnlockAchievement(GPGSIds.achievement_18, success => log = $"{success}");
        if (AchievePoint[3] >= 100)
            GPGSBinder.Inst.UnlockAchievement(GPGSIds.achievement_19, success => log = $"{success}");
        if (AchievePoint[3] >= 1000)
            GPGSBinder.Inst.UnlockAchievement(GPGSIds.achievement_20, success => log = $"{success}");

        if (AchievePoint[4] >= 10)
            GPGSBinder.Inst.UnlockAchievement(GPGSIds.achievement_23, success => log = $"{success}");
        if (AchievePoint[4] >= 100)
            GPGSBinder.Inst.UnlockAchievement(GPGSIds.achievement_24, success => log = $"{success}");
        if (AchievePoint[4] >= 1000)
            GPGSBinder.Inst.UnlockAchievement(GPGSIds.achievement_25, success => log = $"{success}");

    }

    //���� ����
    public void GameExit()
    {
        GameEndScreen.SetActive(true);
        CloudSave.Instance.SaveWithCloud(0);
    }

}