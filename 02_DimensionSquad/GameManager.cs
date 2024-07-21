using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using Cinemachine;

public class GameManager : MonoBehaviourPunCallbacks
{
    public static GameManager ins;

    [Header("# Game Systems")]
    public PhotonView photonView;
    public FloatingJoystick joystickMove;
    public FixedJoystick joystickAttack;
    public PartyMemberList partyMemberList;
    public Slider expBar;

    [Header("# Player Systems")]
    public Transform[] playerSum;
    public PlayerStatus[] charStatus;
    public GameObject player;
    public PlayerStatus playerStatusNull;

    [Header("# Enemy Spawn")]
    public Transform[] enemySpanwer;

    [Header("# Item Select")]
    public ItemData itemTable;
    public Image[] itemIcon;
    public TextMeshProUGUI[] itemName;
    public TextMeshProUGUI[] itemNotice;

    [Header("# HUD")]
    public FloatingJoystick joystick;
    public TextMeshProUGUI skillText;
    public GameObject selectItem;
    public float exp;
    public TextMeshProUGUI timerText;
    public GameObject mainUI;
    public GameObject deadUI;
    public GameObject clearUI;
    public GameObject OptionUI;
    public TextMeshProUGUI enemyNum;

    int num;
    public float maxExp;
    public int lv = 0;

    public bool[] isBuffOn;

    int itemSelect;
    float beforeExp;

    int playerChar;
    int playerNum;

    bool isBoss;
    bool isSkillAOn = true;
    bool isSkillUOn = false;
    bool isSpawn;
    bool selectOn;
    float timer = 0;
    float spawnTime = 10f;

    List<ItemStatus> itemList = new List<ItemStatus>();
    GameObject[] players;

    CinemachineVirtualCamera cm;
    int cmNum;
    
    private void Awake()
    {
        ins = this;
    }

    private void Start()
    {
        num = PhotonNetwork.LocalPlayer.ActorNumber - 1;
        player = PhotonNetwork.Instantiate("Player", playerSum[PhotonNetwork.LocalPlayer.ActorNumber - 1].position, Quaternion.identity, 0);

        Hashtable playerCP = PhotonNetwork.LocalPlayer.CustomProperties;

        playerChar = (int)playerCP["CharType"];
        playerNum = (int)playerCP["CharNum"];

        maxExp = PhotonNetwork.PlayerList.Length * 5;

        player.GetComponent<PlayerSystem>().Setup(playerChar, playerNum, partyMemberList.memberHP[PhotonNetwork.LocalPlayer.ActorNumber - 1]);

        for (int i = 0; i < itemTable.item.Length; i++)
        {
            itemList.Add(itemTable.item[i]);
        }

        itemList = SuffleList(itemList);

        if (PhotonNetwork.IsMasterClient)
        {
            isSpawn = true;
            StartCoroutine(EnemySpawn());
        }

        players = GameObject.FindGameObjectsWithTag("Player");
        cm = GameObject.Find("CMcamera").GetComponent<CinemachineVirtualCamera>();

        StartCoroutine(StartTimer());
    }

    private void Update()
    {
        GameObject[] playersArray = GameObject.FindGameObjectsWithTag("Enemy");

        enemyNum.text = string.Format("{0:n0}", playersArray.Length);
        expBar.value = exp / maxExp;

        if (exp >= maxExp)
        {
            lv += 1;
            itemSelect += 1;

            for (int i = 0; i < 3; i++)
            {
                itemIcon[i].sprite = itemList[lv - itemSelect + i].icon;
                itemName[i].text = itemList[lv - itemSelect + i].name;
                itemNotice[i].text = itemList[lv - itemSelect + i].notice;
            }

            selectItem.SetActive(true);
            exp = 0f;
        }

    }

    IEnumerator EnemySpawn()
    {
        float spawn = 0;

        while (isSpawn)
        {
            spawn += Time.deltaTime;

            if (spawn >= spawnTime)
            {
                for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
                {
                    int rand = Random.Range(0, 3);

                    GameObject enemy = null;

                    if (rand == 0)
                        enemy = PhotonNetwork.Instantiate("Enemy", enemySpanwer[i].position, Quaternion.identity);
                    else if (rand == 1)
                        enemy = PhotonNetwork.Instantiate("Enemy02", enemySpanwer[i].position, Quaternion.identity);
                    else if (rand == 2)
                        enemy = PhotonNetwork.Instantiate("Enemy03", enemySpanwer[i].position, Quaternion.identity);

                    if (isBoss == false && timer >= 300f)
                    {
                        int ran = Random.Range(0, 4);
                        GameObject boss = PhotonNetwork.Instantiate("EnemyBoss", enemySpanwer[ran].position, Quaternion.identity);
                        PhotonView bossPV = boss.GetComponent<PhotonView>();
                        bossPV.TransferOwnership(PhotonNetwork.MasterClient);

                        isBoss = true;
                    }

                    PhotonView enemyPV = enemy.GetComponent<PhotonView>();
                    enemyPV.TransferOwnership(PhotonNetwork.MasterClient);
                }

                spawnTime -= 0.1f;
                spawn = 0;
            }

            yield return null;
        }
    }


