using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Ÿ���� ���̺� ���� �Է¹޴� ���ø��� �����ϴ� ��ũ��Ʈ
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
    public int towerNum; //�� Ÿ���� ��ġ�� ��
    public UnitTypeData unitTypeData; //�������� �������� ������ Ȯ��
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
        public float rate; //���ݼӵ�,�������� ����
        public float range; 
        public float speed; //�̵��ӵ�
        public float buff; //���ݷ� ������ (0.5 = +50%)
        public float cooldown; //���� ���ġ ��Ÿ��
        public float skillCooltime; //���� ��ų ��Ÿ��
        public float skillDelay; //���� ��ų ���ӽð�
        public int cost;
        public int sell;
    }

    public TowerTemplate(int num)
    {
        weapon = new Weapon[num];
    }

}
