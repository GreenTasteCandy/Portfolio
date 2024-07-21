using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * 무기 데이터 테이블
 */
public enum WeaponType { Melee = 0, Range }
public enum SkillType { Attack = 0, Buff, Passive }

[CreateAssetMenu]
public class WeaponData : ScriptableObject
{
    public weaponStruct[] weapon;
}

[System.Serializable]
public class weaponStruct
{
    [Header("Main")]
    public int ind;
    public string name;
    public Sprite icon;
    [TextArea]
    public string desc;

    [Header("Status")]
    public WeaponType type;
    public int level;
    public int count;
    public float damage;
    public float range;
    public float rate;

    [Header("Upgrade status")]
    public int upgradeCost;
    public float dmgIncrease;
    public float rangeIncreace;
    public float rateIncrease;

    [Header("Skill Status")]
    public int skillObj;
    public SkillType skillType;
    public float skillDmg;
    public float skillCooldown;
    public float skillCost;

    public float DMG => dmgIncrease * GameSetting.ins.player.user.weaponLv[ind];
}
