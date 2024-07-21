using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.AI;

public class EnemySystem : MonoBehaviourPun, IPunObservable
{
    public GameObject atk;
    public bool isBoss;

    PhotonView pv;
    Rigidbody2D rb;
    Animator anim;

    List<Transform> players = new List<Transform>();
    Transform target;

    SpriteRenderer[] sprites;
    float hp;
    float dmg;
    float enemySpeed;

    bool isMove;
    bool isAttack;

    Vector3 remotePos;
    Quaternion remoteRot;

    // Start is called before the first frame update
    void Start()
    {
        remotePos = transform.position;
        remoteRot = transform.rotation;

        if (isBoss)
        {
            hp = 600f;
            dmg = 45f;
            enemySpeed = 4f;
        }
        else
        {
            hp = (GameManager.ins.lv * 0.5f) + 1f;
            dmg = Random.Range(2.1f, 3.0f);
            enemySpeed = Random.Range(3.8f, 7.5f);
        }

        pv = GetComponent<PhotonView>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprites = GetComponentsInChildren<SpriteRenderer>();

        GameObject[] playersArray = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject player in playersArray)
        {

            SkillData skillData = player.GetComponent<PlayerSystem>().playerStatus.charStat[0].skillSet;
            if (skillData.status[0].name == "장난끼 많은 소녀" && !skillData.status[0].isUse)
                continue;

            players.Add(player.transform);

        }

        // 랜덤한 플레이어를 추적 대상으로 설정
        if (players.Count > 0)
        {
            target = players[Random.Range(0, players.Count)];
        }
    }

    void FixedUpdate()
    {
        if (pv.IsMine)
        {
            if (target != null)
            {
                if (!anim.GetBool("isMoving"))
                    anim.SetBool("isMoving", true);

                // 대상 방향으로 이동
                if (target.GetComponent<PlayerSystem>().isLive)
                    transform.position = Vector2.MoveTowards(transform.position, target.position, enemySpeed * Time.fixedDeltaTime);

                else
                {
                    if (anim.GetBool("isMoving"))
                        anim.SetBool("isMoving", false);

                    if (isAttack == true)
                    {
                        isAttack = false;
                        StopCoroutine(EnemyAttack());
                    }
                }

                Vector2 dirVec = (target.position - transform.position).normalized;
                pv.RPC("EnemyMove", RpcTarget.AllBuffered, dirVec.x);

                // 근처에 더 가까운 플레이어가 있는지 확인하여 추적 대상 변경
                float closestDistance = Vector3.Distance(transform.position, target.position);
                foreach (Transform player in players)
                {
                    SkillData skillData = player.GetComponent<PlayerSystem>().playerStatus.charStat[0].skillSet;
                    if (skillData.status[0].name == "장난끼 많은 소녀" && skillData.status[0].isUse)
                        continue;

                    if (player != target)
                    {
                        float distance = Vector3.Distance(transform.position, player.position);
                        if (distance < closestDistance)
                        {
                            if (isAttack == false)
                            {
                                isAttack = true;
                                StartCoroutine(EnemyAttack());
                            }
                            closestDistance = distance;
                            target = player;
                        }
                    }
                }
            }
            else
            {

                if (anim.GetBool("isMoving"))
                    anim.SetBool("isMoving", false);

                if (isAttack == true)
                {
                    isAttack = false;
                    StopCoroutine(EnemyAttack());
                }

            }
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, remotePos, 10 * Time.fixedDeltaTime);
            transform.rotation = Quaternion.Lerp(transform.rotation, remoteRot, 10 * Time.fixedDeltaTime);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (isAttack == false)
            {
                isAttack = true;
                StartCoroutine(EnemyAttack());
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Aura"))
        {
            HitAttack(0.1f);
        }
    }

    IEnumerator EnemyAttack()
    {
        while (isAttack)
        {
            anim.SetTrigger("attack");
            atk.SetActive(true);
            yield return new WaitForSeconds(0.1f);
            atk.SetActive(false);
            yield return new WaitForSeconds(1f);
        }
    }

    public void HitAttack(float dmg)
    {
        hp -= dmg;

        if (hp <= 0)
        {
            GameManager.ins.ExpbarGauge(1f);

            if (isBoss)
                GameManager.ins.ExpbarGauge(GameManager.ins.maxExp / 2);

            pv.RPC("ObjectDestroy", RpcTarget.AllBuffered);
        }
    }

    [PunRPC]
    void EnemyMove(float axis)
    {
        if (axis < 0)
            transform.rotation = Quaternion.Euler(0f, 180f, 0f);
        else
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
    }

    [PunRPC]
    void ObjectDestroy()
    {
        Destroy(gameObject);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
        }
        else
        {
            remotePos = (Vector3)stream.ReceiveNext();
            remoteRot = (Quaternion)stream.ReceiveNext();
        }
    }

}
