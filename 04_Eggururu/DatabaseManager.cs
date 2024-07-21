using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using System.Threading.Tasks;
using System;
using Firebase.Extensions;

public class DatabaseManager
{
    //��ũ��Ʈ �̱��� ����
    private static DatabaseManager Ins = null;
    public static DatabaseManager ins
    {
        get
        {
            if (Ins == null)
            {
                Ins = new DatabaseManager();
            }

            return Ins;
        }
    }

    private DatabaseReference db;
    public Dictionary<string, int> dbDict = new Dictionary<string, int>();
    public Dictionary<string, int> dbDict2 = new Dictionary<string, int>();

    public void Init()
    {
        Debug.Log("�����ͺ��̽� �ʱ�ȭ");
        db = FirebaseDatabase.DefaultInstance.RootReference;

        LoadRank("totalRank");
        LoadRank("totalRankClassic");
    }

    public void WriteData(int score, string data)
    {
        RankData rank = new RankData(FirebaseAuthManager.ins.userID, score);
        string json = JsonUtility.ToJson(rank);

        db.Child(data).Child(FirebaseAuthManager.ins.GetUserID()).SetRawJsonValueAsync(json);
    }

    public void LoadRank(string data_)
    {
        db.Child(data_).GetValueAsync().ContinueWith(task =>
        {
            if (task.IsCompleted)
            {
                if (data_ == "totalRank")
                    dbDict = new Dictionary<string, int>();
                else if (data_ == "totalRankClassic")
                    dbDict2 = new Dictionary<string, int>();

                DataSnapshot snapshot = task.Result;

                foreach(DataSnapshot data in snapshot.Children)
                {
                    IDictionary rankinfo = (IDictionary)data.Value;
                    Debug.LogFormat("�ҷ��� ������ : nickname {0} highScore {1}", rankinfo["nickname"], rankinfo["highScore"]);
                    if (data_ == "totalRank")
                        dbDict.Add(rankinfo["nickname"].ToString(), int.Parse(rankinfo["highScore"].ToString()));
                    else if (data_ == "totalRankClassic")
                        dbDict2.Add(rankinfo["nickname"].ToString(), int.Parse(rankinfo["highScore"].ToString()));
                }

                Debug.Log("�ҷ����µ� ������");
            }
        });
    }
}

public class RankData
{
    public string nickname;
    public int highScore;
    public string scoreTime;

    public RankData(string nickname, int highScore)
    {
        this.nickname = nickname;
        this.highScore = highScore;
    }   
}
