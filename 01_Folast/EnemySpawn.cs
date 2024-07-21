using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/*���� ������Ű�� EnemySpawner�� ���ϰ� �ϴ� ��ũ��Ʈ*/
public class EnemySpawn : MonoBehaviour
{
    [SerializeField]
    GameObject enemyHPSliderPrefab;
    [SerializeField]
    Transform canvasTransform; //ü�� �� ǥ�ø� ����
    [SerializeField]
    int wayPointNum; //�̵� ��� �� (-1 ���־�� ��)
    [SerializeField]
    PlayerHP playerHP;
    [SerializeField]
    PlayerGold playerGold;
    [SerializeField]
    WaveSystem waveSystem;
    [SerializeField]
    TowerSpawner towerSpawner;
    [SerializeField]
    ItemSpawn itemSpawn;
    [SerializeField]
    TowerTemplate[] enemyTemplate;
    [SerializeField]
    TowerTemplate[] enemyStatus;

    MapTemplate.Path[] pathPoints;
    DefenceTemplate currentWave; //���潺 ���� ������
    OffenceTemplate currentWave2; //���潺 ���� ������

    int fieldNum;
    int fieldOffenceNum;

    int currentEnemyCount;
    List<Enemy> enemylist;

    float timeOut = 60.0f;

    public int gamePoint;

    public MapTemplate.Path[] PathPoint => pathPoints;
    public DefenceTemplate CurrentWave => currentWave;
    public OffenceTemplate CurrentWave2 => currentWave2;
    public int FieldNum => fieldNum;
    public int FieldOffenceNum => fieldOffenceNum;
    public int CurrentEnemyCount => currentEnemyCount;
    public int MaxEnemyCount => currentWave.maxEnemyCount;
    public List<Enemy> EnemyList => enemylist;
    public float TimeOut => timeOut;
    public int GamePoint => gamePoint;

    private void Awake()
    {
        for (int i = 0; i < enemyTemplate.Length; i++)
        {
            enemyTemplate[i].weapon[0] = enemyStatus[i].weapon[0];
        }

        enemylist = new List<Enemy>();
        //StartCoroutine("SpawnEnemy");
    }

    public void GameEndEnemyStatus()
    {
        for (int i = 0; i < enemyTemplate.Length; i++)
        {
            enemyTemplate[i].weapon[0] = enemyStatus[i].weapon[0];
        }
    }

    public void StartWave(MapTemplate.Path[] path, DefenceTemplate wave) //���潺 ����
    {
        pathPoints = path;
        currentWave = wave;
        currentEnemyCount = currentWave.maxEnemyCount;
        EnemyStatusUp();

        StartCoroutine("SpawnEnemyDefence");
    }

    public void StartWave2(MapTemplate.Path[] path, OffenceTemplate wave2) //���潺 ����
    {
        towerSpawner.BuildSet(false);

        pathPoints = path;
        currentWave2 = wave2;
        currentEnemyCount = currentWave2.maxEnemyCount;

        fieldOffenceNum = Random.Range(0, 3);

        SpawnEnemy2();
    }

    //�������� ���� �� ĳ���� �ɷ�ġ ����
    public void EnemyStatusUp()
    {
        for(int i = 0; i < enemyTemplate.Length; i++)
        {

            if (enemyTemplate[i].equipType == EquipType.Defender)
                enemyTemplate[i].weapon[0].maxHp += 3;
            else if (enemyTemplate[i].equipType == EquipType.Healer)
                enemyTemplate[i].weapon[0].maxHp += 1.5f;
            else
                enemyTemplate[i].weapon[0].maxHp += 1;

            if (enemyTemplate[i].equipType == EquipType.Attacker)
                enemyTemplate[i].weapon[0].damage += 2;
            else
                enemyTemplate[i].weapon[0].damage += 1;

        }
    }

    private IEnumerator SpawnEnemyDefence()//���潺���� �� ������ �����ð����� �������� ����
    {
        int spawnDefence = 0;

        while(spawnDefence < currentWave.field.Length)
        {
            yield return new WaitForSeconds(currentWave.field[spawnDefence].spawnTime);
            StartCoroutine(SpawnEnemy(spawnDefence));
            spawnDefence++;
        }

        //������ �δ������ ��� ����ɽ� ���潺 ����
        StartCoroutine("DefenseEnd");
    }

    private IEnumerator SpawnEnemy(int num) //���潺���� �� ����
    {
        int spawnEnemyCount = 0;

        while (spawnEnemyCount < currentWave.field[num].spawnCount)
        {
            GameObject clone = Instantiate(currentWave.field[num].enemyPrefab);
            Enemy enemy = clone.GetComponent<Enemy>();
            EnemyHP enemyHP = clone.GetComponent<EnemyHP>();
            TowerWeapon attackSet = clone.GetComponent<TowerWeapon>();

            int wayRand = Random.Range(0, currentWave.field[num].path.Length);
            int wayNum = (int)currentWave.field[num].path[wayRand];

            if (wayNum == 6)
            {
                wayNum = Random.Range(0, pathPoints.Length);
            }

            GameObject[] way = pathPoints[wayNum].wayPoints;
            Transform[] paths = new Transform[way.Length];

            for (int i = 0; i < way.Length; i++)
            {
                paths[i] = way[i].GetComponent<Transform>();
            }

            enemy.Setup(this, paths, WaveType.Defense, UnitTypeData.Enemy);

            enemyHP.SetMaxHP(waveSystem.CurrentWave * 2.5f);

            attackSet.Setup(towerSpawner, this, null, null, null);

            enemylist.Add(enemy);

            SpawnEnemyHPSlider(clone);

            OnBuffAllBuffEnemy();

            yield return new WaitForSeconds(currentWave.field[num].spawnDelay);
            spawnEnemyCount++;
        }
    }

