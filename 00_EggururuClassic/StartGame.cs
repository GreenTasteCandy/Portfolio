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

//StartGame은 게임을 시작할때 최초로 설정해주어야 할 값들을 설정하거나 타이틀 및 메인 화면에 대한 시스템입니다

public class StartGame : MonoBehaviour
{
    //변수 초기화 부분

    //저장이 필요한 변수

    //소지금
    public static int diamonds = 10;

    //현재 캐릭터
    public static int CharMesh = 0;
    public static int CharSkin = 0;

    //컨텐츠별 개인 최고기록
    public static int[] HighScoreLocal1 = new int[100];
    public static string[] HighScoreLocalTime1 = new string[100];

    //캐릭터 해금 여부
    public static bool[] EggSkinNum = new bool[21];
    public static bool[] EggSkinNum2 = new bool[21];
    public static bool[] EggSkinNum3 = new bool[21];
    public static bool[] EggSkinNum4 = new bool[21];

    //도전과제
    public static bool[] AchieveClear = new bool[50];
    public static int[] AchievePoint = new int[50];
    /*
     * 0~4:게임 모드별 플레이 횟수
     * 11:재화 생산량
     * 12:사망 횟수
     * 13:깃털 부활 횟수
     * 14:광고 부활 횟수
     * 15~18:스킨 구매 횟수
     */

    //캐주얼 스킬/챌린지 모드 즐겨찾기
    public static int[] skillBookmark = new int[5];
    public static int[] modeBookmark = new int[5];

    //저장이 필요하지 않는 초기화 부분

    //타이틀 화면에서 클릭했는지 여부 확인
    public bool titleClick = false;
    public static int titleClickCheck = 0;

    //현재 선택한 모드,스킬
    public static int seletMode = 0;
    public static int seletSkill = 0;

    //BGM/SE 설정
    public static float BgmSet = 1.0f;
    public static float SeSet = 1.0f;

    //타이틀 로고와 문구
    public RectTransform titleLogo;
    public TextMeshProUGUI titleText;

    //메인 화면,모드화면,챌린지모드,캐주얼모드,옵션창
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

    //BGM/SE 설정버튼
    public GameObject BgmBT;
    public GameObject SeBT;

    //캐주얼모드 스킬 이미지
    public Sprite[] skillNum;

    //게임 옵션 관련
    int optionOn;
    public AudioSource SEsound;
    public AudioSource BGMsound;

    //소지금 UI
    public GameObject MoneyUI;
    public TextMeshProUGUI MoneyCheck;
    public GameObject MoneyIcon;

    //로컬 최고기록
    public int LocalRankNum;
    public TextMeshProUGUI LocalRankMode;
    public TextMeshProUGUI[] LocalRankText;

    //GPGS 로그인 여부 확인
    public static bool GPGSLogin = false;
    public static bool GPGSsave = false;
    public static bool GPGSload = false;

    string log;

    private void Awake()
    {
        //게임 시작시 프레임 60 고정
        Application.targetFrameRate = 60;

        if (GPGSLogin == false)
        {
            //GPGS 로그인,클라우드 데이터 불러오기
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

            //Admob 초기화
            MobileAds.Initialize(initStatus => { });
        }

        //타이틀 시작시 bgm 실행 및 옵션 설정
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
            //화면 터치시 타이틀 - 메인화면 넘어가는 코드
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
        //타이틀 안내문
        if (titleClick == false)
        {
            if (GPGSLogin == false)
                titleText.text = "로그인을 진행하는 중....";
            else if (GPGSLogin == true)
            {
                if (GPGSload == false)
                    titleText.text = "저장된 데이터를 불러오는 중....";
                else
                    titleText.text = "화면을 터치하면 게임이 시작됩니다";
            }
        }
        else if (titleClick == true)
            titleText.text = "";

        //재화 ui 표시
        MoneyCheck.text = string.Format("{0:n0}", diamonds);

        //로컬 최고기록 표시
        if (LocalRankNum == 0)
            LocalRankMode.text = string.Format("{0:n0}", "클래식 모드 기록");
        else if (LocalRankNum == 1)
            LocalRankMode.text = string.Format("{0:n0}", "캐주얼 모드 기록");
        else if (LocalRankNum == 2)
            LocalRankMode.text = string.Format("{0:n0}", "챌린지 : 타임어택 기록");
        else if (LocalRankNum == 3)
            LocalRankMode.text = string.Format("{0:n0}", "챌린지 : 밤 산책 기록");
        else if (LocalRankNum == 4)
            LocalRankMode.text = string.Format("{0:n0}", "챌린지 : 도그파이트 기록");
        else if (LocalRankNum == 5)
            LocalRankMode.text = string.Format("{0:n0}", "챌린지 : 방향역전세계 기록");

        for (int i = LocalRankNum*10; i < (LocalRankNum*10) + 10; i++)
        {
            int lRankNum = i - (LocalRankNum * 10);
            LocalRankText[lRankNum].text = string.Format("{0:n0}", (lRankNum + 1) + " : " + HighScoreLocal1[i] + " / ") + HighScoreLocalTime1[i];
        }
    }

