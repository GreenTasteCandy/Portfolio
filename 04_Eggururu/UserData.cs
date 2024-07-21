using Firebase.Firestore;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserData
{
    //½ºÅ©¸³Æ® ½Ì±ÛÅæ ¼±¾ð
    private static UserData Ins = null;
    public static UserData ins
    {
        get
        {
            if (Ins == null)
            {
                Ins = new UserData();
            }

            return Ins;
        }
    }

    public User data = new User
    {
        nickName = FirebaseAuthManager.ins.userID,
        gold = 30000,
        cash = 100,
        highScore = 0,
        highScore2 = 0,
        playTime = 0,
        skin = new Dictionary<string, int>() { { "´Þ°¿", 0 } },
        skinNum = 0,
        skinNow = "´Þ°¿",
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
}


[FirestoreData]
public class User
{
    [FirestoreProperty]
    public string nickName { get; set; }
    [FirestoreProperty]
    public int gold { get; set; }
    [FirestoreProperty]
    public int cash { get; set; }
    [FirestoreProperty]
    public int highScore { get; set; }
    [FirestoreProperty]
    public int highScore2 { get; set; }
    [FirestoreProperty]
    public int playTime { get; set; }
    [FirestoreProperty]
    public string timeUTC { get; set; }
    [FirestoreProperty]
    public string weeklyUTC { get; set; }
    [FirestoreProperty]
    public int isQuestComplete { get; set; }
    [FirestoreProperty]
    public int skinNum { get; set; }
    [FirestoreProperty]
    public string skinNow { get; set; }
    [FirestoreProperty]
    public Dictionary<string, int> skin { get; set; }
    [FirestoreProperty]
    public Dictionary<string, int> personalRank { get; set; }
    [FirestoreProperty]
    public float bgmVolume { get; set; }
    [FirestoreProperty]
    public float sfxVolume { get; set; }
    [FirestoreProperty]
    public int frameRate { get; set; }
    [FirestoreProperty]
    public List<int> questPrograss { get; set; }
    [FirestoreProperty]
    public int abilityNow { get; set; }
    [FirestoreProperty]
    public int abilityStack { get; set; }
    [FirestoreProperty]
    public int ability2Stack { get; set; }
    [FirestoreProperty]
    public int ability2spStack { get; set; }
    [FirestoreProperty]
    public List<int> birdHair { get; set; }
    [FirestoreProperty]
    public List<int> birdFace { get; set; }
    [FirestoreProperty]
    public List<int> birdShoes { get; set; }
    [FirestoreProperty]
    public int birdHairNow { get; set; }
    [FirestoreProperty]
    public int birdFaceNow { get; set; }
    [FirestoreProperty]
    public int birdShoesNow { get; set; }
}