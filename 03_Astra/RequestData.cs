using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * �Ƿ� ���̺� ��ũ��Ʈ
 * �ϴ� ���� �ʿ��� ������ ���ַ� ����� ���� ������
 * �ý��� �����ϸ鼭 �ʿ��Ѱ� ������ ���ߵ��ؼ� �߰��Ұ�
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
    public RequestBigType bigType; //���� or ���� ����
    public RequestNormalType normalType; //�Ϲ� or �ݺ� ����
    public RequestSmallType smallType; //���� or ���� �� ����
    public RequestDifficulty requestDifficulty; //�Ƿ� ���̵� ����
    public RequestStatus requestStatus; //�Ƿ� ��Ȳ ����

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