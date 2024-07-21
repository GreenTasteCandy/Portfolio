using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.UI;

public class PartyMemberList : MonoBehaviourPunCallbacks
{
    public PhotonView pv;
    public GameObject[] memberList;
    public TextMeshProUGUI[] memberName;
    public Slider[] memberHP;
    public TextMeshProUGUI[] memberArmor;

    GameObject[] players;
    PlayerSystem[] playerSys;

    // Start is called before the first frame update
    void Start()
    {
        pv.TransferOwnership(PhotonNetwork.LocalPlayer);

        foreach (Player player in PhotonNetwork.PlayerList)
        {
            if (player == PhotonNetwork.LocalPlayer)
            {
                memberName[player.ActorNumber - 1].color = Color.green;
            }
            else
            {
                memberName[player.ActorNumber - 1].color = Color.blue;
            }

            memberName[player.ActorNumber - 1].text = player.NickName;
            memberList[player.ActorNumber - 1].SetActive(true);
        }

        players = GameObject.FindGameObjectsWithTag("Player");

        for (int i = 0; i < players.Length; i++)
        {
            playerSys[i] = players[i].GetComponent<PlayerSystem>();
        }

    }

    private void LateUpdate()
    {
        pv.RPC("ChangeGaugeValue", RpcTarget.AllBuffered);
        /*
        for (int i = 0; i < players.Length; i++)
        {
            memberHP[i].value = playerSys[i].curHp / playerSys[i].maxhp;
            memberArmor[i].text = string.Format("{0:n0}", playerSys[i].curArmor);
        }*/
    }

    [PunRPC]
    void ChangeGaugeValue()
    {
        int num = PhotonNetwork.LocalPlayer.ActorNumber - 1;
        PlayerSystem playerGauge = GameManager.ins.player.GetComponent<PlayerSystem>();

        memberHP[num].value = playerGauge.curHp / playerGauge.maxhp;
        memberArmor[num].text = string.Format("{0:n0}", playerGauge.curArmor);
    }

}