    private void SpawnEnemy2() //���潺���� �� ����
    {
        for (int i = 0; i < currentWave2.field.Length; i++)
        {
            int rand = Random.Range(0, currentWave2.field[i].posX.Length);
            Vector3 enemyPos = new Vector3((currentWave2.field[i].posX[rand] - 1) - 8.5f, 1.5f, 4.5f - ((int)currentWave2.field[i].posY[rand]));
            GameObject clone = Instantiate(currentWave2.field[i].enemyPrefab, enemyPos, Quaternion.Euler(new Vector3(65, 0, 0)));
            Enemy enemy = clone.GetComponent<Enemy>();
            EnemyHP enemyHP = clone.GetComponent<EnemyHP>();
            TowerWeapon attackSet = clone.GetComponent<TowerWeapon>();

            enemy.Setup(this, null, WaveType.Offense, UnitTypeData.Enemy);

            enemyHP.SetMaxHP(waveSystem.CurrentWave2 * 2.5f);

            attackSet.Setup(towerSpawner, this, null, null, null);

            enemylist.Add(enemy);

            SpawnEnemyHPSlider(clone);
        }

        OnBuffAllBuffEnemy();
    }

    public void OnBuffAllBuffEnemy() //���� Ÿ���� �ֺ��� ��� Ÿ���� ������ �ɵ��� ��
    {
        GameObject[] towers = GameObject.FindGameObjectsWithTag("Enemy");

        for (int i = 0; i < towers.Length; ++i)
        {
            TowerWeapon weapon = towers[i].GetComponent<TowerWeapon>();

            if (weapon.WeaponType == WeaponType.Buff || weapon.WeaponType == WeaponType.BuffM || weapon.WeaponType == WeaponType.BuffR)
            {
                weapon.OnBuffAroundEnemy();
            }
        }
    }

    //���潺 ���ѽð� ����
    public void OffenseTimeCheck()
    {
        timeOut = 60.0f + InventorySet.instance.OffenceBounsTime;
        StartCoroutine("OffenseTimeOut");
    }

    //���潺 ������ ������ ȹ��
    private IEnumerator DefenseEnd()
    {
        while(true)
        {
            if (currentEnemyCount <= 0)
            {
                towerSpawner.TowerListSave(0);  //���潺 ���ֵ� ��Ȱ��ȭ
                itemSpawn.ItemRandomGet();  //������ ȹ�� ������ ����
                StopCoroutine("DefenseEnd");
            }
            
            yield return null;
        }

    }

    //���潺 ���ѽð� üũ
    private IEnumerator OffenseTimeOut()
    {
        while (timeOut > 0)
        {
            timeOut -= Time.deltaTime;
            
            if (timeOut <= 0 || currentEnemyCount <= 0)
            {
                //���ѽð��� ���� ���� �� �߰� ���� ����
                playerGold.CurrentGold += (int)(timeOut * (1 + waveSystem.CurrentWave2) + InventorySet.instance.ClearReward);
                playerGold.CurrentGold += (int)(playerGold.CurrentGold * InventorySet.instance.PercentReward);
                gamePoint += (int)(timeOut * (5 + waveSystem.CurrentWave2));

                if (playerGold.CurrentGold < 0)
                    playerGold.CurrentGold = 0;

                GameManager.instance.playerGold.MaxGold();

                //������ �����Ͻ� ���� Ŭ���� ȭ������ �̵�
                if (waveSystem.CurrentWave2 >= waveSystem.MaxWave)
                {
                    playerHP.StartCoroutine("GameClearFadeOut");
                    StopCoroutine("OffenseTimeOut");
                }

                //�ʿ� �����ִ� �� ���� ����
                for (int i = 0; i < enemylist.Count; i++)
                {
                    Destroy(enemylist[i].gameObject);
                }
                enemylist = new List<Enemy>();

                towerSpawner.TowerListSave(1); //���� �ִ� ���潺 ���ֵ� ��Ȱ��ȭ
                waveSystem.ReadyWave();  //���̺� �ý��� �۵�
                StopCoroutine("OffenseTimeOut");
            }

            yield return new WaitForSeconds(Time.deltaTime);
        }
    }

    //�� �ı���
    public void DestroyEnemy(EnemyDestroyType type, Enemy enemy, int gold)
    {
        //���� �������� ������ �����Ͻ�
        if (type == EnemyDestroyType.Arrive)
        {
            playerHP.TakeDamage(1);
        }
        //���� ü���� 0�̵Ǿ� ����ҽ�
        else if (type == EnemyDestroyType.Kill)
        {
            gamePoint += (3 * waveSystem.CurrentWave) + 3;

            if (waveSystem.WaveType == WaveType.Offense)
            {
                int gainGold = gold + (int)(waveSystem.CurrentWave2 * (gold / 6));
                playerGold.CurrentGold += gainGold;
            }
        }

        currentEnemyCount -= 1;
        enemylist.Remove(enemy);
        Destroy(enemy.gameObject);
    }

    private void SpawnEnemyHPSlider(GameObject enemy) //�� ü�� �� ����
    {
        GameObject sliderClone = Instantiate(enemyHPSliderPrefab);
        sliderClone.transform.SetParent(canvasTransform);
        sliderClone.transform.localScale = Vector3.one;

        sliderClone.GetComponent<SliderPositionAutoSetter>().Setup(enemy.transform);
        sliderClone.GetComponent<EnemyHPViewer>().Setup(enemy.GetComponent<EnemyHP>());
    }

    [System.Serializable]
    public struct FieldSet
    {
        public GameObject[] gameField;
        public GameObject[] wayPoints;
    }
}
