using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    [Header("# Game Version")]
    public TextMeshProUGUI versionText;

    [Header("# Disconnect Screen")]
    public GameObject disconnectPanel;
    public TMP_InputField nickNameInput;

    [Header("# Lobby Screen")]
    public GameObject lobbyPanel;
    public TMP_InputField roomSeedNumber;
    public TextMeshProUGUI nicknameText;

    [Header("# WaitRoom Screen")]
    public GameObject roomPanel;
    public TextMeshProUGUI seedNumber;
    public TextMeshProUGUI[] chatText;
    public TMP_InputField chatInput;
    public GameObject startButton;
    public GameObject readyButton;
    public TextMeshProUGUI readyButtonText;

    [Header("- Room UI")]
    public GameObject partyUI;
    public TextMeshProUGUI[] playerNickNames; 
    public Image[] playerCharImage;

    [Header("- Charactor Select UI")]
    public GameObject[] CharUI;
    public GameObject[] typeSelect;
    public TMP_Dropdown[] dropdownType;
    public PlayerStatus[] charTable;
    public Image[] playerTypeIcon;

    public PhotonView pv;

    bool isReady = false;
    int readyCount = 0;
    int maxPlayerCount = 4;

    int selectType;
    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    private void Start()
    {
        PhotonNetwork.GameVersion = Application.version;
        versionText.text = "Ver. " + Application.version + "  ";
    }

    public void Connect() => PhotonNetwork.ConnectUsingSettings();

    public void Disconnect() => PhotonNetwork.Disconnect();

    public override void OnConnectedToMaster()
    {
        disconnectPanel.SetActive(false);
        lobbyPanel.SetActive(true);
        PhotonNetwork.JoinLobby();

        PhotonNetwork.LocalPlayer.SetCustomProperties(new Hashtable { { "IsReady", false }, { "CharType", 0 }, { "CharNum", 0 } });
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        lobbyPanel.SetActive(false);
        roomPanel.SetActive(false);
    }

    public override void OnJoinedLobby()
    {
        lobbyPanel.SetActive(true);
        roomPanel.SetActive(false);
        PhotonNetwork.LocalPlayer.NickName = nickNameInput.text;
        nicknameText.text = PhotonNetwork.LocalPlayer.NickName;
    }

    public void CreateRoom()
    {
        PhotonNetwork.CreateRoom(Random.Range(11111111, 99999999).ToString(), new RoomOptions { MaxPlayers = 4 });
    }

    public void JoinRandomRoom() => PhotonNetwork.JoinRandomRoom();
    public void JoinSelectRoom() => PhotonNetwork.JoinRoom(roomSeedNumber.text);
    public void LeaveRoom() => PhotonNetwork.LeaveRoom();

    public override void OnJoinedRoom()
    {
        for (int i = 0; i < chatText.Length; i++)
        {
            chatText[i].text = "";
        }

        lobbyPanel.SetActive(false);
        roomPanel.SetActive(true);
        seedNumber.text = "seeds : " + PhotonNetwork.CurrentRoom.Name + " / " + "현재 " + PhotonNetwork.CurrentRoom.PlayerCount + "명 접속중";

        startButton.GetComponent<Button>().interactable = false;

        if (PhotonNetwork.IsMasterClient)
        {
            startButton.SetActive(true);
            readyButton.SetActive(false);
            isReady = true;
            pv.RPC("SyncReadyState", RpcTarget.MasterClient, PhotonNetwork.LocalPlayer.ActorNumber, true);
        }
        else
        {
            startButton.SetActive(false);
            readyButton.SetActive(true);
            isReady = false;
            readyButtonText.text = "게임 준비";
            pv.RPC("SyncReadyState", RpcTarget.AllBuffered, PhotonNetwork.LocalPlayer.ActorNumber, false);
        }

        pv.RPC("PartyMemberName", RpcTarget.All);

        for (int i = 0; i < 4; i++) { typeSelect[i].SetActive(false); }
        typeSelect[PhotonNetwork.LocalPlayer.ActorNumber - 1].SetActive(true);
    }

    public override void OnJoinRandomFailed(short returnCode, string message) { CreateRoom(); }

    public override void OnCreateRoomFailed(short returnCode, string message) { CreateRoom(); }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        seedNumber.text = "seeds : " + PhotonNetwork.CurrentRoom.Name + " / " + "현재 " + PhotonNetwork.CurrentRoom.PlayerCount + "명 접속중";
        pv.RPC("ChatRPC", RpcTarget.All, "<color=yellow>" + newPlayer.NickName + "님이 참가하였습니다.");

        pv.RPC("PartyMemberName", RpcTarget.All);

        for (int i = 0; i < 4; i ++) { typeSelect[i].SetActive(false); }
        typeSelect[PhotonNetwork.LocalPlayer.ActorNumber - 1].SetActive(true);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        seedNumber.text = "seeds : " + PhotonNetwork.CurrentRoom.Name + " / " + "현재 " + PhotonNetwork.CurrentRoom.PlayerCount + "명 접속중";
        pv.RPC("ChatRPC", RpcTarget.All, "<color=yellow>" + otherPlayer.NickName + "님이 퇴장하였습니다.");

        pv.RPC("PartyMemberLeft", RpcTarget.All);

        for (int i = 0; i < 4; i++) { typeSelect[i].SetActive(false); }

        if (PhotonNetwork.IsMasterClient)
        {
            startButton.SetActive(true);
            readyButton.SetActive(false);
        }
        else
        {
            startButton.SetActive(false);
            readyButton.SetActive(true);
        }
    }

    public void OnReadyButtonTouch()
    {
        if (!isReady)
        {
            isReady = true;
            readyButtonText.text = "준비 완료";
            pv.RPC("SyncReadyState", RpcTarget.AllBuffered, PhotonNetwork.LocalPlayer.ActorNumber, true);
        }
        else
        {
            isReady = false;
            readyButtonText.text = "게임 준비";
            pv.RPC("SyncReadyState", RpcTarget.AllBuffered, PhotonNetwork.LocalPlayer.ActorNumber, false);
        }
    }

    public void OnValueChange()
    {
        selectType = dropdownType[PhotonNetwork.LocalPlayer.ActorNumber - 1].value;
    }

    public void OnCharactorSelect(int num)
    {
        if (num == PhotonNetwork.LocalPlayer.ActorNumber - 1)
        {
            partyUI.SetActive(false);
            CharUI[selectType].SetActive(true);
        }
    }

    public void TouchCharactor(int charNum)
    {
        pv.RPC("SelectPlayerCharactor", RpcTarget.All, PhotonNetwork.LocalPlayer.ActorNumber, selectType, charNum);

        partyUI.SetActive(true);
        CharUI[selectType].SetActive(false);
    }

    [PunRPC]
    void PartyMemberName()
    {
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            playerNickNames[player.ActorNumber - 1].text = player.NickName;

            int num1 = (int)player.CustomProperties["CharType"];
            int num2 = (int)player.CustomProperties["CharNum"];
            playerCharImage[player.ActorNumber - 1].color = Color.white;
            playerCharImage[player.ActorNumber - 1].sprite = charTable[num1].charStat[num2].charImage;
            playerTypeIcon[player.ActorNumber - 1].color = Color.white;
            playerTypeIcon[player.ActorNumber - 1].sprite = charTable[num1].typeIcon;
        }
    }

    [PunRPC]
    void PartyMemeberLeft()
    {
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            playerNickNames[player.ActorNumber - 1].text = "";
            playerCharImage[player.ActorNumber - 1].color = new Color(0, 0, 0, 0);
            playerCharImage[player.ActorNumber - 1].sprite = null;
            playerTypeIcon[player.ActorNumber - 1].color = new Color(0, 0, 0, 0);
            playerTypeIcon[player.ActorNumber - 1].sprite = null;
        }
    }

    [PunRPC]
    void SelectPlayerCharactor(int num, int charType, int charNum)
    {

        foreach (Player player in PhotonNetwork.PlayerList)
        {
            if (player.ActorNumber == num)
            {
                player.CustomProperties["CharType"] = charType;
                player.CustomProperties["CharNum"] = charNum;
                playerCharImage[player.ActorNumber - 1].color = Color.white;
                playerCharImage[player.ActorNumber - 1].sprite = charTable[charType].charStat[charNum].charImage;
                playerTypeIcon[player.ActorNumber - 1].color = Color.white;
                playerTypeIcon[player.ActorNumber - 1].sprite = charTable[charType].typeIcon;
                break;
            }
        }

    }

    [PunRPC]
    void SyncReadyState(int actorNumber, bool isReady)
    {
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            if (player.ActorNumber == actorNumber)
            {
                player.CustomProperties["IsReady"] = isReady;
                break;
            }
        }

        CheckReadyCount();
    }

    void CheckReadyCount()
    {
        readyCount = 0;
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            if ((bool)player.CustomProperties["IsReady"])
            {
                int num1 = (int)player.CustomProperties["CharType"];
                int num2 = (int)player.CustomProperties["CharNum"];
                playerCharImage[player.ActorNumber - 1].color = Color.white;
                playerCharImage[player.ActorNumber - 1].sprite = charTable[num1].charStat[num2].charImage;
                playerTypeIcon[player.ActorNumber - 1].color = Color.white;
                playerTypeIcon[player.ActorNumber - 1].sprite = charTable[num1].typeIcon;

                readyCount++;
            }
            else
            {
                playerCharImage[player.ActorNumber - 1].sprite = null;
            }
        }

        if (readyCount == PhotonNetwork.PlayerList.Length && PhotonNetwork.IsMasterClient)
        {
            startButton.SetActive(true);
            startButton.GetComponent<Button>().interactable = true;
        }
        else
        {
            startButton.SetActive(false);
            startButton.GetComponent<Button>().interactable = false;
        }
    }

    public void OnStartButtonTouch()
    {
        pv.RPC("StartGame", RpcTarget.AllBuffered);
    }

    [PunRPC]
    void StartGame()
    {
        PhotonNetwork.LoadLevel("SampleScene");
    }

    public void SendChat()
    {
        string msg = PhotonNetwork.NickName + " : " + chatInput.text;
        pv.RPC("ChatRPC", RpcTarget.All, PhotonNetwork.NickName + " : " + chatInput.text);
        chatInput.text = "";
    }

    [PunRPC]
    void ChatRPC(string msg)
    {
        bool isInput = false;

        for (int i = 0; i < chatText.Length; i++)
        {
            if (chatText[i].text == "")
            {
                isInput = true;
                chatText[i].text = msg;
                break;
            } 
        }
        if (!isInput)
        {
            for (int i = 1; i < chatText.Length; i++)
                chatText[i - 1].text = chatText[i].text;

            chatText[chatText.Length - 1].text = msg;
        }
    }

}
