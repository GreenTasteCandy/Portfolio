using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PathSet { A = 0, B, C, D, E, F, Rand }
[CreateAssetMenu]
public class DefenceTemplate : ScriptableObject
{
    public int maxEnemyCount;
    public Defence[] field;

    [System.Serializable]
    public struct Defence
    {
        public float spawnTime;
        public float spawnDelay;
        public int spawnCount;
        public GameObject enemyPrefab;
        public PathSet[] path;
    }
}
