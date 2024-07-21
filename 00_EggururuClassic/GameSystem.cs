using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

//GameSystem은 게임의 전반적인 시스템입니다

public class GameSystem : MonoBehaviour
{
    //변수 초기화 부분
    public TextMeshProUGUI GamePoint;
    public Move eggPlayer;

    public GameObject[] deco;
    public GameObject[] hurdle;
    public GameObject[] platform;

    public GameObject goldCoin;

    public GameObject wallBreaker;

    public RectTransform Timer;
    public Image GaugeJump;
    public Image GaugeSkill;

    public GameObject GaugeSkillButton;
    public GameObject GaugeSkillButton2;
    public GameObject SkillButton;

    public GameObject BGMButton;
    public GameObject SEButton;

    public RectTransform MainHUD;
    public RectTransform PauseHUD;
    public RectTransform GameOverHUD;

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI HighscoreText;
    public TextMeshProUGUI scorePointText;

    public TextMeshProUGUI RebirthText;
    public TextMeshProUGUI RewardText;

    public Sprite[] skillicon;

    public int RebirthCost = 1;
    public int MoneyPoint = 0;

    public static bool GameOn = true;
    public static bool GameSet = true;

    int CountMove = 0;
    int CountMove2 = 2;

    int[] ran = new int[] { 0, 0, 0, 0, 0, 0 };
    int[] createZ = new int[] { -4, 0, 4, -2, 2 };

    int ranWall= 0;
    int checkWallbreak = 0;

    bool objCreate = false;
    bool platCreate = false;
    bool decoCreate = false;

    public AudioSource[] BGMsound;

    Image SkillColor;
    Image SkillUI;

    bool rebirthUse = false;
    bool rebirthAds = false;

    int modeNum = StartGame.seletMode;
    float gameOverAlpha = 0;
    int checkLocalScore;

    string log;

    void Awake()
    {
        //게임 시작시 초기 설정
        GameOn = true;
        GameSet = true;

        PauseHUD.anchoredPosition = new Vector3(9999, 0, 9999);
        GameOverHUD.anchoredPosition = new Vector3(9999, 0, 9999);
        MainHUD.anchoredPosition = Vector3.zero;
        GameOverHUD.GetComponent<CanvasGroup>().alpha = gameOverAlpha;

        //길 생성 오브젝트 초기화
        ranWall = (int)Random.Range(0,3);

        if (ranWall == 0)
            checkWallbreak = 0;
        else if (ranWall == 1)
            checkWallbreak = 2;
        else
            checkWallbreak = 4;

        //게임모드에 따른 배경음악 실행
        BGMButton.GetComponent<Slider>().value = StartGame.BgmSet;
        SEButton.GetComponent<Slider>().value = StartGame.SeSet;

        BGMplay();

        //점수 및 캐릭터 이동횟수 초기화
        GamePoint.text = string.Format("{0:n0}", PlayerPrefs.GetInt("score"));
        CountMove = eggPlayer.moveCount;

        //장식용 나무 생성 
        for(int i = 0; i < 12; i++)
        {
            ran[0] = (int)Random.Range(0, 2);

            GameObject treeCreate = Instantiate(deco[ran[0]]);
            treeCreate.transform.position = new Vector3(6, 0.2f, i * 5);
            treeCreate.transform.rotation = Quaternion.identity;
        }

        for (int  j = 0; j < 12; j++)
        {
            ran[0] = (int)Random.Range(0, 2);

            GameObject treeCreate = Instantiate(deco[ran[0]]);
            treeCreate.transform.position = new Vector3(-6, 0.2f, j * 5);
            treeCreate.transform.rotation = Quaternion.identity;
        }
        ran[3] = (int)Random.Range(0, 2);

        //게임모드에 따른 hud 설정
        if (StartGame.seletMode == 1)
        {
            GaugeSkillButton.GetComponent<Image>().enabled = true;
            GaugeSkillButton2.GetComponent<Image>().enabled = true;
            SkillButton.GetComponent<Image>().enabled = true;
            SkillButton.GetComponent<Button>().enabled = true;

            SkillColor = GaugeSkillButton.GetComponent<Image>();
            SkillUI = GaugeSkillButton2.GetComponent<Image>();

            if (StartGame.seletSkill == 0)
            {
                SkillColor.color = Color.yellow;
                SkillUI.sprite = skillicon[0];
            }
            else if (StartGame.seletSkill == 1)
            {
                SkillColor.color = Color.green;
                SkillUI.sprite = skillicon[1];
            }
            else if (StartGame.seletSkill == 2)
            {
                SkillColor.color = Color.red;
                SkillUI.sprite = skillicon[2];
            }
            else if (StartGame.seletSkill == 3)
            {
                SkillColor.color = Color.blue;
                SkillUI.sprite = skillicon[3];
            }
        }
        else
        {
            GaugeSkillButton.GetComponent<Image>().enabled = false;
            GaugeSkillButton2.GetComponent<Image>().enabled = false;
            SkillButton.GetComponent<Image>().enabled = false;
            SkillButton.GetComponent<Button>().enabled = false;
        }

    }

