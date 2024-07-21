using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//���� ��ġ�� Ÿ�� ����
public enum BuildTowerType { Tower = 0, Hero }

/*
 * Ÿ���� ��ġ�ϴ� �ý��ۿ� ���õ� ����� ����ִ� ��ũ��Ʈ
 * Ÿ�� ��ġ,��ġ �غ�,��ġ ���,��ġ�� ü�¹� �������� ����� �� �ִ�
 */
public class TowerSpawner : MonoBehaviour
{
    [SerializeField]
    SystemTextViewer systemTextViewer;
    [SerializeField]
    Transform canvasTransform;
    [SerializeField]
    GameObject towerHPSliderPrefab;
    [SerializeField]
    InventorySet inven;
    [SerializeField]
    GameObject inGameUI;
    [SerializeField]
    SkillSpawner skillSpawner;

    [SerializeField]
    OptionMenu soundEffect;

    [SerializeField]
    Transform[] fieldObject;
    [SerializeField]
    GameObject rockObject;
    [SerializeField]
    int rockNum;

    bool isOnTowerButton = false; //Ÿ�� ��ġ ��ư�� �����°�
    bool[] isHeroCoolTime = new bool[3]; //���� ��ġ ������ΰ�
    float[] heroCoolTime = new float[3];
    GameObject followTowerClone = null; //��ġ�� �� ���̴� �ӽ� Ÿ��
    int towerType;
    int typeNum;
    BuildTowerType buildType;
    int[] towerDefenseNum = new int[5];
    List<TowerWeapon> towerList;
    List<TowerWeapon> towerListBackup;
    List<GameObject> towersDefense;
    List<GameObject> rockList;
    float[] coolTime = new float[3];
    int pathNum;

    BuildTowerType buildTower;

    public int[] TowerSummon => towerDefenseNum;
    public float[] HeroCoolTime => heroCoolTime;
    public bool[] IsHeroCool => isHeroCoolTime;
    public bool IsOnTowerBuild => isOnTowerButton;
    public int TowerType => towerType;
    public BuildTowerType BuildTower => buildTower;
    public TowerTemplate[] towerTemPlate => SettingVariable.unitSlot;
    public TowerTemplate[] heroTemPlate => SettingVariable.heroSlot;
    public List<TowerWeapon> TowerList => towerList;
    public float[] SkillRate => coolTime;
    public int PathNum => pathNum;

    private void Start()
    {
        //��ġ�Ǵ� ����,����,��ֹ� ����Ʈ �ʱ�ȭ
        towerList = new List<TowerWeapon>();
        towerListBackup = new List<TowerWeapon>();
        towersDefense = new List<GameObject>();
        rockList = new List<GameObject>();

        for(int i = 0; i < SettingVariable.unitSlot.Length; i++)
        {
            SettingVariable.unitSlot[i].towerNum = 0;
            SettingVariable.unitSlot[i].towerLevel = 0;
        }

        for(int j = 0; j < SettingVariable.heroSlot.Length; j++)
        {
            SettingVariable.heroSlot[j].towerNum = 0;
        }

        //�ʵ忡 ��ֹ� ��ġ �κ�
        for (int i = 0; i < rockNum; i++)
        {
            int ranRock = Random.Range(0, fieldObject.Length);
            TileCheck tile = fieldObject[ranRock].GetComponent<TileCheck>();

            if (tile.isBuildTower == true)
            {
                i--;
                continue;
            }

            tile.isBuildTower = true;

            Vector3 rockPosition = fieldObject[ranRock].transform.position;
            rockPosition.y += 1.0f;

            GameObject rock = Instantiate(rockObject, rockPosition, Quaternion.identity);
            TowerWeapon rocky = rock.GetComponent<TowerWeapon>();
            rock.GetComponent<TowerWeapon>().Setup(this, GameManager.instance.enemySpawner, GameManager.instance.playerGold, tile, null);
            rockList.Add(rock);
        }
    }

