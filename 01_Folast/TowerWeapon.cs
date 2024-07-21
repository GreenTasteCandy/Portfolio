using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * ������Ʈ�� ���� �ý����� ����ϴ� ��ũ��Ʈ
 * ��,�Ʊ� ��� �ش� ��ũ��Ʈ�� ����Ѵ�
 */
public enum WeaponType { Cannon = 0, Laser, Melee, Area, Buff, BuffM, BuffR, BuffA, Heal }
public enum WeaponState { SearchTarget = 0, TryAttackCannon, TryAttackLaser, TryAttackMelee, TryAttackArea, TryAttackSkill }

public class TowerWeapon : MonoBehaviour
{
    //������Ʈ�� �⺻ ������
    [Header("Commons")]
    [SerializeField]
    TowerTemplate towerTemplate;
    [SerializeField]
    Transform spawnPoint;
    [SerializeField]
    TypeObject type;
    [SerializeField]
    WeaponType weaponType;

    //����ü ���� ������
    [Header("Cannon")]
    [SerializeField]
    GameObject projectilePrefab;

    //��Ʈ��ĵ ���� ������
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
    int buffLevel; //������ �޴� ���� (0 : ���� X, 1 �̻� : �޴� ���� ����)
    float skillBuffRate;
    bool skillDebuff;
    float skillDelay;
    Animator m_Anim;
    Movement2D moveCheck;

