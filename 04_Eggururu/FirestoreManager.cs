using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Extensions;
using Firebase.Firestore;
using System.Linq;
using System.Runtime.CompilerServices;

/*
 * Firestore�� ����ϴ� ��ũ��Ʈ
 * ���� �����͸� ����/�ҷ����� �뵵�� ���ȴ�
 */

public class FirestoreManager
{
    //��ũ��Ʈ �̱��� ����
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

    //firestore �ʱ�ȭ
    public void Init()
    {
        db = FirebaseFirestore.DefaultInstance;
        LoadData();
    }

    //���� ���� ������ ���� ����
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
            skin = new Dictionary<string, int>() { { "�ް�", 0 } },
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
            { "name", FirebaseAuthManager.ins.userID }, //���� �г���
            { "gold", 100 }, //����
            { "cash", 0 }, //����
            { "highScore", 0 }, //���� �ְ� ���
            { "playTime", 1 }, //�� �÷��� Ƚ��
            { "activeSkin", 0}, //���� �������� �� ��Ų
            { "skins", new List<int>() }, //������ �� ��Ų
            { "personalRank", new List<int>() }, //�޼��� ���� ���� ���
            { "timeUTC", DateTime.UtcNow.ToString() }, //������ �ð�
            { "weekQuest", 0 } //�ְ� ����Ʈ �޼���
        };*/
        docRef.SetAsync(user, SetOptions.MergeAll).ContinueWithOnMainThread(task => {
            Debug.Log("Added data to the alovelace document in the users collection.");
        });
    }

    //�ʵ尪 ���� �� �߰��� ���� �� ���� �����Ϳ� �����ִ� �޼ҵ�
    public void NewFieldMerge()
    {
        if (FirebaseAuthManager.ins.isLogin == false)
            return;

        DocumentReference docRef = db.Collection("users").Document(FirebaseAuthManager.ins.GetUserID());
        
        docRef.SetAsync(UserData.ins.data, SetOptions.MergeAll);
    }

    //���� ������ �ҷ����� �޼ҵ�
    public void LoadData()
    {
        if (FirebaseAuthManager.ins.isLogin == false)
            return;

        FirebaseAuthManager.ins.logData = "���� �����͸� �ҷ�������....";
        isStart = false;

        DocumentReference docRef = db.Collection("users").Document(FirebaseAuthManager.ins.GetUserID());
        docRef.GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            var snapshot = task.Result;
            if (snapshot.Exists) //���� ���� �����Ͱ� �ִ��� üũ
            {
                Debug.Log(String.Format("User: {0}", snapshot.Id));
                User user = new User();
                user = snapshot.ConvertTo<User>();

                UserData.ins.data = user;
                isStart = true;

                NewFieldMerge();
            }
            else //���� �����Ͱ� ���� �� �ٽ� ���� �� ����
            {
                SaveData();
            }
        });

    }

    public void UpdateField(string keys, object value) //������ �ʵ尪�� ������Ʈ �ϴ� �޼ҵ�
    {
        if (FirebaseAuthManager.ins.isLogin == false)
            return;

        DocumentReference docRef = db.Collection("users").Document(FirebaseAuthManager.ins.GetUserID());

        docRef.UpdateAsync(keys, value)
        .ContinueWithOnMainThread(task => {
            if (task.IsCompleted)
            {
                Debug.Log(keys + "added successfully!");
                // ���⿡ �߰��� ó���� ������ ���� �� �־�.
            }
            else if (task.IsFaulted)
            {
                Debug.LogError("Failed to add gold: " + task.Exception);
            }
        });
    }

    public void AddGold(string check, int amount) //������ �ʵ尪�� �������ִ� �޼ҵ�
    {
        if (FirebaseAuthManager.ins.isLogin == false)
            return;

        DocumentReference docRef = db.Collection("users").Document(FirebaseAuthManager.ins.GetUserID());

        docRef.UpdateAsync(check, FieldValue.Increment(amount))
        .ContinueWithOnMainThread(task => {
            if (task.IsCompleted)
            {
                Debug.Log(check + "added successfully!");
                // ���⿡ �߰��� ó���� ������ ���� �� �־�.
            }
            else if (task.IsFaulted)
            {
                Debug.LogError("Failed to add gold: " + task.Exception);
            }
        });
    }

    public void BuyItems(string check, object point) //����Ʈ �������� ���� �߰����ִ� �޼ҵ�
    {
        if (FirebaseAuthManager.ins.isLogin == false)
            return;

        DocumentReference docRef = db.Collection("users").Document(FirebaseAuthManager.ins.GetUserID());

        docRef.UpdateAsync(check, FieldValue.ArrayUnion(point))
        .ContinueWithOnMainThread(task => {
            if (task.IsCompleted)
            {
                Debug.Log(check + "added successfully!");
                // ���⿡ �߰��� ó���� ������ ���� �� �־�.
            }
            else if (task.IsFaulted)
            {
                Debug.LogError("Failed to add gold: " + task.Exception);
            }
        });

    }
}
