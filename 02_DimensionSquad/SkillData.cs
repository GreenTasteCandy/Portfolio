using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SkillType { Passive = 0, Active, Ultimate }
public enum SkillActive { None = 0, Grenade, Buff }

[CreateAssetMenu]
public class SkillData : ScriptableObject
{
    public SkillStatus[] status;
}

[System.Serializable]
public class SkillStatus
{
    public string name;
    public Sprite image;
    public SkillType type;
    public WeaponType atkType;
    public SkillActive activeType;
    public float dmg;
    public float speed;
    public float atkSpeed;
    public float atkRange;
    public float range;
    public float cost;
    public float rate;
    public bool isUse;
    public AudioClip skillSE;
    public GameObject[] skillobj;
    public int[] skillPoint;
}