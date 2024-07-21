using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PositionY { A = 0,B,C,D,E,F,G,H,I,J }

[CreateAssetMenu]
public class OffenceTemplate : ScriptableObject
{
    public int maxEnemyCount;
    public Offence[] field;

    [System.Serializable]
    public struct Offence
    {
        public GameObject enemyPrefab;
        public int[] posX;
        public PositionY[] posY;
    }
}