    IEnumerator StartTimer()
    {
        while (timer < 600f)
        {
            timer += Time.deltaTime;

            float remainTime = 600f - timer;
            int min = Mathf.FloorToInt(remainTime / 60);
            int sec = Mathf.FloorToInt(remainTime % 60);
            timerText.text = string.Format("{0:D2}:{1:D2}", min, sec);

            yield return null;
        }

        clearUI.SetActive(true);
        mainUI.SetActive(false);
        player.GetComponent<PlayerSystem>().isLive = false;
    }

    public void ExpbarGauge(float getExp)
    {
        photonView.RPC("ExpGet", RpcTarget.AllBuffered, getExp);
    }

    List<itemStatus> SuffleList<itemStatus>(List<itemStatus> list)
    {
        int rand1, rand2;
        itemStatus temp;

        for (int i = 0; i < list.Count; i++)
        {
            rand1 = Random.Range(0, list.Count);
            rand2 = Random.Range(0, list.Count);

            temp = list[rand1];
            list[rand1] = list[rand2];
            list[rand2] = temp;
        }

        return list;
    }

    [PunRPC]
    void ExpGet(float value)
    {
        exp += value;
    }

    public void SelectItem(int num)
    {
        PlayerSystem playerStat = player.GetComponent<PlayerSystem>();
        playerStat.AddItem(itemList[lv - itemSelect + num].addStatus);

        itemSelect -= 1;

        for (int i = 0; i < 3; i++)
        {
            itemIcon[i].sprite = itemList[lv - itemSelect + i].icon;
            itemName[i].text = itemList[lv - itemSelect + i].name;
            itemNotice[i].text = itemList[lv - itemSelect + i].notice;
        }

        selectItem.SetActive(itemSelect > 0);
    }

    public void ActiveSkill()
    {
        if (isSkillAOn)
        {
            SkillData skillData = player.GetComponent<PlayerSystem>().UseSkillActive();
            PlayerSystem playerSystem = player.GetComponent<PlayerSystem>();

            if (skillData.status[1].activeType == SkillActive.Buff)
            {
                for (int i = 0; i < skillData.status[1].skillPoint.Length; i++)
                {
                    if (skillData.status[1].skillPoint[i] > 0)
                    {
                        isBuffOn[i] = true;
                        StartCoroutine(BuffTime(skillData.status[1].range - playerSystem.playerStatus.charStat[0].skillRate, i));
                    }
                }
            }

            StartCoroutine(SkillCoolTime(skillData.status[1].rate - playerSystem.playerStatus.charStat[0].skillRate));
            isSkillAOn = false;
        }
    }

    IEnumerator BuffTime(float rate, int num)
    {
        float timeset = rate;
        float cooltime = 0;

        while (cooltime < timeset)
        {
            cooltime += Time.deltaTime;

            yield return null;
        }

        isBuffOn[num] = false;
    }

    IEnumerator SkillCoolTime(float rate)
    {
        float timeset = rate;
        float cooltime = 0;

        while (cooltime < timeset)
        {
            cooltime += Time.deltaTime;
            skillText.text = string.Format("{0:n0}", rate - cooltime);

            yield return null;
        }

        SkillData skillData = player.GetComponent<PlayerSystem>().playerStatus.charStat[0].skillSet;
        if (skillData.status[0].name == "Àå³­³¢ ¸¹Àº ¼Ò³à" && !skillData.status[0].isUse)
            skillData.status[0].isUse = true;

        skillText.text = "";
        isSkillAOn = true;
    }

    public void CameraMove()
    {
        cmNum += 1;
        if (cmNum >= PhotonNetwork.PlayerList.Length)
            cmNum = 0;

        cm.Follow = players[cmNum].transform;
        cm.LookAt = players[cmNum].transform;
    }

    public void OnOption()
    {
        OptionUI.SetActive(true);
        mainUI.SetActive(false);
    }

    public void ReturnGame()
    {
        OptionUI.SetActive(false);
        mainUI.SetActive(true);
    }

    public void ExitGame()
    {
        player.GetComponent<PlayerSystem>().playerStatus = playerStatusNull;

        PhotonNetwork.LoadLevel("MainLobby");
    }

}
