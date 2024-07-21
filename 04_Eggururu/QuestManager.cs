using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class QuestManager : MonoBehaviour
{
    //��ũ��Ʈ �̱��� ����
    private static QuestManager Ins = null;
    public static QuestManager ins
    {
        get
        {
            if (Ins == null)
            {
                Ins = new QuestManager();
            }

            return Ins;
        }
    }

    public void ResetDailyQuest() //����,�ְ� ����Ʈ �ʱ�ȭ �޼ҵ�
    {
        bool isCheck = false;

        DateTime nextResetTime = DateTime.MinValue; //�ʱ�ȭ �ð�
        DateTime currentTime = DateTime.UtcNow; //���� �ð�

        //��������Ʈ �ð� �ҷ�����
        DateTime checkTime = DateTime.ParseExact(UserData.ins.data.timeUTC, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
        //�ְ�����Ʈ �ð� �ҷ�����
        DateTime checkWeekly = DateTime.ParseExact(UserData.ins.data.weeklyUTC, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);

        //��������Ʈ �ð� �ʱ�ȭ
        if (currentTime.Hour >= 4) //���� �ð��� ���� 4�� �����ϰ��
        {
            nextResetTime = currentTime.Date.AddDays(1).AddHours(4);
        }
        else //������ ���
        {
            nextResetTime = currentTime.Date.AddHours(4);
        }

        if (DateTime.UtcNow > checkTime) //���� �ð��� �ʱ�ȭ �ð� �����ϰ��
        {
            for (int i = 0; i < DataTable.ins.questList.Count; i++)
            {
                if (DataTable.ins.questList[i].type == QuestType.Daily)
                {
                    UserData.ins.data.questPrograss[i] = 0;
                }
            }

            UserData.ins.data.timeUTC = nextResetTime.ToString("yyyy-MM-dd HH:mm:ss"); //���ο� ��������Ʈ �ʱ�ȭ �ð� ����
            isCheck = true;
        }

        //�ְ� ����Ʈ �ð� �ʱ�ȭ
        //�ʱ�ȭ ������ ������ ���� 0��
        if (currentTime.DayOfWeek == DayOfWeek.Monday && currentTime.Hour >= 4)
        {
            nextResetTime = currentTime.AddDays(7).Date.AddHours(4); //���ο� �ְ��ʱ�ȭ �ð� ����

            if (DateTime.UtcNow > checkWeekly)
            {
                for (int i = 0; i < DataTable.ins.questList.Count; i++)
                {
                    UserData.ins.data.questPrograss[i] = 0;
                }

                UserData.ins.data.isQuestComplete = 0; //����Ʈ ��ü ����� �ʱ�ȭ
                UserData.ins.data.weeklyUTC = nextResetTime.ToString("yyyy-MM-dd HH:mm:ss"); //���ο� �ְ�����Ʈ �ʱ�ȭ �ð� ����
                isCheck = true;
            }
        }

        if (isCheck == true)
        {
            FirestoreManager.ins.NewFieldMerge();
        }

    }

    public void QuestComplete() //����Ʈ �Ϸ� üũ �޼ҵ�
    {
        bool isGain = false;

        int i = 0;
        foreach (QuestData data in DataTable.ins.questList)
        {
            if (UserData.ins.data.questPrograss[i] >= data.cost)
            {
                if (UserData.ins.data.isQuestComplete < 100 && UserData.ins.data.isQuestComplete != -1)
                    UserData.ins.data.isQuestComplete += data.point;

                UserData.ins.data.gold += data.reward;
                UserData.ins.data.questPrograss[i] = -1;
                isGain = true;
            }

            i += 1;
        }

        if (UserData.ins.data.isQuestComplete >= 100)
        {
            UserData.ins.data.isQuestComplete = -1;
            UserData.ins.data.gold += 500;
            UserData.ins.data.cash += 15;
            isGain = true;
        }

        if (isGain == true)
        {
            FirestoreManager.ins.NewFieldMerge();
        }

    }

    IEnumerator DataLoad()
    {
        FirestoreManager.ins.NewFieldMerge();

        yield return null;

        FirestoreManager.ins.LoadData();
    }

    public void QuestPrograss(int ind) //����Ʈ ���� �޼ҵ�
    {
        for (int i = 0; i < DataTable.ins.questList.Count; i++)
        {
            if (UserData.ins.data.questPrograss[i] != -1)
            {
                if (DataTable.ins.questList[i].qCase == QuestCase.Play)
                {
                    UserData.ins.data.questPrograss[i] += 1;
                    Debug.Log("����Ʈ : �÷��� Ƚ��");
                }

                else if (DataTable.ins.questList[i].qCase == QuestCase.Scores)
                {
                    UserData.ins.data.questPrograss[i] += ind;
                    Debug.Log("����Ʈ : ���� ����");
                }

                else if (DataTable.ins.questList[i].qCase == QuestCase.Highscore)
                {
                    if (ind > UserData.ins.data.questPrograss[i])
                        UserData.ins.data.questPrograss[i] = ind;

                    Debug.Log("����Ʈ : �ְ� ����");
                }
            }
        }
    }

}

public enum QuestType { Daily = 0, Weekly }
public enum QuestCase { Play = 0, Scores, Highscore }

[System.Serializable]
public class QuestData
{
    public string name;
    public string docs;
    public QuestType type;
    public int cost;
    public int reward;
    public int point;
    public QuestCase qCase;

    public QuestData(string name, QuestType type, string docs, int cost, int reward, int point, QuestCase qCase)
    {
        this.name = name;
        this.docs = docs;
        this.cost = cost;
        this.reward = reward;
        this.point = point;
        this.type = type;
        this.qCase = qCase;
    }
}