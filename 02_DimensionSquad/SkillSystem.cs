using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SkillSystem : MonoBehaviourPun
{
    public PhotonView pv;
    public Rigidbody2D rb;
    public CapsuleCollider2D collider;

    Vector2 dir;
    SkillData data;
    int num;

    float range;
    public void Setup(Vector2 dir, int num, SkillData stat)
    {
        this.dir = dir;
        this.num = num;
        data = stat;

        if (data.status[num].atkType == WeaponType.range)
        {
            rb.AddForce(dir * data.status[num].speed, ForceMode2D.Impulse);
        }
        else
        {
            Invoke("SkillDestroy", 1f);
        }
    }
    private void Update()
    {
        if (data.status[num].atkType == WeaponType.range)
        {
            range += Time.deltaTime;

            if (range >= data.status[num].range)
            {
                SkillDestroy();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (data.status[num].atkType == WeaponType.heal)
        {
            if (collision.CompareTag("Player"))
            {
                collision.GetComponent<PlayerSystem>().HealPlayer(data.status[num].dmg);
            }
        }
        else if (data.status[num].atkType == WeaponType.melee)
        {
            if (collision.CompareTag("Enemy"))
            {
                GameObject hitFx = PhotonNetwork.Instantiate("Effect01", collision.transform.position, Quaternion.identity);
                GameObject atkEnemy = PhotonNetwork.Instantiate("BloodFX", collision.transform.position, Quaternion.identity);

                collision.GetComponent<EnemySystem>().HitAttack(data.status[num].dmg);
                Invoke("SkillDestroy", 1f);
            }
        }
        else if (data.status[num].atkType == WeaponType.range)
        {
            if (collision.CompareTag("Enemy"))
            {
                GameObject hitFx = PhotonNetwork.Instantiate("Effect01", collision.transform.position, Quaternion.identity);
                GameObject atkEnemy = PhotonNetwork.Instantiate("BloodFX", collision.transform.position, Quaternion.identity);

                collision.GetComponent<EnemySystem>().HitAttack(data.status[num].dmg);
                SkillDestroy();
            }
        }
    }

    void SkillDestroy()
    {
        if (data.status[num].activeType == SkillActive.Grenade)
        {
            GameObject hit = PhotonNetwork.Instantiate(data.status[num].skillobj[1].name, transform.position, Quaternion.identity);
            hit.GetComponent<SkillSystem>().Setup(Vector2.zero, 0, data);
        }
        pv.RPC("ObjectDestroy", RpcTarget.AllBuffered);
    }

    [PunRPC]
    void ObjectDestroy()
    {
        Destroy(gameObject);
    }

}