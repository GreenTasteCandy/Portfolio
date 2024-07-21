using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class QuestManager : MonoBehaviour
{
    //스크립트 싱글톤 선언
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

    public void ResetDailyQuest() //일일,주간 퀘스트 초기화 메소드
    {
        bool isCheck = false;

        DateTime nextResetTime = DateTime.MinValue; //초기화 시간
        DateTime currentTime = DateTime.UtcNow; //현재 시간

        //일일퀘스트 시간 불러오기
        DateTime checkTime = DateTime.ParseExact(UserData.ins.data.timeUTC, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
        //주간퀘스트 시간 불러오기
        DateTime checkWeekly = DateTime.ParseExact(UserData.ins.data.weeklyUTC, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);

        //일일퀘스트 시간 초기화
        if (currentTime.Hour >= 4) //현재 시간이 오전 4시 이후일경우
        {
            nextResetTime = currentTime.Date.AddDays(1).AddHours(4);
        }
        else //이전일 경우
        {
            nextResetTime = currentTime.Date.AddHours(4);
        }

        if (DateTime.UtcNow > checkTime) //현재 시간이 초기화 시간 이후일경우
        {
            for (int i = 0; i < DataTable.ins.questList.Count; i++)
            {
                if (DataTable.ins.questList[i].type == QuestType.Daily)
                {
                    UserData.ins.data.questPrograss[i] = 0;
                }
            }

            UserData.ins.data.timeUTC = nextResetTime.ToString("yyyy-MM-dd HH:mm:ss"); //새로운 일일퀘스트 초기화 시간 설정
            isCheck = true;
        }

        //주간 퀘스트 시간 초기화
        //초기화 기준은 월요일 오전 0시
        if (currentTime.DayOfWeek == DayOfWeek.Monday && currentTime.Hour >= 4)
        {
            nextResetTime = currentTime.AddDays(7).Date.AddHours(4); //새로운 주간초기화 시간 설정

            if (DateTime.UtcNow > checkWeekly)
            {
                for (int i = 0; i < DataTable.ins.questList.Count; i++)
                {
                    UserData.ins.data.questPrograss[i] = 0;
                }

                UserData.ins.data.isQuestComplete = 0; //퀘스트 전체 진행률 초기화
                UserData.ins.data.weeklyUTC = nextResetTime.ToString("yyyy-MM-dd HH:mm:ss"); //새로운 주간퀘스트 초기화 시간 설정
                isCheck = true;
            }
        }

        if (isCheck == true)
        {
            FirestoreManager.ins.NewFieldMerge();
        }

    }

    public void QuestComplete() //퀘스트 완료 체크 메소드
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

    public void QuestPrograss(int ind) //퀘스트 진행 메소드
    {
        for (int i = 0; i < DataTable.ins.questList.Count; i++)
        {
            if (UserData.ins.data.questPrograss[i] != -1)
            {
                if (DataTable.ins.questList[i].qCase == QuestCase.Play)
                {
                    UserData.ins.data.questPrograss[i] += 1;
                    Debug.Log("퀘스트 : 플레이 횟수");
                }

                else if (DataTable.ins.questList[i].qCase == QuestCase.Scores)
                {
                    UserData.ins.data.questPrograss[i] += ind;
                    Debug.Log("퀘스트 : 누적 점수");
                }

                else if (DataTable.ins.questList[i].qCase == QuestCase.Highscore)
                {
                    if (ind > UserData.ins.data.questPrograss[i])
                        UserData.ins.data.questPrograss[i] = ind;

                    Debug.Log("퀘스트 : 최고 점수");
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