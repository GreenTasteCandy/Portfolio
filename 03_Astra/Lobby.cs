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

    /*수행 가능한 의뢰가 있거나 완료된 의뢰가 있으면 버튼 오른쪽 위에 아이콘 표시*/
    public void CheckRequestStatus()
    {
        bool areThereMainPerformableRequests = false; //수행 가능한 메인 의뢰가 있는가
        bool areThereSubPerformableRequests = false; // 수행 가능한 서브 의뢰가 있는가
        bool areThereCompleteableRequests = false; //완료 가능한 의뢰가 있는가

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

        if (areThereCompleteableRequests == true) //완료 가능한 의뢰가 있다면 초록색
        {
            requestStatus.SetActive(true);
            requestStatus.GetComponent<Image>().color = Color.green;
        }
        else if (areThereMainPerformableRequests == true) //수행 가능한 메인 의뢰가 있다면 빨간색
        {
            requestStatus.SetActive(true);
            requestStatus.GetComponent<Image>().color = Color.red;
        }
        else if (areThereSubPerformableRequests == true) //수행 가능한 서브 의뢰가 있다면 노란색
        {
            requestStatus.SetActive(true);
            requestStatus.GetComponent<Image>().color = Color.yellow;
        }
        else //그 외 경우엔 아이콘을 표시하지 않음
        {
            requestStatus.SetActive(false);
        }    
    }
}
