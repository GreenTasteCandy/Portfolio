using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * 의뢰 테이블 스크립트
 * 일단 대충 필요한 변수들 위주로 만들어 놓긴 했으나
 * 시스템 제작하면서 필요한게 있으면 알잘딱해서 추가할것
 */
public enum RequestBigType { Main = 0, Sub }
public enum RequestNormalType { Normal = 0, Repeat }
public enum RequestSmallType { Collect = 0, Hunt, Talk, Complex, Tutorial }
public enum RequestDifficulty { Easy = 0, Medium, Hard }
public enum AppearingPlanet { None = 0, Meter, Def, Cocytus }
public enum RequestStatus { Performable = 0, Progressing, Completeable, Completed, Unperformable }
public enum RequestTargetType { Collect = 0, Hunt, Talk, None }
public enum RequestTargetStatus { Progressing = 0, Completed }

[CreateAssetMenu]
public class RequestData : ScriptableObject
{
    public RequestStruct[] requests;
}

[System.Serializable]
public class RequestStruct
{
    public string requestIndex;
    public string requestName;
    public string clientName;
    public int requestOpenLevel;
    [TextArea]
    public string desc;
    public bool Checked = false;

    [Header("Enum")]
    public RequestBigType bigType; //메인 or 서브 구분
    public RequestNormalType normalType; //일반 or 반복 구분
    public RequestSmallType smallType; //수집 or 전투 등 구분
    public RequestDifficulty requestDifficulty; //의뢰 난이도 구분
    public RequestStatus requestStatus; //의뢰 상황 구분

    [Header("Target")]
    public RequestTargetStruct[] requestTargets;

    [Header("Reward")]
    public int rewardGold;
    public string openRequestIndex;
    public RequestRewardItemStruct[] rewardItems;
}

[System.Serializable]
public class RequestTargetStruct
{
    public RequestTargetType targetType;
    public RequestTargetStatus targetStatus;
    public int targetItemIndex;
    public int targetItemCountGoal;
    public int targetEnemyIndex;
    public int targetEnemyCount;
    public int targetEnemyCountGoal;

    public RequestTargetStruct(RequestTargetType type, int ind, int count)
    {
        targetType = type;

        if (type == RequestTargetType.Collect)
        {
            targetItemIndex = ind;
            targetItemCountGoal = count;
        }
        else
        {
            targetEnemyIndex = ind;
            targetEnemyCountGoal = count;
        }
    }
}

[System.Serializable]
public class RequestRewardItemStruct
{
    public int rewardItemIndex;
    public int rewardItemCount;
}