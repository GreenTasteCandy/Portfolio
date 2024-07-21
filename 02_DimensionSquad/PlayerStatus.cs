using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType { melee = 0, range, heal }
public enum CharType { Dealer = 0, Tanker, Healer, Special }

[CreateAssetMenu]
public class PlayerStatus : ScriptableObject
{
    public Sprite typeIcon;
    public Status[] charStat;
}

[System.Serializable]
public class Status
{
    [Header("# Status")]
    public string name;
    public CharType type;
    public float hp;
    public float maxhp;
    public float mp;
    public float armor;
    public float atk;
    public float def;
    public Sprite charImage;

    [Header("# Attack Setting")]
    public WeaponType atkType;
    public GameObject atkObj;
    public AudioClip atkSE;
    public float atkRate;
    public float atkRange;
    public float atkWidth;
    public float speed;
    public float shotSpeed;
    public int atkNum;

    [Header("# SkillSetting")]
    public int[] shotNum;
    public float[] auraDmg;
    public float skillRate;
    public SkillData skillSet;
}