    //메인으로 되돌아가기
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

    //타이틀에서 클릭후 메인으로
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

    //모드 선택 화면으로
    public void ClickMode()
    {
        SEsound.Play();
        mainScreen.SetActive(false);
        modeScreen.SetActive(true);
    }

    //클래식모드 실행
    public void ClickModeClassic()
    {
        SEsound.Play();
        AchieveUnlock();

        seletMode = 0;
        LoadingSceneManager.LoadScene("InGame");
    }

    //캐주얼 모드 실행
    public void ClickModeCasual()
    {
        SEsound.Play();

        modeScreen.SetActive(false);
        CasualSkillScreen.SetActive(true);
    }

    //캐주얼 모드 스킬 1번 선택
    public void ClickCasualSkill()
    {
        SEsound.Play();
        AchieveUnlock();

        seletMode = 1;
        seletSkill = 0;

        LoadingSceneManager.LoadScene("InGame");
    }

    //캐주얼 모드 스킬 2번 선택
    public void ClickCasualSkill2()
    {
        SEsound.Play();
        AchieveUnlock();

        seletMode = 1;
        seletSkill = 1;

        LoadingSceneManager.LoadScene("InGame");
    }

    //캐주얼 모드 스킬 3번 선택
    public void ClickCasualSkill3()
    {
        SEsound.Play();
        AchieveUnlock();

        seletMode = 1;
        seletSkill = 2;

        LoadingSceneManager.LoadScene("InGame");
    }

    //캐주얼 모드 스킬 4번 선택
    public void ClickCasualSkill4()
    {
        SEsound.Play();
        AchieveUnlock();

        seletMode = 1;
        seletSkill = 3;

        LoadingSceneManager.LoadScene("InGame");
    }

    //챌린지모드 선택 화면으로
    public void ClickModeTimeAttack()
    {
        SEsound.Play();

        ChallangeScreen.SetActive(true);
        modeScreen.SetActive(false);
    }

    //챌린지모드 : 타임어택 실행
    public void ClickChallangeMode1()
    {
        SEsound.Play();
        AchieveUnlock();

        seletMode = 2;
        LoadingSceneManager.LoadScene("InGame");
    }

    //챌린지모드 : 밤산책 실행
    public void ClickChallangeMode2()
    {
        SEsound.Play();
        AchieveUnlock();

        seletMode = 3;
        LoadingSceneManager.LoadScene("InGame");
    }

    //챌린지모드 : 도그파이트 실행
    public void ClickChallangeMode3()
    {
        SEsound.Play();
        AchieveUnlock();

        seletMode = 4;
        LoadingSceneManager.LoadScene("InGame");
    }

    //챌린지모드 : 방향역전세계
    public void ClickChallangeMode4()
    {
        SEsound.Play();
        AchieveUnlock();

        seletMode = 5;
        LoadingSceneManager.LoadScene("InGame");
    }

    //상점 화면으로
    public void ClickShop()
    {
        SEsound.Play();

        ShopScreen.SetActive(true);
        mainScreen.SetActive(false);
    }

    //게임옵션 화면으로
    public void ClickOption()
    {
        SEsound.Play();

        OptionScreen.SetActive(true);
        mainScreen.SetActive(true);

        optionOn = 1;
    }

    //개인랭킹 화면
    public void ClickLocalRank()
    {
        SEsound.Play();

        LocalRankScreen.SetActive(true);
        mainScreen.SetActive(false);
    }

    //개인기록 왼쪽
    public void LocalRankLeft()
    {
        SEsound.Play();

        LocalRankNum -= 1;
        if (LocalRankNum < 0)
            LocalRankNum = 5;
    }

    //개인기록 오른쪽
    public void LocalRankRight()
    {
        SEsound.Play();

        LocalRankNum += 1;
        if (LocalRankNum > 5)
            LocalRankNum = 0;
    }

    //랭킹 화면 표시
    public void ClickRank() => Social.ShowLeaderboardUI();

    //업적 화면 표시
    public void ClickAchieve() => Social.ShowAchievementsUI();

    //타이틀 bgm 설정
    public void TitleBGMplay()
    {
        BgmSet = BgmBT.GetComponent<Slider>().value;
        BGMsound.volume = BgmSet;
    }

    //타이틀 se 설정
    public void TitleSEplay()
    {
        SeSet = SeBT.GetComponent<Slider>().value;
        SEsound.volume = SeSet;
    }

    //게임 플레이 관련 업적 해금
    public void AchieveUnlock()
    {
        //게임 모드별 플레이 횟수
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

    //게임 종료
    public void GameExit()
    {
        GameEndScreen.SetActive(true);
        CloudSave.Instance.SaveWithCloud(0);
    }

}