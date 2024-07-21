using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * 타워의 테이블 값을 입력받는 템플릿을 설정하는 스크립트
 */
public enum UnitTypeData { Unit = 0, Enemy, Hero, Object, Boss }
public enum AttackType { Melee = 0, Range, Support }
public enum EquipType { Defender = 0, Attacker, Healer }

[CreateAssetMenu]
public class TowerTemplate : ScriptableObject
{
    public GameObject towerPrefab;
    public GameObject followTowerPrefab;
    public Weapon[] weapon;
    public string nameTag;
    public int towerLevel;
    public int towerNum; //그 타워가 설치된 수
    public UnitTypeData unitTypeData; //유닛인지 영웅인지 적인지 확인
    public AttackType attackType;
    public EquipType equipType;
    public Sprite typeIcon;
    public string notice;

    [System.Serializable]
    public struct Weapon
    {
        public Sprite sprite; 
        public float maxHp; 
        public float damage; 
        public float rate; //공격속도,낮을수록 빠름
        public float range; 
        public float speed; //이동속도
        public float buff; //공격력 증가율 (0.5 = +50%)
        public float cooldown; //영웅 재배치 쿨타임
        public float skillCooltime; //영웅 스킬 쿨타임
        public float skillDelay; //영웅 스킬 지속시간
        public int cost;
        public int sell;
    }

    public TowerTemplate(int num)
    {
        weapon = new Weapon[num];
    }

}
