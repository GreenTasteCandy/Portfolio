using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

/*
 * 적 AI 관련 스크립트임
 * 딱히 만질일은 없을거지만 암튼 그렇게 알아두셈 ㅇㅋ?
 * 시스템 자체는 포라스트때 타워 AI에서 돚거 한 뒤 개조함
 */

public enum EnemyType { none = 0, defensive, aggressive }
public enum EnemyState { Idle = 0, Walk, Attack, Dead, Back, Stun }

public class EnemySystem : MonoBehaviour
{
    [Header("Status")]
    public int num;
    public string name;
    public EnemyType type;
    public float health;
    public float damage;
    public float speed;
    public float rate;
    public float range;

    public Animator anim;
    public GameObject attackRange;
    public GameObject hitEffect;

    public float returnRange;
    public bool isFind;
    public bool isDead;
    public bool isActive;

    public bool Grounded = true;
    public float GroundedOffset = -0.14f;
    public float GroundedRadius = 0.28f;
    public LayerMask GroundLayers;
    public float FallTimeout = 0.15f;
    public float JumpTimeout = 0.50f;
    public float Gravity = -15.0f;
    public float JumpHeight = 1.2f;

    private float _verticalVelocity;
    private float _terminalVelocity = 53.0f;

    // timeout deltatime
    private float _jumpTimeoutDelta;
    private float _fallTimeoutDelta;

    bool isHit;
    float maxHP;
    Vector3 location;
    float delay_;
    float delayMove;
    Rigidbody rigid;
    Gathering table;
    public EnemyState state = EnemyState.Idle;


    private void Start()
    {
        name = GameManager.ins.enemyTable.enemyTable[num].name;
        health = GameManager.ins.enemyTable.enemyTable[num].health;
        type = GameManager.ins.enemyTable.enemyTable[num].type;
        damage = GameManager.ins.enemyTable.enemyTable[num].damage;
        speed = GameManager.ins.enemyTable.enemyTable[num].speed;
        rate = GameManager.ins.enemyTable.enemyTable[num].rate;
        range = GameManager.ins.enemyTable.enemyTable[num].range;
        returnRange = GameManager.ins.enemyTable.enemyTable[num].returnRange;

        delayMove = Random.Range(2.5f, 5.0f);
        maxHP = health;
        location = transform.position;
        rigid = GetComponent<Rigidbody>();
        table = GetComponent<Gathering>();

        ChangeState(state);

        isActive = Vector3.Distance(transform.position, GameManager.ins.player.transform.position) < 100;
    }

