using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Extensions;
using Firebase.Firestore;
using System.Linq;
using System.Runtime.CompilerServices;

/*
 * Firestore를 사용하는 스크립트
 * 유저 데이터를 저장/불러오는 용도로 사용된다
 */

public class FirestoreManager
{
    //스크립트 싱글톤 선언
    private static FirestoreManager Ins = null;
    public static FirestoreManager ins
    {
        get
        {
            if (Ins == null)
            {
                Ins = new FirestoreManager();
            }

            return Ins;
        }
    }

    public string logData;
    public bool isStart;
    private FirebaseFirestore db;

    //firestore 초기화
    public void Init()
    {
        db = FirebaseFirestore.DefaultInstance;
        LoadData();
    }

    //유저 저장 데이터 최초 생성
    public void SaveData()
    {
        if (FirebaseAuthManager.ins.isLogin == false)
            return;

        DocumentReference docRef = db.Collection("users").Document(FirebaseAuthManager.ins.GetUserID());
        User user = new User
        {
            nickName = FirebaseAuthManager.ins.userID,
            gold = 100,
            cash = 0,
            highScore = 0,
            highScore2 = 0,
            playTime = 0,
            skin = new Dictionary<string, int>() { { "달걀", 0 } },
            skinNum = 0,
            timeUTC = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"),
            weeklyUTC = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"),
            isQuestComplete = 0,
            personalRank = new Dictionary<string, int>(),
            bgmVolume = 1f,
            sfxVolume = 1f,
            frameRate = 60,
            questPrograss = new List<int> { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            abilityNow = 0,
            abilityStack = 0,
            ability2Stack = 0,
            ability2spStack = 0,
            birdHair = new List<int> { 0 },
            birdFace = new List<int> { 0 },
            birdShoes = new List<int> { 0 },
            birdHairNow = 0,
            birdFaceNow = 0,
            birdShoesNow = 0
        };
        /*Dictionary<string, object> user = new Dictionary<string, object>
        {
            { "name", FirebaseAuthManager.ins.userID }, //유저 닉네임
            { "gold", 100 }, //깃털
            { "cash", 0 }, //와플
            { "highScore", 0 }, //유저 최고 기록
            { "playTime", 1 }, //총 플레이 횟수
            { "activeSkin", 0}, //현재 적용중인 알 스킨
            { "skins", new List<int>() }, //구매한 알 스킨
            { "personalRank", new List<int>() }, //달성한 유저 개인 기록
            { "timeUTC", DateTime.UtcNow.ToString() }, //접속한 시간
            { "weekQuest", 0 } //주간 퀘스트 달성률
        };*/
        docRef.SetAsync(user, SetOptions.MergeAll).ContinueWithOnMainThread(task => {
            Debug.Log("Added data to the alovelace document in the users collection.");
        });
    }

    //필드값 없을 시 추가로 생성 후 기존 데이터와 합쳐주는 메소드
    public void NewFieldMerge()
    {
        if (FirebaseAuthManager.ins.isLogin == false)
            return;

        DocumentReference docRef = db.Collection("users").Document(FirebaseAuthManager.ins.GetUserID());
        
        docRef.SetAsync(UserData.ins.data, SetOptions.MergeAll);
    }

    //유저 데이터 불러오는 메소드
    public void LoadData()
    {
        if (FirebaseAuthManager.ins.isLogin == false)
            return;

        FirebaseAuthManager.ins.logData = "유저 데이터를 불러오는중....";
        isStart = false;

        DocumentReference docRef = db.Collection("users").Document(FirebaseAuthManager.ins.GetUserID());
        docRef.GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            var snapshot = task.Result;
            if (snapshot.Exists) //유저 저장 데이터가 있는지 체크
            {
                Debug.Log(String.Format("User: {0}", snapshot.Id));
                User user = new User();
                user = snapshot.ConvertTo<User>();

                UserData.ins.data = user;
                isStart = true;

                NewFieldMerge();
            }
            else //유저 데이터가 없을 시 다시 생성 후 저장
            {
                SaveData();
            }
        });

    }

    public void UpdateField(string keys, object value) //데이터 필드값을 업데이트 하는 메소드
    {
        if (FirebaseAuthManager.ins.isLogin == false)
            return;

        DocumentReference docRef = db.Collection("users").Document(FirebaseAuthManager.ins.GetUserID());

        docRef.UpdateAsync(keys, value)
        .ContinueWithOnMainThread(task => {
            if (task.IsCompleted)
            {
                Debug.Log(keys + "added successfully!");
                // 여기에 추가로 처리할 내용을 넣을 수 있어.
            }
            else if (task.IsFaulted)
            {
                Debug.LogError("Failed to add gold: " + task.Exception);
            }
        });
    }

    public void AddGold(string check, int amount) //데이터 필드값을 가감해주는 메소드
    {
        if (FirebaseAuthManager.ins.isLogin == false)
            return;

        DocumentReference docRef = db.Collection("users").Document(FirebaseAuthManager.ins.GetUserID());

        docRef.UpdateAsync(check, FieldValue.Increment(amount))
        .ContinueWithOnMainThread(task => {
            if (task.IsCompleted)
            {
                Debug.Log(check + "added successfully!");
                // 여기에 추가로 처리할 내용을 넣을 수 있어.
            }
            else if (task.IsFaulted)
            {
                Debug.LogError("Failed to add gold: " + task.Exception);
            }
        });
    }

    public void BuyItems(string check, object point) //리스트 데이터의 값을 추가해주는 메소드
    {
        if (FirebaseAuthManager.ins.isLogin == false)
            return;

        DocumentReference docRef = db.Collection("users").Document(FirebaseAuthManager.ins.GetUserID());

        docRef.UpdateAsync(check, FieldValue.ArrayUnion(point))
        .ContinueWithOnMainThread(task => {
            if (task.IsCompleted)
            {
                Debug.Log(check + "added successfully!");
                // 여기에 추가로 처리할 내용을 넣을 수 있어.
            }
            else if (task.IsFaulted)
            {
                Debug.LogError("Failed to add gold: " + task.Exception);
            }
        });

    }
}
