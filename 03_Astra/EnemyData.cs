using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class EnemyData : ScriptableObject
{
    public EnemyStruct[] enemyTable;
}

[System.Serializable]
public class EnemyStruct
{
    public string name;
    public EnemyType type;
    public float health;
    public float damage;
    public float speed;
    public float rate;
    public float range;
    public float returnRange;
}