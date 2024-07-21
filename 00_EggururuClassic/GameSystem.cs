using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

//GameSystem�� ������ �������� �ý����Դϴ�

public class GameSystem : MonoBehaviour
{
    //���� �ʱ�ȭ �κ�
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
        //���� ���۽� �ʱ� ����
        GameOn = true;
        GameSet = true;

        PauseHUD.anchoredPosition = new Vector3(9999, 0, 9999);
        GameOverHUD.anchoredPosition = new Vector3(9999, 0, 9999);
        MainHUD.anchoredPosition = Vector3.zero;
        GameOverHUD.GetComponent<CanvasGroup>().alpha = gameOverAlpha;

        //�� ���� ������Ʈ �ʱ�ȭ
        ranWall = (int)Random.Range(0,3);

        if (ranWall == 0)
            checkWallbreak = 0;
        else if (ranWall == 1)
            checkWallbreak = 2;
        else
            checkWallbreak = 4;

        //���Ӹ�忡 ���� ������� ����
        BGMButton.GetComponent<Slider>().value = StartGame.BgmSet;
        SEButton.GetComponent<Slider>().value = StartGame.SeSet;

        BGMplay();

        //���� �� ĳ���� �̵�Ƚ�� �ʱ�ȭ
        GamePoint.text = string.Format("{0:n0}", PlayerPrefs.GetInt("score"));
        CountMove = eggPlayer.moveCount;

        //��Ŀ� ���� ���� 
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

        //���Ӹ�忡 ���� hud ����
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