    public void ReadyToSpawnTower(int type) //���� ��ġ �غ�
    {
        soundEffect.SEPlay(0);
        towerType = type;

        if (isOnTowerButton == true)//��ư �ߺ��ؼ� ������ �� ����
        {
            isOnTowerButton = false;
            Destroy(followTowerClone);
            return;
        }

        //���潺���� ���� ��ġ
        if (GameManager.instance.waveSystem.WaveType == WaveType.ReadyWave || GameManager.instance.waveSystem.WaveType == WaveType.Defense)
        {
            //���� �ڽ�Ʈ�� �����ҽ� ȭ�鿡 ���� ǥ��
            int lv = SettingVariable.unitSlot[towerType].towerLevel;
            int cost = SettingVariable.unitSlot[towerType].weapon[lv].cost - (int)inven.UnitCostDown;
            int price = cost < SettingVariable.unitSlot[towerType].weapon[0].cost ? SettingVariable.unitSlot[towerType].weapon[0].cost : cost;

            if (price > GameManager.instance.playerGold.CurrentGold)
            {
                systemTextViewer.PrintText(SystemType.Money);
                return;
            }

            isOnTowerButton = true;
            buildTower = BuildTowerType.Tower;

            followTowerClone = Instantiate(SettingVariable.unitSlot[towerType].followTowerPrefab);

            StartCoroutine("OnTowerCancelSystem");
        }

        //���潺���� ���� ����
        else if (GameManager.instance.waveSystem.WaveType == WaveType.Offense)
        {

            if (towerDefenseNum[type] >= 1)
            {
                buildType = BuildTowerType.Tower;
                typeNum = type;
                OffenceUnit(pathNum);

                StartCoroutine("OnTowerCancelSystem");
            }
        }
    }

    public void ReadyToSpawnHero(int type) //���� ��ġ �غ�
    {
        soundEffect.SEPlay(0);
        towerType = type;

        if (isOnTowerButton == true) //��ư �ߺ��ؼ� ������ �� ����
        {
            for (int i = 0; i < SettingVariable.heroSlot.Length; i++)
            {
                if (GameObject.Find(SettingVariable.heroSlot[i].followTowerPrefab.name))
                    Destroy(SettingVariable.heroSlot[i].followTowerPrefab);
            }

            isOnTowerButton = false;

            return;
        }

        if (isHeroCoolTime[type] == true) //�ش� ���� ��Ÿ�� �����Ͻ� ��ġ�Ұ�
            return;

        if (SettingVariable.heroSlot[type].towerNum >= 1) // ���� ��ġ�� ��ų �ߵ�
        {
            if (isHeroCoolTime[type] == false)
            {
                coolTime[type] = SettingVariable.heroSlot[type].weapon[0].skillCooltime;
                skillSpawner.skillOn[type] = true;
                skillSpawner.SkillActive(SettingVariable.heroSlot[type]);
                if (type == 0)
                    StartCoroutine("HeroCoolDown");
                else if (type == 1)
                    StartCoroutine("HeroCoolDown2");
                else if (type == 2)
                    StartCoroutine("HeroCoolDown3");

                isHeroCoolTime[type] = true;
            }

            return;
        }

        //���潺���� ���� ��ġ
        if (GameManager.instance.waveSystem.WaveType == WaveType.ReadyWave || GameManager.instance.waveSystem.WaveType == WaveType.Defense)
        {
            isOnTowerButton = true;

            buildTower = BuildTowerType.Hero;
            followTowerClone = Instantiate(SettingVariable.heroSlot[towerType].followTowerPrefab);
            StartCoroutine("OnTowerCancelSystem");
        }

        //���潺���� ���� ����
        else if (GameManager.instance.waveSystem.WaveType == WaveType.Offense)
        {
            buildType = BuildTowerType.Hero;
            typeNum = type;
            OffenceUnit(pathNum);

            StartCoroutine("OnTowerCancelSystem");
        }
    }

    public void OffenseWaySet(int waySet) //���潺 ��� ����
    {
        pathNum = waySet;
    }