    /*
     * �ܺο��� ������Ʈ�� �ɷ�ġ�� �޾ƿö� ���Ǵ� ������Ƽ ����
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

    //������Ʈ�� �⺻ ����
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

    public void ChangeState(WeaponState newState) //���� ���� ����
    {
        StopCoroutine(weaponState.ToString());
        weaponState = newState;
        StartCoroutine(weaponState.ToString());
    }

    void RotateToTarget() //���� ����� ���� ȸ��
    {
        float dx = attackTarget.position.x - transform.position.x;
        float dy = attackTarget.position.y - transform.position.y;
        float dz = attackTarget.position.z - transform.position.z;

        float degree = Mathf.Atan2(dx, dy) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, degree);
    }

    private IEnumerator SearchTarget() //���� ��� Ž��
    {
        while(true)
        {
            attackTarget = FindClosestAttackTarget();

            if (moveCheck.enabled == true)
                m_Anim.Play("Run");

            moveCheck.isMove = true; //������ ����� ������ ��� �̵�

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

    private IEnumerator TryAttackCannon() //����ü ����
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

    private IEnumerator TryAttackArea() //���� ����
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

    private IEnumerator TryAttackLaser() //��Ʈ��ĵ ����
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

    private IEnumerator TryAttackSkill() //���� ��ų
    {
        Instantiate(attackEffects[2], transform.position, Quaternion.Euler(new Vector3(85, 0, 0)));

        if (towerTemplate.nameTag == "����")
        {
            m_Anim.Play("Attack");
            Collider[] hit = Physics.OverlapSphere(spawnPoint.position, 3, targetLayer);

            for (int i = 0; i < hit.Length; ++i)
            {
                float skillDamage = towerTemplate.weapon[Level].damage * 1.2f;
                hit[i].gameObject.GetComponent<EnemyHP>().TakeDamage(skillDamage, attackEffects[1]);
            }
        }

        else if (towerTemplate.nameTag == "����")
        {
            m_Anim.Play("Attack");
            Collider[] hit = Physics.OverlapSphere(spawnPoint.position, 2, targetLayer);

            for (int i = 0; i < hit.Length; ++i)
            {
                float skillDamage = towerTemplate.weapon[Level].damage * 1.5f;
                hit[i].gameObject.GetComponent<EnemyHP>().TakeDamage(skillDamage, attackEffects[1]);
            }
        }

        if (towerTemplate.nameTag == "���")
        {
            m_Anim.Play("Attack");
            Collider[] hit = Physics.OverlapSphere(spawnPoint.position, towerTemplate.weapon[Level].range + equip.ItemRange, targetLayer);

            for (int i = 0; i < hit.Length; ++i)
            {
                float skillDamage = (towerTemplate.weapon[Level].damage + equip.ItemDamage) * 1.2f;
                hit[i].gameObject.GetComponent<EnemyHP>().TakeDamage(skillDamage, attackEffects[3]);
            }

        }

        else if (towerTemplate.nameTag == "�����")
        {
            skillBuffRate = 0.5f;
            skillDelay = towerTemplate.weapon[0].skillDelay;
            StartCoroutine(SkillTimeDelay());
        }

        else if (towerTemplate.nameTag == "���÷��þ�")
        {
            skillDebuff = true;
            skillDelay = towerTemplate.weapon[0].skillDelay;
            StartCoroutine(SkillTimeDelay());
        }

        else if (towerTemplate.nameTag == "�ͱ׷κ�")
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

        else if (towerTemplate.nameTag == "���")
        {
            m_Anim.Play("Attack");
            Collider[] hit = Physics.OverlapSphere(spawnPoint.position, towerTemplate.weapon[Level].range + equip.ItemRange, LayerMask.GetMask("Unit"));

            for (int i = 0; i < hit.Length; ++i)
            {
                hit[i].gameObject.GetComponent<EnemyHP>().PlusHP(100.0f, attackEffects[3]);
            }

        }

        else if (towerTemplate.nameTag == "�θ���")
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

    private IEnumerator SkillTimeDelay() // ��ų ���ӽð�
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

    public void OnBuffAroundTower() //�Ʊ� ���� ����
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

    public void OnBuffAroundEnemy() //�� ���� ����
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


    Transform FindClosestAttackTarget() //���� ����� ���� ��� Ž��
    {
        float closestDistSqr = Mathf.Infinity; //���� ������ �ִ� ���� ã�� ���� ���� �Ÿ��� �ִ��

        //������ ����Ž��
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

        //���� ���� Ž��
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

    bool IsPossibleToAttackTarget() //���� ��󿡰� ������ �������� �˻�
    {
        if (attackTarget == null)
        {
            return false;
        }

        float distance = Vector3.Distance(attackTarget.position, transform.position);

        if (UnitType == UnitTypeData.Hero)
        {
            if (distance > towerTemplate.weapon[Level].range + equip.ItemRange) //���� ���� �ȿ� �ִ��� �˻��ϰ�, ���� ������ ����ٸ� ���ο� �� Ž��
            {
                attackTarget = null;
                return false;
            }
            else
            {
                moveCheck.isMove = false; //������ �����ҽ� �̵��� ����
            }
        }
        else
        {
            if (distance > towerTemplate.weapon[Level].range) //���� ���� �ȿ� �ִ��� �˻��ϰ�, ���� ������ ����ٸ� ���ο� �� Ž��
            {
                attackTarget = null;
                return false;
            }
            else
            {
                if (UnitType != UnitTypeData.Boss)
                    moveCheck.isMove = false; //������ �����ҽ� �̵��� ����
            }
        }

        return true;
    }

    void SpawnProjectile() //�߻�ü ����
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

    void EnableLaser() //������ ȿ�� Ŵ
    {
        lineRenderer.gameObject.SetActive(true);
        //hitEffect.gameObject.SetActive(true);
    }

    void DisableLaser() //������ ȿ�� ��
    {
        lineRenderer.gameObject.SetActive(false);
        //hitEffect.gameObject.SetActive(false);
    }

    void SpawnLaser() //������ ����
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

    void AttackArea() //�������ݽ� ������
    {
        Instantiate(attackEffects[0], transform.position, Quaternion.Euler(new Vector3(85, 0, 0)));

        if (towerTemplate.nameTag == "ƫ��")
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

    public int Upgrade() //�Ʊ� ���� ��ȭ
    {
        //��ȭ ����
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

        if (playerMoney.CurrentGold < upgradeCost) //�� ������ ����
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

        if (weaponType == WeaponType.Laser) //�������� ������ ���� ������ ���� ��ȭ
        {
            lineRenderer.startWidth = 0.05f + Level * 0.05f;
            lineRenderer.endWidth = 0.05f;
        }

        towerSummon.OnBuffAllBuffTowers(); //��ȭ�� �� ���� ȿ�� ����

        return 1;
    }

    public void Sell() //�Ǹ�
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

    public void UnitDie() //���� ���
    {
        if (ownerTile != null)
            ownerTile.isBuildTower = false;

        towerSummon.UnitDestroy(this);
    }

}
