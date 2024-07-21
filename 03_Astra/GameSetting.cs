using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using GooglePlayGames;
using GooglePlayGames.BasicApi;

/*
 * 초기 게임 세팅값
 */

public enum UI { LobbyUI = 0, Hud, Explore, Deal, Equip, Storage, Request, Ship, Chat, PlanetInfo, Planet }

public class GameSetting : MonoBehaviour
{
    static public GameSetting ins;

    [Header("Player Status")]
    public UserData player;

    [Header("Money")]
    public static int gold = 0;
    public int crescent;

    //플레이어 능력치
    [Header("Player Status")]
    public string nickName;
    public float hp;
    public float maxhp;
    public int weapons;

    //창고 데이터
    [Header("Inventory")]
    public ItemData supply;

    //의뢰 데이터
    [Header("Requests")]
    public RequestData requestTable;

    //거래 데이터
    [Header("Merchants")]
    public Deal deal;
    public float normalRange = 0.7f;
    public float rareRange = 0.4f;
    public float epicRange = 0.2f;
    public float legendRange = 0.1f;

    //장비 데이터
    [Header("Equipment")]
    public WeaponData weaponAll;
    public WeaponData equipWeapon;

    //옵션 설정값
    [Header("Options")]
    static public float bgmValue = 0.5f;
    static public float sfxValue = 0.5f;
    static public float lookValue = 0.5f;
    static public bool isMinimap = true;
    static public bool isCompass = true;

    [Header("Dialog")]
    public DialogData dialogTable;
    public ChatManager chatManager;

    //데이터 테이블들
    //해당 테이블은 이 스크립트에서 스프레드시트값을 불러오고 다른곳에서도 사용하게 된다
    [Header("Data Table")]
    public LinkedTable linkedTable;
    public ItemData itemTable;
    public EnemyData enemyTable;
    public MerchantData merchantTable;
    public WeaponData weaponTable;
    public DialogData mainDialog;

    [Header("UI")]
    public Image logo;
    public GameObject titleText;
    public GameObject title;
    public GameObject lobbyUI;
    public GameObject hud;
    public GameObject exploreUI;
    public GameObject dealUI;
    public GameObject equipUI;
    public GameObject storageUI;
    public GameObject requestUI;
    public GameObject shipUI;
    public GameObject ship;
    public GameObject chatUI;
    public GameObject planetInfoUI;
    public GameObject planet;

    [Header("Buttons")]
    public GameObject exploreButton;
    public GameObject dealButton;
    public GameObject equipButton;
    public GameObject storageButton;
    public GameObject requestButton;
    public GameObject shipButton;
    public UI ui;

    public AudioSource audioSource;
    public AudioClip lobbyBGM;

    public float delay;
    public bool titleOn;

    static public bool isStart = false;

    static public bool isLogin;
    static public bool isCloud;

    //데이터 테이블 리스트들
    const string URL = "https://docs.google.com/spreadsheets/d/1cBgCyx3vzOXo9Fz3vpJrafFtcOKN9tQXgsOhMkJF3Bs/export?format=csv&gid=0&range=A2:F100";
    const string URL2 = "https://docs.google.com/spreadsheets/d/1cBgCyx3vzOXo9Fz3vpJrafFtcOKN9tQXgsOhMkJF3Bs/export?format=csv&gid=1016137814&range=A2:H10";
    const string URL3 = "https://docs.google.com/spreadsheets/d/1cBgCyx3vzOXo9Fz3vpJrafFtcOKN9tQXgsOhMkJF3Bs/export?format=csv&gid=623927173&range=A2:O5";
    const string URL4 = "https://docs.google.com/spreadsheets/d/1cBgCyx3vzOXo9Fz3vpJrafFtcOKN9tQXgsOhMkJF3Bs/export?format=csv&gid=1891064523&range=A2:K13";
    const string URL5 = "https://docs.google.com/spreadsheets/d/1cBgCyx3vzOXo9Fz3vpJrafFtcOKN9tQXgsOhMkJF3Bs/export?format=csv&gid=555220467&range=A2:C30";
    const string URL6 = "https://docs.google.com/spreadsheets/d/1cBgCyx3vzOXo9Fz3vpJrafFtcOKN9tQXgsOhMkJF3Bs/export?format=csv&gid=1668105367&range=A2:O391";
    const string URL7 = "https://docs.google.com/spreadsheets/d/1cBgCyx3vzOXo9Fz3vpJrafFtcOKN9tQXgsOhMkJF3Bs/export?format=csv&gid=1835097952&range=A2:AD27";