    public void OffenceUnit(int waySet) //���潺 ��κ� ���� ����
    {
        int randPos = waySet;

        //���� ���� �κ�
        if (buildType == BuildTowerType.Tower)
        {
            GameObject tower = Instantiate(SettingVariable.unitSlot[towerType].towerPrefab);
            tower.transform.localScale = new Vector3(-1, 1, 1);
            Enemy towerMove = tower.GetComponent<Enemy>();
            Movement2D movement = tower.GetComponent<Movement2D>();
            TowerWeapon towerWeapon = tower.GetComponent<TowerWeapon>();
            movement.enabled = true;

            GameObject[] way = GameManager.instance.enemySpawner.PathPoint[randPos].wayPoints;
            Transform[] waypoint = new Transform[way.Length];

            for (int i = 0; i < way.Length; i++)
            {
                waypoint[i] = way[i].GetComponent<Transform>();
            }

            towerMove.Setup(null, waypoint, WaveType.Offense, UnitTypeData.Unit);
            towerWeapon.Setup(this, GameManager.instance.enemySpawner, GameManager.instance.playerGold, null, null);
            SpawnTowerHPSlider(tower);
            towerList.Add(towerWeapon);
            towerDefenseNum[typeNum]--;
        }

        //���� ���� �κ�
        else if (buildType == BuildTowerType.Hero)
        {
            GameObject clone = Instantiate(SettingVariable.heroSlot[towerType].towerPrefab);
            clone.transform.localScale = new Vector3(-1, 1, 1);
            TowerWeapon tower = clone.GetComponent<TowerWeapon>();
            Enemy towerMove = clone.GetComponent<Enemy>();
            EnemyHP hpSet = clone.GetComponent<EnemyHP>();
            Movement2D movement = clone.GetComponent<Movement2D>();
            movement.enabled = true;

            GameObject[] way = GameManager.instance.enemySpawner.PathPoint[randPos].wayPoints;
            Transform[] waypoint = new Transform[way.Length];

            for (int i = 0; i < way.Length; i++)
            {
                waypoint[i] = way[i].GetComponent<Transform>();
            }

            towerMove.Setup(null, waypoint, WaveType.Offense, UnitTypeData.Hero);
            tower.Setup(this, GameManager.instance.enemySpawner, GameManager.instance.playerGold, null, inven);
            hpSet.SetMaxHP(inven.ItemMaxHP);
            SpawnTowerHPSlider(clone);

            towerList.Add(tower);
            SettingVariable.heroSlot[towerType].towerNum++;
        }
    }

    //���潺 ���� ��ġ
    public void SpawnTower(Transform tileTransform,BuildTowerType buildTower)
    {
        if (isOnTowerButton == false) //Ÿ�� ��ġ ��ư�� ������ ������ �۵��ϵ��� �ϱ� ����
        {
            return;
        }

        TileCheck tile = tileTransform.GetComponent<TileCheck>();

        if (tile.isBuildTower == true) //�̹� � Ÿ���� ��ġ�Ǿ� �ִٸ� ��ġ �Ұ�
        {
            systemTextViewer.PrintText(SystemType.Build);
            return;
        }

        isOnTowerButton = false;
        tile.isBuildTower = true;

        //Ÿ���� ��ġ�ɶ� ����Ǵ� Ÿ�� �ʱⰪ �� ���� ������Ʈ ��ġ
        if (buildTower == BuildTowerType.Tower)
        {
            //Ÿ�� ��ġ�� ��� ����
            int level = SettingVariable.unitSlot[towerType].towerLevel;
            int cost = SettingVariable.unitSlot[towerType].weapon[level].cost - (int)inven.UnitCostDown;
            int price = cost < 0 ? 0 : cost;

            GameManager.instance.playerGold.CurrentGold -= price;

            Vector3 position = tileTransform.position + new Vector3(0, 1.5f, 0);
            GameObject clone = Instantiate(SettingVariable.unitSlot[towerType].towerPrefab, position, Quaternion.Euler(new Vector3(65, 0, 0)));
            TowerWeapon tower = clone.GetComponent<TowerWeapon>();
            clone.GetComponent<TowerWeapon>().Setup(this, GameManager.instance.enemySpawner, GameManager.instance.playerGold, tile, null);
            SpawnTowerHPSlider(clone);
            towersDefense.Add(clone);
            towerList.Add(tower);
            SettingVariable.unitSlot[towerType].towerNum++;
        }
        //������ ��ġ�ɶ� ����Ǵ� ���� �ʱⰪ �� ���� ������Ʈ ��ġ
        else if (buildTower == BuildTowerType.Hero)
        {
            Vector3 position = tileTransform.position + new Vector3(0, 1.5f, 0);
            GameObject clone = Instantiate(SettingVariable.heroSlot[towerType].towerPrefab, position, Quaternion.Euler(new Vector3(65, 0, 0)));
            TowerWeapon tower = clone.GetComponent<TowerWeapon>();
            EnemyHP hpSet = clone.GetComponent<EnemyHP>();

            tower.Setup(this, GameManager.instance.enemySpawner, GameManager.instance.playerGold, tile, inven);
            hpSet.SetMaxHP(inven.ItemMaxHP);
            SpawnTowerHPSlider(clone);

            towersDefense.Add(clone);
            towerList.Add(tower);
            SettingVariable.heroSlot[towerType].towerNum++;
        }

        OnBuffAllBuffTowers(); //���� ��ġ�Ǵ� Ÿ���� ���� Ÿ�� �ֺ��� ��ġ�� �� ���� ȿ���� �޾ƾ� �ϹǷ� ���� ȿ�� ����

        Destroy(followTowerClone);
        StopCoroutine("OnTowerCancelSystem");
    }

