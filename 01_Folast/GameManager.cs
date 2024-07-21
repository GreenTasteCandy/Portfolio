using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public TowerSpawner towerSpawner;
    public EnemySpawn enemySpawner;
    public WaveSystem waveSystem;
    public ItemSpawn itemSpawn;
    public PlayerGold playerGold;
    public PlayerHP playerHP;

    private void Awake()
    {
        instance = this;
    }
}
