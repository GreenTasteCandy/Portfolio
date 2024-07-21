using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Lobby : MonoBehaviour
{
    [Header("Lobby")]
    [SerializeField]
    TextMeshProUGUI textPlayerName;
    [SerializeField]
    TextMeshProUGUI textGold;
    [SerializeField]
    GameObject requestStatus;
    public Request request;

    private void LateUpdate()
    {
        textPlayerName.text = GameSetting.ins.nickName;
        textGold.text = GameSetting.ins.player.Money.ToString();

        if (GameSetting.ins.requestTable.requests[0].requestStatus != RequestStatus.Progressing && GameSetting.ins.requestTable.requests[1].requestStatus != RequestStatus.Progressing && GameSetting.ins.requestTable.requests[2].requestStatus != RequestStatus.Progressing)
        {
            request.CheckTargets();
        }

        CheckRequestStatus();
    }

    /*���� ������ �Ƿڰ� �ְų� �Ϸ�� �Ƿڰ� ������ ��ư ������ ���� ������ ǥ��*/
    public void CheckRequestStatus()
    {
        bool areThereMainPerformableRequests = false; //���� ������ ���� �Ƿڰ� �ִ°�
        bool areThereSubPerformableRequests = false; // ���� ������ ���� �Ƿڰ� �ִ°�
        bool areThereCompleteableRequests = false; //�Ϸ� ������ �Ƿڰ� �ִ°�

        for (int i = 0; i < GameSetting.ins.requestTable.requests.Length; i++)
        {
            if (GameSetting.ins.requestTable.requests[i].requestName != "" && GameSetting.ins.requestTable.requests[i].requestStatus == RequestStatus.Performable && GameSetting.ins.requestTable.requests[i].Checked == false)
            {
                if (GameSetting.ins.requestTable.requests[i].bigType == RequestBigType.Main) { areThereMainPerformableRequests = true; }
                if (GameSetting.ins.requestTable.requests[i].bigType == RequestBigType.Sub) { areThereSubPerformableRequests = true; }
            }
            if (GameSetting.ins.requestTable.requests[i].requestName != "" && GameSetting.ins.requestTable.requests[i].requestStatus == RequestStatus.Completeable)
            {
                areThereCompleteableRequests = true;
            }
        }

        if (areThereCompleteableRequests == true) //�Ϸ� ������ �Ƿڰ� �ִٸ� �ʷϻ�
        {
            requestStatus.SetActive(true);
            requestStatus.GetComponent<Image>().color = Color.green;
        }
        else if (areThereMainPerformableRequests == true) //���� ������ ���� �Ƿڰ� �ִٸ� ������
        {
            requestStatus.SetActive(true);
            requestStatus.GetComponent<Image>().color = Color.red;
        }
        else if (areThereSubPerformableRequests == true) //���� ������ ���� �Ƿڰ� �ִٸ� �����
        {
            requestStatus.SetActive(true);
            requestStatus.GetComponent<Image>().color = Color.yellow;
        }
        else //�� �� ��쿣 �������� ǥ������ ����
        {
            requestStatus.SetActive(false);
        }    
    }
}