        //���ӿ�����
        if (GameSet == false && GameOn == true)
        {
            GameOverBGM();
            PauseHUD.anchoredPosition = new Vector3(9999, 0, 9999);
            MainHUD.anchoredPosition = new Vector3(9999, 0, 9999);

            //��ŷ ��� �� �� ��庰 ��ȭ ����
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

        //���ӿ����� ���İ� ����
        if (GameSet == false && GameOn == false && gameOverAlpha <= 1)
        {
            gameOverAlpha += 0.01f;
            GameOverHUD.GetComponent<CanvasGroup>().alpha = gameOverAlpha;
        }

        //���� �Ͻ����� �������� Ȯ��
        if (GameOn && GameSet)
        {

            //�÷��̾ �̵��� �ϰ� �ִ��� Ȯ��
            //�̵��ϰ� �ִٸ� ��ֹ� �� ���ǰ�� �����Ѵ�
            if (CountMove != eggPlayer.moveCount)
            {
                objCreate = true;
                decoCreate = true;
                CountMove = eggPlayer.moveCount;
            }

            //������ �����ؾ� �Ǵ��� Ȯ��
            //�����ؾ� �Ѵٸ� ������ �����Ѵ�
            if (CountMove2 == eggPlayer.moveCount)
            {
                platCreate = true;
                CountMove2 = eggPlayer.moveCount + 2;
            }

            //��ֹ� ���� �ý���
            if (objCreate == true)
            {
                //��ֹ� ���� ����
                ran[2] = (int)Random.Range(0, 2);

                //������ ���� ��ֹ� ���� ����
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

                //��ֹ� ���� �κ�
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

                //ĳ�־��� : ����ã��
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

                //�� ���� ������Ʈ ����
                GameObject wallBreak = Instantiate(wallBreaker);
                wallBreak.transform.position = new Vector3(-4 + (checkWallbreak * 2), 0.2f, 60);
                wallBreak.transform.rotation = Quaternion.identity;

                //�� ���� ������Ʈ ���� ����
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

                //�� ���� ������Ʈ ���� ��ȯ
                if (checkWallbreak > 5)
                    checkWallbreak = 4;
                else if (checkWallbreak < -1)
                    checkWallbreak = 0;

                //����� false
                objCreate = false;
            }

            //���ǰ ���� �ý���
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

            //���� ���� �ý���
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

    //���� �Ͻ�����
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
        //���� UI
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

        //�޼��� ��� �� �ְ��� ǥ��
        scoreText.text = string.Format("{0:n0}", "�޼� ��� : " + Move.score);
        HighscoreText.text = string.Format("{0:n0}", "�ְ� ��� : " + StartGame.HighScoreLocal1[StartGame.seletMode * 10]);
        scorePointText.text = string.Format("{0:n0}", "ȹ�� ���� : " + MoneyPoint);

        //��Ȱ ��ư �ؽ�Ʈ
        if (StartGame.seletMode == 2)
        {
            RewardText.text = "��Ȱ �Ұ���";
        }
        else
        {
            if (rebirthUse == false)
                RebirthText.text = "���� 5�� �Ҹ��Ͽ� ��Ȱ�ϱ�";
            else if (rebirthUse == true)
                RebirthText.text = "��� �Ϸ�";
        }

        if (StartGame.seletMode == 2)
        {
            RewardText.text = "��Ȱ �Ұ���";
        }
        else
        {
            if (rebirthAds == false)
                RewardText.text = "���� ��û��      ��Ȱ�ϱ�";
            else if (rebirthAds == true)
                RewardText.text = "��� �Ϸ�";
        }
    }

    //�ٽ� Ÿ��Ʋ��
    public void ReturnTitle()
    {
        StartGame.diamonds += MoneyPoint;
        StartGame.AchievePoint[11] += MoneyPoint;

        ScoreCheck();

        LoadingSceneManager.LoadScene("title");
        StartGame.titleClickCheck = 1;
    }

    //���� �ٽ� ����
    public void RestartedGame()
    {
        StartGame.diamonds += MoneyPoint;
        StartGame.AchievePoint[11] += MoneyPoint;

        ScoreCheck();

        LoadingSceneManager.LoadScene("InGame");
    }

    //����� bgm ����
    public void GameOverBGM()
    {
        BGMsound[1].Stop();
        BGMsound[2].Stop();
        BGMsound[3].Stop();

        BGMsound[0].Play();
    }

    //BGM�� ��忡 ���� ����
    public void BGMplay()
    {
        if (StartGame.seletMode == 0)
            BGMsound[1].Play();
        else if (StartGame.seletMode == 1)
            BGMsound[2].Play();
        else if (StartGame.seletMode >= 2)
            BGMsound[3].Play();
    }

    //BGM����
    public void BGMSetting()
    {
        StartGame.BgmSet = BGMButton.GetComponent<Slider>().value;
        BGMsound[0].volume = StartGame.BgmSet;
        BGMsound[1].volume = StartGame.BgmSet;
        BGMsound[2].volume = StartGame.BgmSet;
        BGMsound[3].volume = StartGame.BgmSet;
    }

    //SE ����
    public void SEplay()
    {
        StartGame.SeSet = SEButton.GetComponent<Slider>().value;
    }

    //���� ������ ��� üũ �� ��ȭ ����
    public void ScoreCheck()
    {

        //��ŷ ��� �� �� ��庰 ��ȭ ����
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

        //��ȭ ���귮
        if (StartGame.AchievePoint[11] >= 10)
            GPGSBinder.Inst.UnlockAchievement(GPGSIds.achievement_30, success => log = $"{success}");
        if (StartGame.AchievePoint[11] >= 100)
            GPGSBinder.Inst.UnlockAchievement(GPGSIds.achievement_31, success => log = $"{success}");
        if (StartGame.AchievePoint[11] >= 1000)
            GPGSBinder.Inst.UnlockAchievement(GPGSIds.achievement_32, success => log = $"{success}");
        if (StartGame.AchievePoint[11] >= 5000)
            GPGSBinder.Inst.UnlockAchievement(GPGSIds.achievement_33, success => log = $"{success}");

        //���α�� ���
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

        //���� �ְ��� ����
        int scoreSave;
        string scoreDateSave = "";
        bool sortOff = false;

        //��ũ�� ���� �����Ͱ� �ִ��� Ȯ��
        for (int u = localScoreStart; u < localScoreEnd; u++)
        {
            if (StartGame.HighScoreLocal1[u] == Move.score)
            {
                sortOff = true;
                break;
            }
        }

        //���÷�ŷ�� ��� ����
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

    //����� �ٽ� Ÿ��Ʋ��
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

    //����� ���� �ٽ� ����
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

    //��ȭ �Ҹ��� ��Ȱ
    public void RebirthGame()
    {
        if (gameOverAlpha >= 0.99f)
        {
            //���� ���Ӹ�尡 Ÿ�Ӿ������� �ƴ��� Ȯ��
            if (StartGame.seletMode != 2)
            {
                //��ȭ �Ҹ�� ��Ȱ�� ����ߴ��� Ȯ��,��ȭ�� ��Ȱ �ڽ�Ʈ��ŭ �ִ��� Ȯ��
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

    //���� ��û�� ��Ȱ
    public void WatchAdsRebirth()
    {
        if (gameOverAlpha >= 0.99f)
        {
            //���� ���Ӹ�尡 Ÿ�Ӿ������� �ƴ��� Ȯ��
            if (StartGame.seletMode != 2)
            {
                //��ȭ �Ҹ�� ��Ȱ�� ����ߴ��� Ȯ��,��ȭ�� ��Ȱ �ڽ�Ʈ��ŭ �ִ��� Ȯ��
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