    private void Awake()
    {
        ins = this; //자기참조

        if (isLogin == false && isCloud == false)
        {
            //GPGS 로그인을 진행했는지 체크
            GPGSCloudSavingLoad();
        }
    }

    void GPGSCloudSavingLoad()
    {
        PlayGamesPlatform.Activate(); //GPGS 로그인

        //GPGS 로그인을 진행했는지 체크
        PlayGamesPlatform.Instance.Authenticate((status) =>
        {
            bool isPlayer = false;
            bool isItem = false;
            bool isRequest = false;

            //로그인에 성공했을 경우
            if (status == SignInStatus.Success)
            {
                isLogin = true;

                GPGSBinder.Inst.LoadCloud("SaveUser", (success, data) =>
                {
                    if (success)
                    {
                        UserStatus status = JsonUtility.FromJson<UserStatus>(data);
                        player.user.isFirst = status.isFirst;

                        player.user.money = status.money;
                        player.user.armorLv = status.armorLv;
                        player.user.gloveLv = status.gloveLv;
                        player.user.bootLv = status.bootLv;

                        player.user.weaponLv[0] = status.weaponLv[0];
                        player.user.weaponLv[1] = status.weaponLv[1];
                        player.user.weaponLv[2] = status.weaponLv[2];
                        player.user.weaponLv[3] = status.weaponLv[3];

                        player.user.toolLv[0] = status.toolLv[0];

                        isPlayer = true;
                    }
                });

                GPGSBinder.Inst.LoadCloud("SaveItem", (success, data) =>
                {
                    if (success)
                    {
                        PlayerInven itemNum = JsonUtility.FromJson<PlayerInven>(data);

                        for (int i = 0; i < player.inven.itemNum.Length; i++)
                        {
                            player.inven.itemNum[i] = itemNum.itemNum[i];
                        }

                        isItem = true;
                    }

                });


                GPGSBinder.Inst.LoadCloud("SaveRequest", (success, data) =>
                {
                    if (success)
                    {
                        PlayerRequest requset = JsonUtility.FromJson<PlayerRequest>(data);

                        for (int i = 0; i < player.request.requestStatus.Length; i++)
                        {
                            player.request.requestStatus[i] = requset.requestStatus[i];
                        }

                        isRequest = true;
                    }
                });

                if (isPlayer && isItem && isRequest)
                    isCloud = true;
            }
        });

        if (!isCloud)
            GPGSCloudSavingLoad();
    }

    private void Start()
    {
        if (Application.internetReachability != NetworkReachability.NotReachable)
        {
            StartCoroutine(DownloadTable(URL, 0));
            StartCoroutine(DownloadTable(URL2, 1));
            StartCoroutine(DownloadTable(URL3, 2));
            StartCoroutine(DownloadTable(URL4, 3));
            StartCoroutine(DownloadTable(URL5, 4));
            StartCoroutine(DownloadTable(URL6, 5));
            StartCoroutine(DownloadTable(URL7, 6));
        }

        if (isLogin && !isStart)
        {
            gold = player.user.money;

            for (int i = 0; i < itemTable.items.Length; i++)
            {
                itemTable.items[i].count = player.inven.itemNum[i];
            }

            for (int i = 0; i < requestTable.requests.Length; i++)
            {
                requestTable.requests[i].requestStatus = player.request.requestStatus[i];
            }
        }

        if (isStart)
            TitleClick();
        else
        {
            StartCoroutine(TitleFlicker());
        }
    }

    private void Update()
    {
        delay += Time.deltaTime;

        if (delay >= 180f)
        {
            MarketStock();
            delay = 0;
        }
    }

    IEnumerator DownloadTable(string url, int num)
    {
        UnityWebRequest www = UnityWebRequest.Get(url);
        yield return www.SendWebRequest();
        SetTables(www.downloadHandler.text, num);
    }