    private IEnumerator HeroCoolDown()//���� 1�� ��ư ��Ÿ��
    {
        float cooldown = coolTime[0];

        while (heroCoolTime[0] < cooldown)
        {
            heroCoolTime[0] += Time.deltaTime;

            //��Ÿ���� ������ �� ���·� ���ư���
            if (heroCoolTime[0] >= cooldown)
            {
                isHeroCoolTime[0] = false;
                skillSpawner.skillOn[0] = false;
                StopCoroutine("HeroCoolDown");
                heroCoolTime[0] = 0;
            }

            yield return new WaitForSeconds(Time.deltaTime);
        }
    }

    private IEnumerator HeroCoolDown2()//���� 2�� ��ư ��Ÿ��
    {
        float cooldown = coolTime[1];

        while (heroCoolTime[1] < cooldown)
        {
            heroCoolTime[1] += Time.deltaTime;

            //��Ÿ���� ������ �� ���·� ���ư���
            if (heroCoolTime[1] >= cooldown)
            {
                isHeroCoolTime[1] = false;
                skillSpawner.skillOn[1] = false;
                StopCoroutine("HeroCoolDown2");
                heroCoolTime[1] = 0;
            }

            yield return new WaitForSeconds(Time.deltaTime);
        }
    }

    private IEnumerator HeroCoolDown3()//���� 3�� ��ư ��Ÿ��
    {
        float cooldown = coolTime[2];

        while (heroCoolTime[2] < cooldown)
        {
            heroCoolTime[2] += Time.deltaTime;

            //��Ÿ���� ������ �� ���·� ���ư���
            if (heroCoolTime[2] >= cooldown)
            {
                isHeroCoolTime[2] = false;
                skillSpawner.skillOn[2] = false;
                StopCoroutine("HeroCoolDown3");
                heroCoolTime[2] = 0;
            }

            yield return new WaitForSeconds(Time.deltaTime);
        }
    }

    public void OnBuffAllBuffTowers() //���� Ÿ���� �ֺ��� ��� Ÿ���� ������ �ɵ��� ��
    {
        GameObject[] towers = GameObject.FindGameObjectsWithTag("Tower");

        for ( int i = 0; i < towers.Length; ++i )
        {
            TowerWeapon weapon = towers[i].GetComponent<TowerWeapon>();

            if ( weapon.WeaponType == WeaponType.Buff)
            {
                weapon.OnBuffAroundTower();
            }
        }
    }

