using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//유닛 설치시 타입 구분
public enum BuildTowerType { Tower = 0, Hero }

/*
 * 타워를 설치하는 시스템에 관련된 기능이 들어있는 스크립트
 * 타워 설치,설치 준비,설치 취소,설치시 체력바 생성등의 기능이 들어가 있다
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

    bool isOnTowerButton = false; //타워 설치 버튼을 눌렀는가
    bool[] isHeroCoolTime = new bool[3]; //영웅 설치 대기중인가
    float[] heroCoolTime = new float[3];
    GameObject followTowerClone = null; //설치할 때 보이는 임시 타워
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
        //설치되는 유닛,영웅,장애물 리스트 초기화
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

        //필드에 장애물 설치 부분
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

    public void ReadyToSpawnTower(int type) //유닛 설치 준비
    {
        soundEffect.SEPlay(0);
        towerType = type;

        if (isOnTowerButton == true)//버튼 중복해서 누르는 것 방지
        {
            isOnTowerButton = false;
            Destroy(followTowerClone);
            return;
        }

        //디펜스에서 유닛 설치
        if (GameManager.instance.waveSystem.WaveType == WaveType.ReadyWave || GameManager.instance.waveSystem.WaveType == WaveType.Defense)
        {
            //유닛 코스트가 부족할시 화면에 문구 표시
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

        //오펜스에서 유닛 생성
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

    public void ReadyToSpawnHero(int type) //영웅 설치 준비
    {
        soundEffect.SEPlay(0);
        towerType = type;

        if (isOnTowerButton == true) //버튼 중복해서 누르는 것 방지
        {
            for (int i = 0; i < SettingVariable.heroSlot.Length; i++)
            {
                if (GameObject.Find(SettingVariable.heroSlot[i].followTowerPrefab.name))
                    Destroy(SettingVariable.heroSlot[i].followTowerPrefab);
            }

            isOnTowerButton = false;

            return;
        }

        if (isHeroCoolTime[type] == true) //해당 영웅 쿨타임 상태일시 설치불가
            return;

        if (SettingVariable.heroSlot[type].towerNum >= 1) // 영웅 설치시 스킬 발동
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

        //디펜스에서 영웅 설치
        if (GameManager.instance.waveSystem.WaveType == WaveType.ReadyWave || GameManager.instance.waveSystem.WaveType == WaveType.Defense)
        {
            isOnTowerButton = true;

            buildTower = BuildTowerType.Hero;
            followTowerClone = Instantiate(SettingVariable.heroSlot[towerType].followTowerPrefab);
            StartCoroutine("OnTowerCancelSystem");
        }

        //오펜스에서 영웅 생성
        else if (GameManager.instance.waveSystem.WaveType == WaveType.Offense)
        {
            buildType = BuildTowerType.Hero;
            typeNum = type;
            OffenceUnit(pathNum);

            StartCoroutine("OnTowerCancelSystem");
        }
    }

    public void OffenseWaySet(int waySet) //오펜스 경로 설정
    {
        pathNum = waySet;
    }

    public void OffenceUnit(int waySet) //오펜스 경로별 유닛 생성
    {
        int randPos = waySet;

        //유닛 생성 부분
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

        //영웅 생성 부분
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

    //디펜스 유닛 설치
    public void SpawnTower(Transform tileTransform,BuildTowerType buildTower)
    {
        if (isOnTowerButton == false) //타워 설치 버튼을 눌렀을 때에만 작동하도록 하기 위함
        {
            return;
        }

        TileCheck tile = tileTransform.GetComponent<TileCheck>();

        if (tile.isBuildTower == true) //이미 어떤 타워가 설치되어 있다면 설치 불가
        {
            systemTextViewer.PrintText(SystemType.Build);
            return;
        }

        isOnTowerButton = false;
        tile.isBuildTower = true;

        //타워가 설치될때 진행되는 타워 초기값 및 실제 오브젝트 배치
        if (buildTower == BuildTowerType.Tower)
        {
            //타워 설치시 비용 감소
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
        //영웅이 설치될때 진행되는 영웅 초기값 및 실제 오브젝트 배치
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

        OnBuffAllBuffTowers(); //새로 배치되는 타워가 버프 타워 주변에 배치될 때 버프 효과를 받아야 하므로 버프 효과 갱신

        Destroy(followTowerClone);
        StopCoroutine("OnTowerCancelSystem");
    }

    private IEnumerator HeroCoolDown()//영웅 1번 버튼 쿨타임
    {
        float cooldown = coolTime[0];

        while (heroCoolTime[0] < cooldown)
        {
            heroCoolTime[0] += Time.deltaTime;

            //쿨타임이 끝나면 원 상태로 돌아간다
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

    private IEnumerator HeroCoolDown2()//영웅 2번 버튼 쿨타임
    {
        float cooldown = coolTime[1];

        while (heroCoolTime[1] < cooldown)
        {
            heroCoolTime[1] += Time.deltaTime;

            //쿨타임이 끝나면 원 상태로 돌아간다
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

    private IEnumerator HeroCoolDown3()//영웅 3번 버튼 쿨타임
    {
        float cooldown = coolTime[2];

        while (heroCoolTime[2] < cooldown)
        {
            heroCoolTime[2] += Time.deltaTime;

            //쿨타임이 끝나면 원 상태로 돌아간다
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

    public void OnBuffAllBuffTowers() //버프 타워가 주변에 모든 타워를 버프를 걸도록 함
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

    private IEnumerator OnTowerCancelSystem() //타워 설치 취소
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

    public void UnitRemove(TowerWeapon unit) //타워 삭제(오펜스 타워 삭제 및 장애물 삭제시)
    {
        //타일 체크가 되었는지 확인
        if (unit.TileBuild != null)
            unit.TileBuild.isBuildTower = false; //확인 후 비활성화

        //타워의 설치갯수,리스트 값 제거
        unit.TowerTemplate.towerNum = 0;
        towerList.Remove(unit);
        towersDefense.Remove(unit.gameObject);

        //타워 삭제
        Destroy(unit.gameObject);
    }

    public void UnitDestroy(TowerWeapon unit) //타워 파괴
    {
        //타워 테이터 타입 체크
        if (unit.UnitType == UnitTypeData.Object)
        {
            GameManager.instance.playerGold.PayGold(-(int)InventorySet.instance.DestroyRock);

            //장애물일경우 바로 삭제
            unit.TileBuild.isBuildTower = false;
            rockList.Remove(unit.gameObject);
            Destroy(unit.gameObject);
            if (ObjectDetector.touchScreen == true)
            {
                ObjectDetector.touchScreen = false;
            }
            return;
        }

        //영웅 파괴시 쿨타임 실행
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

        //유닛 파괴시
        else if (unit.UnitType == UnitTypeData.Unit)
        {
            //유닛 파괴시 디펜스 라운드인지 체크
            if (GameManager.instance.waveSystem.WaveType == WaveType.Defense)
            {
                //유닛의 갯수만 파괴
                unit.TowerTemplate.towerNum--;
            }
        }

        //타워의 리스트 값 삭제 후 타워 파괴
        towerList.Remove(unit);
        towersDefense.Remove(unit.gameObject);
        if (ObjectDetector.touchScreen == true)
        {
            ObjectDetector.touchScreen = false;
        }
        Destroy(unit.gameObject);
    }

    private void SpawnTowerHPSlider(GameObject tower) //타워 체력 바 생성
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

    //현재 저장된 타워 리스트의 데이터를 저장/불러오기
    public void TowerListSave(int check)
    {

        //타워리스트 저장 후 초기화
        if (check == 0)
        {
            for(int h = 0; h < rockList.Count; h++)
            {
                rockList[h].SetActive(false);
            }

            //쿨타임 실행시 쿨타임 초기화
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

        //저장된 타워리스트를 불러오기
        else if (check == 1)
        {
            for (int h = 0; h < rockList.Count; h++)
            {
                rockList[h].SetActive(true);
            }
            
            //쿨타임 실행시 쿨타임 초기화
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
