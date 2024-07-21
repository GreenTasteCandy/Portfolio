using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * 오브젝트의 공격 시스템을 담당하는 스크립트
 * 적,아군 모두 해당 스크립트를 사용한다
 */
public enum WeaponType { Cannon = 0, Laser, Melee, Area, Buff, BuffM, BuffR, BuffA, Heal }
public enum WeaponState { SearchTarget = 0, TryAttackCannon, TryAttackLaser, TryAttackMelee, TryAttackArea, TryAttackSkill }

public class TowerWeapon : MonoBehaviour
{
    //오브젝트의 기본 설정값
    [Header("Commons")]
    [SerializeField]
    TowerTemplate towerTemplate;
    [SerializeField]
    Transform spawnPoint;
    [SerializeField]
    TypeObject type;
    [SerializeField]
    WeaponType weaponType;

    //투사체 공격 설정값
    [Header("Cannon")]
    [SerializeField]
    GameObject projectilePrefab;

    //히트스캔 공격 설정값
    [Header("Laser")]
    [SerializeField]
    LineRenderer lineRenderer;
    [SerializeField]
    Transform hitEffect;
    [SerializeField]
    LayerMask targetLayer;

    [Header("AttackEffects")]
    [SerializeField]
    GameObject[] attackEffects;

    int level;
    float currentHP;
    WeaponState weaponState = WeaponState.SearchTarget;
    Transform attackTarget = null;
    SpriteRenderer spriteRenderer;
    EnemySpawn enemySpawn;
    TowerSpawner towerSummon;
    PlayerGold playerMoney;
    TileCheck ownerTile;
    InventorySet equip;
    EnemyHP hpCurrent;

    float addedDamage;
    int buffLevel; //버프를 받는 여부 (0 : 버프 X, 1 이상 : 받는 버프 레벨)
    float skillBuffRate;
    bool skillDebuff;
    float skillDelay;
    Animator m_Anim;
    Movement2D moveCheck;

    /*
     * 외부에서 오브젝트의 능력치를 받아올때 사용되는 프로퍼티 제작
     */
    public int Level => towerTemplate.towerLevel;
    public Sprite TowerSprite => towerTemplate.weapon[Level].sprite;
    public float MaxHP => towerTemplate.weapon[Level].maxHp;
    public float Damage => towerTemplate.weapon[Level].damage;
    public float Rate => towerTemplate.weapon[Level].rate;
    public float Range => towerTemplate.weapon[Level].range;
    public int MaxLevel => towerTemplate.weapon.Length - 1;
    public float Speed => towerTemplate.weapon[Level].speed;
    public int Cost => towerTemplate.weapon[Level].cost;
    public float SkillCoolDown => towerTemplate.weapon[Level].skillCooltime;
    public float SkillDelay => towerTemplate.weapon[Level].skillDelay;
    public string Name => towerTemplate.nameTag;
    public string Notice => towerTemplate.notice;
    public WeaponType WeaponType => weaponType;
    public UnitTypeData UnitType => towerTemplate.unitTypeData;
    public TowerTemplate TowerTemplate => towerTemplate;
    public InventorySet Inventory => equip;
    public TileCheck TileBuild => ownerTile;
    public float AddedDamage
    {
        set => addedDamage = Mathf.Max(0, value);
        get => addedDamage;
    }
    public int BuffLevel
    {
        set => buffLevel = Mathf.Max(0, value);
        get => buffLevel;
    }

    //오브젝트의 기본 설정
    public void Setup(TowerSpawner towerSpawn, EnemySpawn enemySpawner, PlayerGold playerGold, TileCheck checkTile, InventorySet inven)
    {
        if (UnitType != UnitTypeData.Object)
            m_Anim = transform.Find("Model").GetComponent<Animator>();

        moveCheck = GetComponent<Movement2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        hpCurrent = GetComponent<EnemyHP>();

        enemySpawn = enemySpawner;
        towerSummon = towerSpawn;
        playerMoney = playerGold;
        ownerTile = checkTile;
        equip = inven;

        if (UnitType != UnitTypeData.Object)
            ChangeState(WeaponState.SearchTarget);
    }