    void Update()
    {

        //게임오버시
        if (GameSet == false && GameOn == true)
        {
            GameOverBGM();
            PauseHUD.anchoredPosition = new Vector3(9999, 0, 9999);
            MainHUD.anchoredPosition = new Vector3(9999, 0, 9999);

            //랭킹 등록 및 각 모드별 재화 생산
            if (modeNum == 0)
                MoneyPoint = (int)(Move.score / 75);
            else if (modeNum == 1)
                MoneyPoint = (int)(Move.score / 100);
            else if (modeNum == 2)
                MoneyPoint = (int)(Move.score / 60);
            else if (modeNum == 3)
                MoneyPoint = (int)(Move.score / 60);
            else if (modeNum == 4)
                MoneyPoint = (int)(Move.score / 60);
            else if (modeNum == 5)
                MoneyPoint = (int)(Move.score / 60);

            GameOverHUD.anchoredPosition = Vector3.zero;
            GameOn = false;
        }

        //게임오버시 알파값 변경
        if (GameSet == false && GameOn == false && gameOverAlpha <= 1)
        {
            gameOverAlpha += 0.01f;
            GameOverHUD.GetComponent<CanvasGroup>().alpha = gameOverAlpha;
        }

        //게임 일시정지 상태인지 확인
        if (GameOn && GameSet)
        {

            //플레이어가 이동을 하고 있는지 확인
            //이동하고 있다면 장애물 및 장식품을 생성한다
            if (CountMove != eggPlayer.moveCount)
            {
                objCreate = true;
                decoCreate = true;
                CountMove = eggPlayer.moveCount;
            }

            //발판을 생성해야 되는지 확인
            //생성해야 한다면 발판을 생성한다
            if (CountMove2 == eggPlayer.moveCount)
            {
                platCreate = true;
                CountMove2 = eggPlayer.moveCount + 2;
            }

            //장애물 생성 시스템
            if (objCreate == true)
            {
                //장애물 종류 결정
                ran[2] = (int)Random.Range(0, 2);

                //점수에 따른 장애물 갯수 설정
                if (StartGame.seletMode == 4)
                {
                    if (Move.score >= 2000)
                        ran[0] = (int)Random.Range(1, 3);
                    else if (Move.score >= 1000 && Move.score < 2000)
                        ran[0] = (int)Random.Range(1, 4);
                    else if (Move.score >= 500 && Move.score < 1000)
                        ran[0] = (int)Random.Range(2, 4);
                    else if (Move.score >= 200 && Move.score < 500)
                        ran[0] = (int)Random.Range(2, 5);
                    else
                        ran[0] = (int)Random.Range(3, 5);
                }
                else 
                {
                    if (Move.score >= 2000)
                        ran[0] = (int)Random.Range(3, 5);
                    else if (Move.score >= 1000 && Move.score < 2000)
                        ran[0] = (int)Random.Range(2, 5);
                    else if (Move.score >= 500 && Move.score < 1000)
                        ran[0] = (int)Random.Range(1, 5);
                    else if (Move.score >= 200 && Move.score < 500)
                        ran[0] = (int)Random.Range(1, 4);
                    else
                        ran[0] = (int)Random.Range(1, 3);
                }

                //장애물 생성 부분
                ran[3] = (int)Random.Range(0, 2);

                if (ran[3] == 0)
                {
                    for (int i = 0; i < ran[0]; i++)
                    {
                        GameObject wall = Instantiate(hurdle[ran[2]]);
                        wall.transform.position = new Vector3(createZ[i], 0.2f, 60);
                        wall.transform.rotation = Quaternion.identity;
                    }
                }
                else
                {
                    for (int i = 4; i > 4 - ran[0]; i--)
                    {
                        GameObject wall = Instantiate(hurdle[ran[2]]);
                        wall.transform.position = new Vector3(createZ[i], 0.2f, 60);
                        wall.transform.rotation = Quaternion.identity;
                    }
                }

                //캐주얼모드 : 보물찾기
                if (StartGame.seletMode == 1 && StartGame.seletSkill == 0)
                {
                    int goldSum;
                    goldSum = (int)Random.Range(0, 5);

                    if (goldSum == 1)
                    {
                        for (int i = 0; i < ran[0]; i++)
                        {
                            GameObject gold = Instantiate(goldCoin);
                            gold.transform.position = new Vector3(-4 + (i * 2), 0.2f, 60);
                            gold.transform.rotation = Quaternion.Euler(-90, 0, 0);
                        }
                    }
                }

                //길 생성 오브젝트 생성
                GameObject wallBreak = Instantiate(wallBreaker);
                wallBreak.transform.position = new Vector3(-4 + (checkWallbreak * 2), 0.2f, 60);
                wallBreak.transform.rotation = Quaternion.identity;

                //길 생성 오브젝트 방향 설정
                ran[1] = (int)Random.Range(0, 2);

                if (checkWallbreak == 4)
                    checkWallbreak -= 1;
                else if (checkWallbreak == 0)
                    checkWallbreak += 1;
                else
                {
                    if (ran[1] == 0)
                        checkWallbreak += 1;
                    else
                        checkWallbreak -= 1;
                }

                //길 생성 오브젝트 방향 전환
                if (checkWallbreak > 5)
                    checkWallbreak = 4;
                else if (checkWallbreak < -1)
                    checkWallbreak = 0;

                //종료시 false
                objCreate = false;
            }

            //장식품 생성 시스템
            if (decoCreate == true)
            {
                ran[0] = (int)Random.Range(0, 2);

                GameObject treeCreate = Instantiate(deco[ran[0]]);
                treeCreate.transform.position = new Vector3(6, 0.2f, 60);
                treeCreate.transform.rotation = Quaternion.identity;

                ran[0] = (int)Random.Range(0, 2);

                GameObject treeCreate2 = Instantiate(deco[ran[0]]);
                treeCreate2.transform.position = new Vector3(-6, 0.2f, 60);
                treeCreate2.transform.rotation = Quaternion.identity;

                decoCreate = false;
            }

            //발판 생성 시스템
            if (platCreate == true)
            {
                GameObject floor = Instantiate(platform[0]);
                floor.transform.position = new Vector3(0, 0, 60);
                floor.transform.rotation = Quaternion.identity;

                GameObject floor2 = Instantiate(platform[1]);
                floor2.transform.position = new Vector3(25, 0, 60);
                floor2.transform.rotation = Quaternion.identity;

                GameObject floor3 = Instantiate(platform[1]);
                floor3.transform.position = new Vector3(-25, 0, 60);
                floor3.transform.rotation = Quaternion.identity;

                platCreate = false;
            }

        }
    }