    private IEnumerator OnTowerCancelSystem() //Ÿ�� ��ġ ���
    {
        while (true)
        {
            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButtonDown(1))
            {
                isOnTowerButton = false;
                Destroy(followTowerClone);
                break;
            }

            yield return null;
        }
    }

    public void UnitRemove(TowerWeapon unit) //Ÿ�� ����(���潺 Ÿ�� ���� �� ��ֹ� ������)
    {
        //Ÿ�� üũ�� �Ǿ����� Ȯ��
        if (unit.TileBuild != null)
            unit.TileBuild.isBuildTower = false; //Ȯ�� �� ��Ȱ��ȭ

        //Ÿ���� ��ġ����,����Ʈ �� ����
        unit.TowerTemplate.towerNum = 0;
        towerList.Remove(unit);
        towersDefense.Remove(unit.gameObject);

        //Ÿ�� ����
        Destroy(unit.gameObject);
    }

    public void UnitDestroy(TowerWeapon unit) //Ÿ�� �ı�
    {
        //Ÿ�� ������ Ÿ�� üũ
        if (unit.UnitType == UnitTypeData.Object)
        {
            GameManager.instance.playerGold.PayGold(-(int)InventorySet.instance.DestroyRock);

            //��ֹ��ϰ�� �ٷ� ����
            unit.TileBuild.isBuildTower = false;
            rockList.Remove(unit.gameObject);
            Destroy(unit.gameObject);
            if (ObjectDetector.touchScreen == true)
            {
                ObjectDetector.touchScreen = false;
            }
            return;
        }

        //���� �ı��� ��Ÿ�� ����
        else if (unit.UnitType == UnitTypeData.Hero)
        {
            if (unit.TowerTemplate == SettingVariable.heroSlot[0])
            {
                StopCoroutine("HeroCoolDown");
                coolTime[0] = SettingVariable.heroSlot[0].weapon[0].cooldown - inven.ItemCoolDown;
                StartCoroutine("HeroCoolDown");
                isHeroCoolTime[0] = true;
            }
            else if (unit.TowerTemplate == SettingVariable.heroSlot[1])
            {
                StopCoroutine("HeroCoolDown2");
                coolTime[1] = SettingVariable.heroSlot[1].weapon[0].cooldown - inven.ItemCoolDown;
                StartCoroutine("HeroCoolDown2");
                isHeroCoolTime[1] = true;
            }
            else if (unit.TowerTemplate == SettingVariable.heroSlot[2])
            {
                StopCoroutine("HeroCoolDown3");
                coolTime[2] = SettingVariable.heroSlot[2].weapon[0].cooldown - inven.ItemCoolDown;
                StartCoroutine("HeroCoolDown3");
                isHeroCoolTime[2] = true;
            }
            unit.TowerTemplate.towerNum = 0;
        }

        //���� �ı���
        else if (unit.UnitType == UnitTypeData.Unit)
        {
            //���� �ı��� ���潺 �������� üũ
            if (GameManager.instance.waveSystem.WaveType == WaveType.Defense)
            {
                //������ ������ �ı�
                unit.TowerTemplate.towerNum--;
            }
        }

        //Ÿ���� ����Ʈ �� ���� �� Ÿ�� �ı�
        towerList.Remove(unit);
        towersDefense.Remove(unit.gameObject);
        if (ObjectDetector.touchScreen == true)
        {
            ObjectDetector.touchScreen = false;
        }
        Destroy(unit.gameObject);
    }

    private void SpawnTowerHPSlider(GameObject tower) //Ÿ�� ü�� �� ����
    {
        GameObject sliderClone = Instantiate(towerHPSliderPrefab);
        sliderClone.transform.SetParent(inGameUI.transform);
        sliderClone.transform.localScale = Vector3.one;

        sliderClone.GetComponent<SliderPositionAutoSetter>().Setup(tower.transform);
        sliderClone.GetComponent<EnemyHPViewer>().Setup(tower.GetComponent<EnemyHP>());
    }

    public void BuildSet(bool set)
    {
        isOnTowerButton = set;
    }

    //���� ����� Ÿ�� ����Ʈ�� �����͸� ����/�ҷ�����
    public void TowerListSave(int check)
    {

        //Ÿ������Ʈ ���� �� �ʱ�ȭ
        if (check == 0)
        {
            for(int h = 0; h < rockList.Count; h++)
            {
                rockList[h].SetActive(false);
            }

            //��Ÿ�� ����� ��Ÿ�� �ʱ�ȭ
            for (int u = 0; u < 3; u++)
            {
                isHeroCoolTime[u] = false;
                heroCoolTime[u] = 0;
            }
            StopCoroutine("HeroCoolDown");
            StopCoroutine("HeroCoolDown2");
            StopCoroutine("HeroCoolDown3");

            for (int j = 0; j < 5; j++)
            {
                towerDefenseNum[j] = SettingVariable.unitSlot[j].towerNum;
            }
            for (int i = 0; i < towerList.Count; i++)
            {
                TowerWeapon towerWeapon = towersDefense[i].GetComponent<TowerWeapon>();

                if (towerWeapon.UnitType == UnitTypeData.Hero)
                {
                    UnitRemove(towerWeapon);
                    i--;
                    continue;
                }

                towersDefense[i].SetActive(false);
            }
            inGameUI.SetActive(false);
            towerListBackup = towerList;
            towerList = new List<TowerWeapon>();
        }

        //����� Ÿ������Ʈ�� �ҷ�����
        else if (check == 1)
        {
            for (int h = 0; h < rockList.Count; h++)
            {
                rockList[h].SetActive(true);
            }
            
            //��Ÿ�� ����� ��Ÿ�� �ʱ�ȭ
            for (int u = 0; u < 3; u++)
            {
                isHeroCoolTime[u] = false;
                heroCoolTime[u] = 0;
            }
            StopCoroutine("HeroCoolDown");
            StopCoroutine("HeroCoolDown2");
            StopCoroutine("HeroCoolDown3");

            for (int i = 0; i < towerList.Count; i++)
            {
                if (towerList[i].UnitType == UnitTypeData.Hero)
                {
                    UnitRemove(towerList[i]);
                    i--;
                    continue;
                }
                GameObject towerObj = towerList[i].gameObject;
                Destroy(towerObj);
            }

            for(int i = 0; i < towersDefense.Count; i++)
            {
                towersDefense[i].SetActive(true);
                towersDefense[i].GetComponent<TowerWeapon>().ChangeState(WeaponState.SearchTarget);
            }

            inGameUI.SetActive(true);
            towerList = new List<TowerWeapon>();
            towerList = towerListBackup;
            towerListBackup = new List<TowerWeapon>();
        }

    }

}