    public void ChangeState(WeaponState newState) //공격 상태 변경
    {
        StopCoroutine(weaponState.ToString());
        weaponState = newState;
        StartCoroutine(weaponState.ToString());
    }

    void RotateToTarget() //공격 대상을 향해 회전
    {
        float dx = attackTarget.position.x - transform.position.x;
        float dy = attackTarget.position.y - transform.position.y;
        float dz = attackTarget.position.z - transform.position.z;

        float degree = Mathf.Atan2(dx, dy) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, degree);
    }

    private IEnumerator SearchTarget() //공격 대상 탐색
    {
        while(true)
        {
            attackTarget = FindClosestAttackTarget();

            if (moveCheck.enabled == true)
                m_Anim.Play("Run");

            moveCheck.isMove = true; //공격할 대상이 없으면 계속 이동

            if (attackTarget != null)
            {
                if (UnitType == UnitTypeData.Boss)
                {
                    int skillRand = Random.Range(0, 100);
                    if (skillRand > 80)
                        ChangeState(WeaponState.TryAttackSkill);
                    else
                    {
                        if (weaponType == WeaponType.Cannon || weaponType == WeaponType.BuffR)
                        {
                            ChangeState(WeaponState.TryAttackCannon);
                        }
                        else if (weaponType == WeaponType.Laser || weaponType == WeaponType.BuffM)
                        {
                            ChangeState(WeaponState.TryAttackLaser);
                        }
                        else if (weaponType == WeaponType.Area || weaponType == WeaponType.BuffA)
                        {
                            ChangeState(WeaponState.TryAttackArea);
                        }
                    }
                }
                else
                {
                    if (weaponType == WeaponType.Cannon || weaponType == WeaponType.BuffR)
                    {
                        ChangeState(WeaponState.TryAttackCannon);
                    }
                    else if (weaponType == WeaponType.Laser || weaponType == WeaponType.BuffM)
                    {
                        ChangeState(WeaponState.TryAttackLaser);
                    }
                    else if (weaponType == WeaponType.Area || weaponType == WeaponType.BuffA || weaponType == WeaponType.Heal)
                    {
                        ChangeState(WeaponState.TryAttackArea);
                    }
                }
            }

            yield return null;
        }
    }

    private IEnumerator TryAttackCannon() //투사체 공격
    {
        while (true)
        {
            if (IsPossibleToAttackTarget() == false)
            {
                ChangeState(WeaponState.SearchTarget);
                break;
            }

            m_Anim.Play("Attack");
            SpawnProjectile();

            if (UnitType == UnitTypeData.Hero)
            {
                float delay = towerTemplate.weapon[Level].rate - equip.ItemRate;
                float rate = delay - (delay * skillBuffRate);
                if (rate < 0)
                    rate = 0.05f;
                yield return new WaitForSeconds(rate);
            }

            else
                yield return new WaitForSeconds(towerTemplate.weapon[Level].rate);

        }
    }

    private IEnumerator TryAttackArea() //범위 공격
    {
        while (true)
        {
            if (IsPossibleToAttackTarget() == false)
            {
                ChangeState(WeaponState.SearchTarget);
                break;
            }

            m_Anim.Play("Attack");
            AttackArea();

            if (UnitType == UnitTypeData.Hero)
            {
                float delay = towerTemplate.weapon[Level].rate - equip.ItemRate;
                float rate = delay - (delay * skillBuffRate);
                if (rate < 0)
                    rate = 0.05f;
                yield return new WaitForSeconds(rate);
            }

            else
                yield return new WaitForSeconds(towerTemplate.weapon[Level].rate);

        }
    }

    private IEnumerator TryAttackLaser() //히트스캔 공격
    {

        while (true)
        {
            if (IsPossibleToAttackTarget() == false)
            {
                ChangeState(WeaponState.SearchTarget);
                break;
            }

            m_Anim.Play("Attack");
            SpawnLaser();

            if (UnitType == UnitTypeData.Hero)
            {
                float delay = towerTemplate.weapon[Level].rate - equip.ItemRate;
                float rate = delay - (delay * skillBuffRate);
                if (rate < 0)
                    rate = 0.05f;
                yield return new WaitForSeconds(rate);
            }
            else
                yield return new WaitForSeconds(towerTemplate.weapon[Level].rate);
        }
    }

    private IEnumerator TryAttackSkill() //영웅 스킬
    {
        Instantiate(attackEffects[2], transform.position, Quaternion.Euler(new Vector3(85, 0, 0)));

        if (towerTemplate.nameTag == "사자")
        {
            m_Anim.Play("Attack");
            Collider[] hit = Physics.OverlapSphere(spawnPoint.position, 3, targetLayer);

            for (int i = 0; i < hit.Length; ++i)
            {
                float skillDamage = towerTemplate.weapon[Level].damage * 1.2f;
                hit[i].gameObject.GetComponent<EnemyHP>().TakeDamage(skillDamage, attackEffects[1]);
            }
        }

        else if (towerTemplate.nameTag == "늑대")
        {
            m_Anim.Play("Attack");
            Collider[] hit = Physics.OverlapSphere(spawnPoint.position, 2, targetLayer);

            for (int i = 0; i < hit.Length; ++i)
            {
                float skillDamage = towerTemplate.weapon[Level].damage * 1.5f;
                hit[i].gameObject.GetComponent<EnemyHP>().TakeDamage(skillDamage, attackEffects[1]);
            }
        }

        if (towerTemplate.nameTag == "용과")
        {
            m_Anim.Play("Attack");
            Collider[] hit = Physics.OverlapSphere(spawnPoint.position, towerTemplate.weapon[Level].range + equip.ItemRange, targetLayer);

            for (int i = 0; i < hit.Length; ++i)
            {
                float skillDamage = (towerTemplate.weapon[Level].damage + equip.ItemDamage) * 1.2f;
                hit[i].gameObject.GetComponent<EnemyHP>().TakeDamage(skillDamage, attackEffects[3]);
            }

        }

        else if (towerTemplate.nameTag == "백년초")
        {
            skillBuffRate = 0.5f;
            skillDelay = towerTemplate.weapon[0].skillDelay;
            StartCoroutine(SkillTimeDelay());
        }

        else if (towerTemplate.nameTag == "라플레시아")
        {
            skillDebuff = true;
            skillDelay = towerTemplate.weapon[0].skillDelay;
            StartCoroutine(SkillTimeDelay());
        }

        else if (towerTemplate.nameTag == "맹그로브")
        {
            for (int j = 0; j < 3; j++)
            {
                m_Anim.Play("Attack");
                Collider[] hit = Physics.OverlapSphere(spawnPoint.position, towerTemplate.weapon[Level].range + equip.ItemRange, targetLayer);

                for (int i = 0; i < hit.Length; ++i)
                {
                    float skillDamage = (towerTemplate.weapon[Level].damage + equip.ItemDamage) * 0.4f;
                    hit[i].gameObject.GetComponent<EnemyHP>().TakeDamage(skillDamage, attackEffects[3]);
                }
                yield return new WaitForSeconds(0.2f);
            }

        }

        else if (towerTemplate.nameTag == "사과")
        {
            m_Anim.Play("Attack");
            Collider[] hit = Physics.OverlapSphere(spawnPoint.position, towerTemplate.weapon[Level].range + equip.ItemRange, LayerMask.GetMask("Unit"));

            for (int i = 0; i < hit.Length; ++i)
            {
                hit[i].gameObject.GetComponent<EnemyHP>().PlusHP(100.0f, attackEffects[3]);
            }

        }

        else if (towerTemplate.nameTag == "두리안")
        {
            if (IsPossibleToAttackTarget() == true)
            {
                Vector3 direction = attackTarget.position - spawnPoint.position;
                RaycastHit[] hit = Physics.RaycastAll(spawnPoint.position, direction, towerTemplate.weapon[Level].range + equip.ItemRange, targetLayer);

                for (int i = 0; i < hit.Length; ++i)
                {
                    if (hit[i].transform == attackTarget)
                    {
                        hitEffect.position = hit[i].point;
                        float skillDamage = (towerTemplate.weapon[Level].damage + equip.ItemDamage) * 3.0f;
                        attackTarget.GetComponent<EnemyHP>().TakeDamage(skillDamage, attackEffects[3]);
                    }
                }
            }

        }

        ChangeState(WeaponState.SearchTarget);
        yield return null;
    }

    private IEnumerator SkillTimeDelay() // 스킬 지속시간
    {
        float timeDelay = 0;

        while (timeDelay < skillDelay)
        {
            timeDelay += Time.deltaTime;

            yield return new WaitForSeconds(Time.deltaTime);
        }

        skillDebuff = false;
        skillBuffRate = 0;
        StopCoroutine("SkillTimeDelay");
    }

    public void OnBuffAroundTower() //아군 유닛 버프
    {
        GameObject[] towers = GameObject.FindGameObjectsWithTag("Tower");

        for (int i  = 0; i < towers.Length; ++i)
        {
            TowerWeapon weapon = towers[i].GetComponent<TowerWeapon>();

            if (UnitType == UnitTypeData.Hero)
            {
                float range = towerTemplate.weapon[level].range + equip.ItemRange;
                if (Vector3.Distance(weapon.transform.position, transform.position) <= range)
                {
                    weapon.AddedDamage = weapon.Damage * (towerTemplate.weapon[level].buff);
                    weapon.BuffLevel = Level;
                }
            }
            else
            {
                if (Vector3.Distance(weapon.transform.position, transform.position) <= towerTemplate.weapon[level].range)
                {
                    weapon.AddedDamage = weapon.Damage * (towerTemplate.weapon[level].buff);
                    weapon.BuffLevel = Level;
                }
            }
        }
    }

    public void OnBuffAroundEnemy() //적 유닛 버프
    {
        GameObject[] towers = GameObject.FindGameObjectsWithTag("Enemy");

        for (int i = 0; i < towers.Length; ++i)
        {
            TowerWeapon weapon = towers[i].GetComponent<TowerWeapon>();

            if (Vector3.Distance(weapon.transform.position, transform.position) <= towerTemplate.weapon[level].range)
            {
                weapon.AddedDamage = weapon.Damage * (towerTemplate.weapon[level].buff);
                weapon.BuffLevel = Level;
            }
        }
    }


    Transform FindClosestAttackTarget() //가장 가까운 공격 대상 탐색
    {
        float closestDistSqr = Mathf.Infinity; //가장 가까이 있는 적을 찾기 위해 최초 거리를 최대로

        //유닛의 공격탐색
        if (type == TypeObject.Unit)
        {
            if (weaponType == WeaponType.Heal)
            {
                for (int i = 0; i < towerSummon.TowerList.Count; ++i)
                {
                    float distance = Vector3.Distance(towerSummon.TowerList[i].transform.position, transform.position);

                    if (distance <= towerTemplate.weapon[Level].range && distance <= closestDistSqr)
                    {
                        closestDistSqr = distance;
                        attackTarget = towerSummon.TowerList[i].transform;
                    }
                }
            }
            else
            {
                for (int i = 0; i < enemySpawn.EnemyList.Count; ++i)
                {
                    float distance = Vector3.Distance(enemySpawn.EnemyList[i].transform.position, transform.position);

                    if (UnitType == UnitTypeData.Hero)
                    {
                        if (distance <= (towerTemplate.weapon[Level].range + equip.ItemRange) && distance <= closestDistSqr)
                        {
                            closestDistSqr = distance;
                            attackTarget = enemySpawn.EnemyList[i].transform;
                        }
                    }
                    else
                    {
                        if (distance <= towerTemplate.weapon[Level].range && distance <= closestDistSqr)
                        {
                            closestDistSqr = distance;
                            attackTarget = enemySpawn.EnemyList[i].transform;
                        }
                    }
                }
            }
        }

        //적의 공격 탐색
        else if (type == TypeObject.Enemy)
        {
            for (int i = 0; i < towerSummon.TowerList.Count; ++i)
            {
                float distance = Vector3.Distance(towerSummon.TowerList[i].transform.position, transform.position);

                if (distance <= towerTemplate.weapon[Level].range && distance <= closestDistSqr)
                {
                    closestDistSqr = distance;
                    attackTarget = towerSummon.TowerList[i].transform;
                }
            }
        }

        return attackTarget;
    }

    bool IsPossibleToAttackTarget() //공격 대상에게 공격이 가능한지 검사
    {
        if (attackTarget == null)
        {
            return false;
        }

        float distance = Vector3.Distance(attackTarget.position, transform.position);

        if (UnitType == UnitTypeData.Hero)
        {
            if (distance > towerTemplate.weapon[Level].range + equip.ItemRange) //공격 범위 안에 있는지 검사하고, 공격 범위를 벗어났다면 새로운 적 탐색
            {
                attackTarget = null;
                return false;
            }
            else
            {
                moveCheck.isMove = false; //공격이 가능할시 이동을 멈춤
            }
        }
        else
        {
            if (distance > towerTemplate.weapon[Level].range) //공격 범위 안에 있는지 검사하고, 공격 범위를 벗어났다면 새로운 적 탐색
            {
                attackTarget = null;
                return false;
            }
            else
            {
                if (UnitType != UnitTypeData.Boss)
                    moveCheck.isMove = false; //공격이 가능할시 이동을 멈춤
            }
        }

        return true;
    }

    void SpawnProjectile() //발사체 생성
    {
        GameObject clone = Instantiate(projectilePrefab, spawnPoint.position, Quaternion.Euler(new Vector3(85, 0, 0)));
        float dmg = towerTemplate.weapon[level].damage + AddedDamage;
        float damage = dmg + (dmg * skillBuffRate);
        if (UnitType == UnitTypeData.Hero)
        {
            Instantiate(attackEffects[0], transform.position, Quaternion.Euler(new Vector3(85, 0, 0)));
            float dmg_ = dmg + equip.ItemDamage;
            damage = dmg_ + (dmg_ * skillBuffRate);
        }

        clone.GetComponent<Projectile>().Setup(attackTarget, damage, type, attackEffects[1]);
    }

    void EnableLaser() //레이저 효과 킴
    {
        lineRenderer.gameObject.SetActive(true);
        //hitEffect.gameObject.SetActive(true);
    }

    void DisableLaser() //레이저 효과 끔
    {
        lineRenderer.gameObject.SetActive(false);
        //hitEffect.gameObject.SetActive(false);
    }

    void SpawnLaser() //레이저 생성
    {
        Vector3 direction = attackTarget.position - spawnPoint.position;
        RaycastHit[] hit = Physics.RaycastAll(spawnPoint.position, direction, towerTemplate.weapon[Level].range, targetLayer);
        if (UnitType == UnitTypeData.Hero)
            hit = Physics.RaycastAll(spawnPoint.position, direction, towerTemplate.weapon[Level].range + equip.ItemRange, targetLayer);

        for (int i = 0; i < hit.Length; ++i)
        {
            if (hit[i].transform == attackTarget)
            {
                //lineRenderer.SetPosition(0, spawnPoint.position);
                //lineRenderer.SetPosition(1, new Vector3(hit[i].point.x, hit[i].point.y, hit[i].point.z));
                hitEffect.position = hit[i].point;
                if (UnitType == UnitTypeData.Hero)
                {
                    Instantiate(attackEffects[0], transform.position, Quaternion.Euler(new Vector3(85, 0, 0)));
                    attackTarget.GetComponent<EnemyHP>().TakeDamage(towerTemplate.weapon[Level].damage + equip.ItemDamage, attackEffects[1]);
                }
                else
                {
                    attackTarget.GetComponent<EnemyHP>().TakeDamage(towerTemplate.weapon[Level].damage, attackEffects[0]);
                }
            }
        }
    }

    void AttackArea() //범위공격시 데미지
    {
        Instantiate(attackEffects[0], transform.position, Quaternion.Euler(new Vector3(85, 0, 0)));

        if (towerTemplate.nameTag == "튤립")
        {
            Collider[] hit = Physics.OverlapSphere(spawnPoint.position, towerTemplate.weapon[Level].range, LayerMask.GetMask("Unit"));

            for (int i = 0; i < hit.Length; ++i)
            {
                hit[i].gameObject.GetComponent<EnemyHP>().PlusHP(towerTemplate.weapon[Level].damage, attackEffects[1]);
            }
        }

        else
        {
            Collider[] hit = Physics.OverlapSphere(spawnPoint.position, towerTemplate.weapon[Level].range + equip.ItemRange, targetLayer);

            for (int i = 0; i < hit.Length; ++i)
            {
                float skillDamage = (towerTemplate.weapon[Level].damage + equip.ItemDamage);
                hit[i].gameObject.GetComponent<EnemyHP>().TakeDamage(skillDamage, attackEffects[1]);
                if (skillDebuff == true)
                    hit[i].gameObject.GetComponent<Movement2D>().MoveDebuff(towerTemplate.weapon[Level].skillDelay, towerTemplate.weapon[Level].buff);
            }
        }
    }

    public int Upgrade() //아군 유닛 강화
    {
        //강화 제한
        if (Level == 1 && WaveSystem.instance.CurrentWave <= 5)
        {
            return 2;
        }
        if (Level == 2 && WaveSystem.instance.CurrentWave <= 12)
        {
            return 2;
        }
        if (Level == 3 && WaveSystem.instance.CurrentWave <= 20)
        {
            return 2;
        }

        int upgradeCost = towerTemplate.weapon[Level + 1].cost;

        if (playerMoney.CurrentGold < upgradeCost) //돈 없으면 꺼져
        {
            return 0;
        }

        playerMoney.CurrentGold -= upgradeCost;
        
        towerTemplate.towerLevel += 1;

        int lv = towerTemplate.towerLevel;
        float plusHP = towerTemplate.weapon[lv].maxHp - towerTemplate.weapon[lv - 1].maxHp;

        for(int i = 0; i < towerSummon.TowerList.Count; i++)
        {
            TowerTemplate tower = towerSummon.TowerList[i].towerTemplate;

            if (tower == towerTemplate)
                towerSummon.TowerList[i].hpCurrent.PlusHP(plusHP);
        }

        if (weaponType == WeaponType.Laser) //레이저는 레벨에 따라 레이저 굵기 변화
        {
            lineRenderer.startWidth = 0.05f + Level * 0.05f;
            lineRenderer.endWidth = 0.05f;
        }

        towerSummon.OnBuffAllBuffTowers(); //강화될 때 버프 효과 갱신

        return 1;
    }

    public void Sell() //판매
    {
        if (UnitType != UnitTypeData.Object)
        {
            if (UnitType == UnitTypeData.Hero)
                towerTemplate.towerNum = 0;
            else
                towerTemplate.towerNum--;

            playerMoney.CurrentGold += towerTemplate.weapon[Level].sell;
            UnitDie();
        }
        else if (UnitType == UnitTypeData.Object)
        {
            if (playerMoney.CurrentGold >= towerTemplate.weapon[Level].sell)
            {
                playerMoney.CurrentGold -= towerTemplate.weapon[Level].sell;
                towerSummon.UnitDestroy(this);
            }
        }
    }

    public void UnitDie() //유닛 사망
    {
        if (ownerTile != null)
            ownerTile.isBuildTower = false;

        towerSummon.UnitDestroy(this);
    }

}