    //게임 일시정지
    public void PauseUse()
    {
        if (GameSet)
        {
            if (GameOn == true)
            {
                MainHUD.anchoredPosition = new Vector3(9999, 0, 9999);
                PauseHUD.anchoredPosition = Vector3.zero;
                GameOn = false;
            }
            else if (GameOn == false)
            {
                PauseHUD.anchoredPosition = new Vector3(9999, 0, 9999);
                MainHUD.anchoredPosition = Vector3.zero;
                GameOn = true;
            }
        }
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            if (GameSet)
            {
                if (GameOn == true)
                {
                    MainHUD.anchoredPosition = new Vector3(9999, 0, 9999);
                    PauseHUD.anchoredPosition = Vector3.zero;
                    GameOn = false;
                }
            }
        }
    }

    void LateUpdate()
    {
        //게임 UI
        GamePoint.text = string.Format("{0:n0}", Move.score);
        float TimerScale = eggPlayer.moveTime / eggPlayer.moveTimeMax;
        Timer.localScale = new Vector3(TimerScale, 1, 1);

        if (TimerScale >= 0.75f)
            Timer.GetComponent<Image>().color = new Color(0.3f, 0.55f, 0.75f);

        else if (TimerScale < 0.75f && TimerScale >= 0.5f)
            Timer.GetComponent<Image>().color = new Color(0.3f, 0.75f, 0.5f);

        else if (TimerScale < 0.5f && TimerScale >= 0.25f)
            Timer.GetComponent<Image>().color = new Color(0.75f, 0.75f, 0.3f);

        else if (TimerScale < 0.25f && TimerScale >= 0)
            Timer.GetComponent<Image>().color = new Color(0.75f, 0.1f, 0.1f);

        GaugeJump.fillAmount = eggPlayer.JumpPoint / 6;

        if (StartGame.seletSkill == 0 || StartGame.seletSkill == 1)
            GaugeSkill.fillAmount = 1;
        else if (StartGame.seletSkill == 2)
            GaugeSkill.fillAmount = eggPlayer.skillGauge / 15;
        else if (StartGame.seletSkill == 3)
            GaugeSkill.fillAmount = eggPlayer.skillGauge / 12;

        //달성한 기록 및 최고기록 표시
        scoreText.text = string.Format("{0:n0}", "달성 기록 : " + Move.score);
        HighscoreText.text = string.Format("{0:n0}", "최고 기록 : " + StartGame.HighScoreLocal1[StartGame.seletMode * 10]);
        scorePointText.text = string.Format("{0:n0}", "획득 깃털 : " + MoneyPoint);

        //부활 버튼 텍스트
        if (StartGame.seletMode == 2)
        {
            RewardText.text = "부활 불가능";
        }
        else
        {
            if (rebirthUse == false)
                RebirthText.text = "깃털 5개 소모하여 부활하기";
            else if (rebirthUse == true)
                RebirthText.text = "사용 완료";
        }

        if (StartGame.seletMode == 2)
        {
            RewardText.text = "부활 불가능";
        }
        else
        {
            if (rebirthAds == false)
                RewardText.text = "광고 시청후      부활하기";
            else if (rebirthAds == true)
                RewardText.text = "사용 완료";
        }
    }

    //다시 타이틀로
    public void ReturnTitle()
    {
        StartGame.diamonds += MoneyPoint;
        StartGame.AchievePoint[11] += MoneyPoint;

        ScoreCheck();

        LoadingSceneManager.LoadScene("title");
        StartGame.titleClickCheck = 1;
    }

    //게임 다시 시작
    public void RestartedGame()
    {
        StartGame.diamonds += MoneyPoint;
        StartGame.AchievePoint[11] += MoneyPoint;

        ScoreCheck();

        LoadingSceneManager.LoadScene("InGame");
    }

    //사망시 bgm 변경
    public void GameOverBGM()
    {
        BGMsound[1].Stop();
        BGMsound[2].Stop();
        BGMsound[3].Stop();

        BGMsound[0].Play();
    }

    //BGM을 모드에 맞춰 실행
    public void BGMplay()
    {
        if (StartGame.seletMode == 0)
            BGMsound[1].Play();
        else if (StartGame.seletMode == 1)
            BGMsound[2].Play();
        else if (StartGame.seletMode >= 2)
            BGMsound[3].Play();
    }

    //BGM설정
    public void BGMSetting()
    {
        StartGame.BgmSet = BGMButton.GetComponent<Slider>().value;
        BGMsound[0].volume = StartGame.BgmSet;
        BGMsound[1].volume = StartGame.BgmSet;
        BGMsound[2].volume = StartGame.BgmSet;
        BGMsound[3].volume = StartGame.BgmSet;
    }

    //SE 설정
    public void SEplay()
    {
        StartGame.SeSet = SEButton.GetComponent<Slider>().value;
    }

    //게임 종료후 기록 체크 및 재화 생산
    public void ScoreCheck()
    {

        //랭킹 등록 및 각 모드별 재화 생산
        if (StartGame.seletMode == 0)
        {
            GPGSBinder.Inst.ReportLeaderboard(GPGSIds.leaderboard, Move.score, success => log = $"{success}");
            
            if (Move.score >= 100)
                GPGSBinder.Inst.UnlockAchievement(GPGSIds.achievement, success => log = $"{success}");

            if (Move.score >= 1000)
                GPGSBinder.Inst.UnlockAchievement(GPGSIds.achievement_2, success => log = $"{success}");

            if (Move.score >= 10000)
                GPGSBinder.Inst.UnlockAchievement(GPGSIds.achievement_3, success => log = $"{success}");

        }
        else if (StartGame.seletMode == 1)
        {
            GPGSBinder.Inst.ReportLeaderboard(GPGSIds.leaderboard_2, Move.score, success => log = $"{success}");

            if (Move.score >= 100)
                GPGSBinder.Inst.UnlockAchievement(GPGSIds.achievement_6, success => log = $"{success}");

            if (Move.score >= 1000)
                GPGSBinder.Inst.UnlockAchievement(GPGSIds.achievement_7, success => log = $"{success}");

            if (Move.score >= 10000)
                GPGSBinder.Inst.UnlockAchievement(GPGSIds.achievement_8, success => log = $"{success}");
        }
        else if (StartGame.seletMode == 2)
        {
            GPGSBinder.Inst.ReportLeaderboard(GPGSIds.leaderboard_3, Move.score, success => log = $"{success}");

            if (Move.score >= 150)
                GPGSBinder.Inst.UnlockAchievement(GPGSIds.achievement_11, success => log = $"{success}");

            if (Move.score >= 300)
                GPGSBinder.Inst.UnlockAchievement(GPGSIds.achievement_12, success => log = $"{success}");

        }
        else if (StartGame.seletMode == 3)
        {
            GPGSBinder.Inst.ReportLeaderboard(GPGSIds.leaderboard_4, Move.score, success => log = $"{success}");

            if (Move.score >= 150)
                GPGSBinder.Inst.UnlockAchievement(GPGSIds.achievement_16, success => log = $"{success}");

            if (Move.score >= 300)
                GPGSBinder.Inst.UnlockAchievement(GPGSIds.achievement_17, success => log = $"{success}");
        }
        else if (StartGame.seletMode == 4)
        {
            GPGSBinder.Inst.ReportLeaderboard(GPGSIds.leaderboard_5, Move.score, success => log = $"{success}");

            if (Move.score >= 150)
                GPGSBinder.Inst.UnlockAchievement(GPGSIds.achievement_21, success => log = $"{success}");

            if (Move.score >= 300)
                GPGSBinder.Inst.UnlockAchievement(GPGSIds.achievement_22, success => log = $"{success}");
        }

        //재화 생산량
        if (StartGame.AchievePoint[11] >= 10)
            GPGSBinder.Inst.UnlockAchievement(GPGSIds.achievement_30, success => log = $"{success}");
        if (StartGame.AchievePoint[11] >= 100)
            GPGSBinder.Inst.UnlockAchievement(GPGSIds.achievement_31, success => log = $"{success}");
        if (StartGame.AchievePoint[11] >= 1000)
            GPGSBinder.Inst.UnlockAchievement(GPGSIds.achievement_32, success => log = $"{success}");
        if (StartGame.AchievePoint[11] >= 5000)
            GPGSBinder.Inst.UnlockAchievement(GPGSIds.achievement_33, success => log = $"{success}");

        //개인기록 등록
        int localScoreStart = 0;
        int localScoreEnd = 10;

        if (StartGame.seletMode == 0)
        {
            localScoreStart = 0;
            localScoreEnd = 10;
        }
        else if (StartGame.seletMode == 1)
        {
            localScoreStart = 10;
            localScoreEnd = 20;
        }
        else if (StartGame.seletMode == 2)
        {
            localScoreStart = 20;
            localScoreEnd = 30;
        }
        else if (StartGame.seletMode == 3)
        {
            localScoreStart = 30;
            localScoreEnd = 40;
        }
        else if (StartGame.seletMode == 4)
        {
            localScoreStart = 40;
            localScoreEnd = 50;
        }
        else if (StartGame.seletMode == 5)
        {
            localScoreStart = 50;
            localScoreEnd = 60;
        }

        //로컬 최고기록 저장
        int scoreSave;
        string scoreDateSave = "";
        bool sortOff = false;

        //랭크에 같은 데이터가 있는지 확인
        for (int u = localScoreStart; u < localScoreEnd; u++)
        {
            if (StartGame.HighScoreLocal1[u] == Move.score)
            {
                sortOff = true;
                break;
            }
        }

        //로컬랭킹에 기록 저장
        if (sortOff == false)
        {
            StartGame.HighScoreLocal1[localScoreEnd - 1] = Move.score;
            StartGame.HighScoreLocalTime1[localScoreEnd - 1] = System.DateTime.Now.ToString();

            for (int i = localScoreEnd - 1; i > localScoreStart; i--)
            {
                for (int j = localScoreStart; j < i; j++)
                {
                    if (StartGame.HighScoreLocal1[j] < StartGame.HighScoreLocal1[j + 1])
                    {
                        scoreSave = StartGame.HighScoreLocal1[j];
                        scoreDateSave = StartGame.HighScoreLocalTime1[j];
                        StartGame.HighScoreLocal1[j] = StartGame.HighScoreLocal1[j + 1];
                        StartGame.HighScoreLocalTime1[j] = StartGame.HighScoreLocalTime1[j + 1];
                        StartGame.HighScoreLocal1[j + 1] = scoreSave;
                        StartGame.HighScoreLocalTime1[j + 1] = scoreDateSave;
                    }
                }
            }
        }

    }

    //사망시 다시 타이틀로
    public void ReturnTitleExit()
    {
        if (gameOverAlpha >= 0.99f)
        {
            StartGame.diamonds += MoneyPoint;
            StartGame.AchievePoint[11] += MoneyPoint;

            ScoreCheck();

            LoadingSceneManager.LoadScene("title");
            StartGame.titleClickCheck = 1;
        }
    }

    //사망시 게임 다시 시작
    public void RestartGame()
    {
        if (gameOverAlpha >= 0.99f)
        {
            StartGame.diamonds += MoneyPoint;
            StartGame.AchievePoint[11] += MoneyPoint;

            ScoreCheck();

            LoadingSceneManager.LoadScene("InGame");
        }
    }

    //재화 소모후 부활
    public void RebirthGame()
    {
        if (gameOverAlpha >= 0.99f)
        {
            //현재 게임모드가 타임어택인지 아닌지 확인
            if (StartGame.seletMode != 2)
            {
                //재화 소모시 부활을 사용했는지 확인,재화가 부활 코스트만큼 있는지 확인
                if (rebirthUse == false && StartGame.diamonds >= 5)
                {
                    Move.startHold = false;

                    StartGame.AchievePoint[13] += 1;
                    if (StartGame.AchievePoint[13] >= 50)
                        GPGSBinder.Inst.UnlockAchievement(GPGSIds.achievement_37, success => log = $"{success}");

                    BGMsound[0].Stop();
                    BGMplay();
                    PauseHUD.anchoredPosition = new Vector3(9999, 0, 9999);
                    GameOverHUD.anchoredPosition = new Vector3(9999, 0, 9999);

                    MainHUD.anchoredPosition = Vector3.zero;
                    StartGame.diamonds -= 5;

                    gameOverAlpha = 0;
                    eggPlayer.moveTime = eggPlayer.moveTimeMax;

                    GameSet = true;
                    GameOn = true;
                    rebirthUse = true;
                }
            }
        }
    }

    //광고 시청후 부활
    public void WatchAdsRebirth()
    {
        if (gameOverAlpha >= 0.99f)
        {
            //현재 게임모드가 타임어택인지 아닌지 확인
            if (StartGame.seletMode != 2)
            {
                //재화 소모시 부활을 사용했는지 확인,재화가 부활 코스트만큼 있는지 확인
                if (rebirthAds == false)
                {
                    Move.startHold = false;

                    StartGame.AchievePoint[14] += 1;
                    if (StartGame.AchievePoint[14] >= 30)
                        GPGSBinder.Inst.UnlockAchievement(GPGSIds.achievement_38, success => log = $"{success}");

                    BGMsound[0].Stop();
                    BGMplay();
                    PauseHUD.anchoredPosition = new Vector3(9999, 0, 9999);
                    GameOverHUD.anchoredPosition = new Vector3(9999, 0, 9999);

                    MainHUD.anchoredPosition = Vector3.zero;

                    gameOverAlpha = 0;
                    eggPlayer.moveTime = eggPlayer.moveTimeMax;

                    GameSet = true;
                    GameOn = true;
                    rebirthAds = true;
                }
            }
        }
    }

}