    public void TitleClick()
    {
        if (isStart)
        {
            audioSource.clip = lobbyBGM;
            audioSource.Play();
            title.SetActive(false);
            lobbyUI.SetActive(true);
            hud.SetActive(true);
            isStart = true;
        }

        if (titleOn)
        {
            audioSource.clip = lobbyBGM;
            audioSource.Play();
            title.SetActive(false);
            isStart = true;

            if (player.user.isFirst != true)
            {
                lobbyUI.SetActive(true);
                hud.SetActive(true);
                
            }
            else
            {
                lobbyUI.SetActive(false);
                hud.SetActive(false);
                chatUI.SetActive(true);
                chatManager.SetDialog("Prolog");
                player.user.isFirst = false;
            }
        }
        else
        {
            StopCoroutine(TitleFlicker());
            logo.color = new Color(1, 1, 1, 1);
            titleText.SetActive(true);
            titleOn = true;
        }
    }

    IEnumerator TitleFlicker()
    {
        float delay = 0f;
        while (delay < 3f)
        {
            logo.color = new Color(1f, 1f, 1f, delay / 3f);
            delay += Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }

        titleText.SetActive(true);
        titleOn = true;
    }

    //스프레드시트 값을 데이터 테이블로 넣는 메소드
    //tsv는 불러오는 스프레드시트 url,num은 넣을 데이터테이블의 인덱스 번호를 의미한다
    void SetTables(string tsv, int num)
    {
        string[] row = tsv.Split('\n');
        int rowSize = row.Length;
        int columnSize = row[0].Split(',').Length;

        for (int i = 0; i < rowSize; i++)
        {
            string[] column = row[i].Split(',');

            for (int j = 0; j < columnSize; j++)
            {
                //아이템 테이블 연동
                if (num == 0)
                    linkedTable.LinkedTableItem(column, i);
                //동물 능력치 테이블 연동
                else if (num == 1)
                    linkedTable.LinkedTableEnemy(column, i);
                //무기 능력치 테이블 연동
                else if (num == 2)
                    linkedTable.LinkedTableWeapon(column, i);
                //상인 능력치 테이블 연동
                else if (num == 3)
                    linkedTable.LinkedTableMerchant(column, i);
                //퀘스트 테이블 연동
                else if (num == 4)
                    linkedTable.LinkedTableDialog(column, i);
                else if (num == 5)
                {
                    foreach (DialogStruct mLog in mainDialog.dialogs)
                    {
                        if (mLog.dialogIndex == column[0])
                        {
                            ContentStruct content = linkedTable.LinkedTableDialogData(column);
                            mLog.contents[int.Parse(column[1])] = content;
                        }
                    }
                }
                else if (num == 6)
                {
                    print(column[j]);
                    Array.Resize(ref requestTable.requests[i].requestTargets, 5);
                    linkedTable.LinkedTableRequest(column, i);
                }

            }
        }
    }

    public void MarketStock()
    {
        for (int i = 0; i < merchantTable.merchants.Length; i++)
        {
            for (int j = 0; j < merchantTable.merchants[i].merchantItems.Length; j++)
            {
                int num = merchantTable.merchants[i].merchantItems[j].tradeInd;
                itemStruct targetItem = itemTable.items[num];

                int cost = UnityEngine.Random.Range(targetItem.priceMin, targetItem.priceMax);
                merchantTable.merchants[i].merchantItems[j].tradeCost = cost;
            }
        }
    }

    public void UI_OnOff(UI uiType, bool active)
    {
        if (uiType == UI.LobbyUI && lobbyUI.activeInHierarchy != active) { lobbyUI.SetActive(active); }
        if (uiType == UI.Hud && hud.activeInHierarchy != active) { hud.SetActive(active); }
        if (uiType == UI.Explore && exploreUI.activeInHierarchy != active) { exploreUI.SetActive(active); }
        if (uiType == UI.Deal && dealUI.activeInHierarchy != active) { dealUI.SetActive(active); }
        if (uiType == UI.Equip && equipUI.activeInHierarchy != active) { equipUI.SetActive(active); }
        if (uiType == UI.Storage && storageUI.activeInHierarchy != active) { storageUI.SetActive(active); }
        if (uiType == UI.Request && requestUI.activeInHierarchy != active) { requestUI.SetActive(active); }
        if (uiType == UI.Ship && shipUI.activeInHierarchy != active) { shipUI.SetActive(active); ship.SetActive(active); }
        if (uiType == UI.Chat && chatUI.activeInHierarchy != active) { chatUI.SetActive(active); }
        if (uiType == UI.PlanetInfo && planetInfoUI.activeInHierarchy != active) { planetInfoUI.SetActive(active); }
        if (uiType == UI.Planet && planet.activeInHierarchy != active) { planet.SetActive(active); }
    }
}