    private void Update()
    {
        if (isDead)
            return;

        //JumpAndGravity();
        //GroundedCheck();

        if (health <= 0)
        {
            StopAllCoroutines();
            attackRange.SetActive(false);
            anim.SetBool("Stun", false);

            int rand = Random.Range(1, 4);
            GameManager.ins.getitem = table.gameObject.GetComponent<Gathering>().GatherItem();
            GameManager.ins.getitem.count = rand;

            anim.SetTrigger("isDead");
            anim.SetBool("Dead", true);
            isDead = true;
            ChangeState(EnemyState.Dead);
        }
    }
    private void GroundedCheck() //현재 땅을 밣고 있는지 체크하는 함수
    {
        // set sphere position, with offset
        Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset,
            transform.position.z);
        Grounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers,
            QueryTriggerInteraction.Ignore);

    }

    private void JumpAndGravity() //점프 및 중력에 관련한 기능
    {
        if (GameManager.ins.stamina <= 0)
            return;

        if (Grounded) //땅을 밟고 있을 때, 이때는 점프를 누르면 위로 올라가게 된다
        {
            // reset the fall timeout timer
            _fallTimeoutDelta = FallTimeout;

            // stop our velocity dropping infinitely when grounded
            if (_verticalVelocity < 0.0f)
            {
                _verticalVelocity = -2f;
            }

            if (_jumpTimeoutDelta <= 0.0f)
            {
                // the square root of H * -2 * G = how much velocity needed to reach desired height
                _verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * Gravity);

            }

            // jump timeout
            if (_jumpTimeoutDelta >= 0.0f)
            {
                _jumpTimeoutDelta -= Time.deltaTime;
            }
        }
        else //땅을 안 밟고 있을 때,이때는 중력이 작동한다
        {
            // reset the jump timeout timer
            _jumpTimeoutDelta = JumpTimeout;

            // fall timeout
            if (_fallTimeoutDelta >= 0.0f)
            {
                _fallTimeoutDelta -= Time.deltaTime;
            }

        }

        // apply gravity over time if under terminal (multiply by delta time twice to linearly speed up over time)
        if (_verticalVelocity < _terminalVelocity)
        {
            _verticalVelocity += Gravity * Time.deltaTime;
        }
    }

    void ChangeState(EnemyState swapState)
    {
        StopCoroutine(state.ToString());
        state = swapState;
        StartCoroutine(state.ToString());
    }

    IEnumerator Idle() //대기 상태
    {
        float delay = 0f;

        while (true)
        {
            rigid.velocity = Vector3.zero;
            delay += Time.deltaTime;

            if (Vector3.Distance(transform.position, GameManager.ins.player.transform.position) <= range)
            {
                anim.SetBool("Run", true);
                anim.SetBool("Walk", false);
                ChangeState(EnemyState.Attack);
            }

            if (delay >= delayMove)
            {
                anim.SetBool("Run", false);
                anim.SetBool("Walk", true);
                delayMove = Random.Range(1f, 3f);
                ChangeState(EnemyState.Walk);
            }

            yield return null;
        }    
    }

    IEnumerator Walk() //걷기 상태
    {
        float delay = 0f;
        Vector3 dirVec = new Vector3(Random.Range(-1.0f, 1.1f), 0, Random.Range(-1.0f, 1.1f));

        while (true)
        {
            Vector3 nextVec = dirVec * (speed / 2f) * Time.fixedDeltaTime;
            Vector3 direction = rigid.position + nextVec;

            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(nextVec), 0.1f);
            rigid.MovePosition(direction);

            delay += Time.deltaTime;

            if (Vector3.Distance(transform.position, GameManager.ins.player.transform.position) <= range)
            {
                if (GameManager.ins.isAtivite)
                {
                    anim.SetBool("Run", true);
                    anim.SetBool("Walk", false);
                    ChangeState(EnemyState.Attack);
                }
            }

            if (Vector3.Distance(transform.position, location) >= returnRange)
            {
                anim.SetBool("Run", true);
                anim.SetBool("Walk", false);
                ChangeState(EnemyState.Back);
            }

            if (delay >= delayMove)
            {
                anim.SetBool("Run", false);
                anim.SetBool("Walk", false);
                delayMove = Random.Range(2.5f, 5.0f);
                ChangeState(EnemyState.Idle);
            }

            yield return null;
        }    
    }

    IEnumerator Attack() //공격 상태
    {
        Vector3 dirVec;
        Vector3 dir = Vector3.zero;

        while (true)
        {

            if (type == EnemyType.defensive)
                dirVec = rigid.position - GameManager.ins.player.transform.position;
            else
                dirVec = GameManager.ins.player.transform.position - rigid.position;

            Vector3 nextVec = dirVec.normalized * speed * Time.deltaTime;
            Vector3 direction = rigid.position + nextVec;

            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(nextVec), 0.3f);
            rigid.MovePosition(direction);

            if (!GameManager.ins.isAtivite)
            {
                anim.SetBool("Run", false);
                anim.SetBool("Walk", false);
                ChangeState(EnemyState.Idle);
            }
            
            if (Vector3.Distance(transform.position, GameManager.ins.player.transform.position) > range)
            {
                anim.SetBool("Run", false);
                anim.SetBool("Walk", false);
                ChangeState(EnemyState.Idle);
            }

            if (Vector3.Distance(transform.position, location) >= returnRange)
            {
                anim.SetBool("Run", true);
                anim.SetBool("Walk", false);
                ChangeState(EnemyState.Back);
            }

            if (type == EnemyType.aggressive)
            {
                delay_ += Time.deltaTime;

                if (delay_ >= rate)
                {
                    anim.SetTrigger("isAttack");
                    attackRange.SetActive(true);
                    yield return new WaitForSeconds(0.1f);
                    attackRange.SetActive(false);
                    delay_ = 0f;
                }
            }

            yield return null;
        }
    }

    IEnumerator Back() //복귀 상태
    {
        while (true)
        {
            Vector3 dirVec = location - rigid.position;
            Vector3 nextVec = dirVec.normalized * speed * 2f * Time.deltaTime;
            Vector3 direction = rigid.position + nextVec;

            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(nextVec), 0.1f);
            rigid.MovePosition(direction);

            if (Vector3.Distance(transform.position, location) <= 0.5f)
            {
                anim.SetBool("Run", false);
                anim.SetBool("Walk", false);
                ChangeState(EnemyState.Idle);
            }

            yield return null;
        }
    }

    IEnumerator Dead() //사망 상태
    {
        GameManager.ins.inven.GetItem(GameManager.ins.getitem);

        //의뢰 목표인 개체라면 처치 수 기록
        for (int i = 0; i < GameSetting.ins.requestTable.requests.Length; i++)
        {
            for (int j = 0; j < GameSetting.ins.requestTable.requests[i].requestTargets.Length; j++)
            {
                if (GameSetting.ins.requestTable.requests[i].requestTargets[j].targetType == RequestTargetType.Hunt && GameSetting.ins.requestTable.requests[i].requestTargets[j].targetEnemyIndex == num && GameSetting.ins.requestTable.requests[i].requestTargets[j].targetEnemyCount < GameSetting.ins.requestTable.requests[i].requestTargets[j].targetEnemyCountGoal)
                {
                    ++GameSetting.ins.requestTable.requests[i].requestTargets[j].targetEnemyCount;
                }
            }
        }

        yield return new WaitForSeconds(5f);

        transform.SetParent(null);
        gameObject.SetActive(false);
    }

    private void OnCollisionEnter(Collision collision)
    {
        //플레이어게 충돌 시
        if (collision.gameObject.CompareTag("Player"))
        {
            rigid.velocity = Vector3.zero;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //플레이어 공격에 맞았을 때
        if (other.gameObject.CompareTag("Weapon"))
        {
            if (!isHit)
            {
                GameManager.ins.EnemyGaugeShow(name, health / maxHP);

                if (other.GetComponent<SkillCollider>().skillOn)
                {
                    StartCoroutine(HitSkillEffect());
                }
                else
                    StartCoroutine(HitEffect());

            }
        }
        else if (other.gameObject.CompareTag("Bullet"))
        {
            if (!isHit)
            {
                GameManager.ins.EnemyGaugeShow(name, health / maxHP);

                StartCoroutine(HitEffect());

                if (other.GetComponent<BulletSystem>().isStun)
                {
                    StopAllCoroutines();
                    StartCoroutine(Stun());
                }
            }
        }
    }

    IEnumerator HitEffect()
    {
        isHit = true;

        health -= GameManager.ins.damage;
        Transform bullet = GameManager.ins.poolManager.GetPool(1).transform;

        bullet.position = transform.position + Vector3.up;

        yield return new WaitForSeconds(1.03f);

        bullet.gameObject.SetActive(false);
        isHit = false;
    }

    IEnumerator HitSkillEffect()
    {
        isHit = true;

        int weaponNum = GameManager.ins.player.GetComponent<PlayerActing>().weaponNum;
        float skillDmg = GameManager.ins.weaponTable.weapon[weaponNum].skillDmg;
        int skillObj = GameManager.ins.weaponTable.weapon[weaponNum].skillObj;

        health -= skillDmg;
        Transform bullet = GameManager.ins.poolManager.GetPool(skillObj).transform;

        bullet.position = transform.position + Vector3.up;

        yield return new WaitForSeconds(1.03f);

        bullet.gameObject.SetActive(false);
        isHit = false;
    }

    IEnumerator Stun()
    {
        anim.SetBool("Stun", true);

        yield return new WaitForSeconds(3f);

        anim.SetBool("Stun", false);
        ChangeState(state);
    }
}
