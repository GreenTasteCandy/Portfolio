using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class BulletSystem : MonoBehaviourPun
{
    public PhotonView pv;
    public Rigidbody2D rb;

    WeaponType type;
    Vector2 dir;
    PlayerStatus stat;
    float dmg;
    float speed;
    float attackRange;

    float range;

    public void Setup(WeaponType type, Vector2 dir, PlayerStatus stat)
    {
        this.type = type;
        this.dir = dir;
        this.stat = stat;

        if (type == WeaponType.melee)
        {
            Invoke("MeleeDestroy", 1f);
        }
        else if (type == WeaponType.range)
        {
            rb.AddForce(dir * stat.charStat[0].shotSpeed, ForceMode2D.Impulse);
        }
    }

    private void Update()
    {
        if (type == WeaponType.range)
        {
            range += Time.deltaTime * stat.charStat[0].shotSpeed;

            if (range >= stat.charStat[0].atkRange)
                MeleeDestroy();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            if (stat.charStat[0].atkType == WeaponType.melee)
            {
                GameObject hitFx = PhotonNetwork.Instantiate("Effect01", collision.transform.position, Quaternion.identity);
                GameObject atkEnemy = PhotonNetwork.Instantiate("BloodFX", collision.transform.position, Quaternion.identity);

                collision.GetComponent<EnemySystem>().HitAttack(stat.charStat[0].atk);
                Invoke("MeleeDestroy", 1f);
            }
            else if (stat.charStat[0].atkType == WeaponType.range)
            {
                GameObject hitFx = PhotonNetwork.Instantiate("Effect01", collision.transform.position, Quaternion.identity);
                GameObject atkEnemy = PhotonNetwork.Instantiate("BloodFX", collision.transform.position, Quaternion.identity);

                collision.GetComponent<EnemySystem>().HitAttack(stat.charStat[0].atk);
                MeleeDestroy();
            }
        }
        else if (collision.CompareTag("Wall"))
        {
            if (stat.charStat[0].atkType == WeaponType.range)
                MeleeDestroy();
        }
    }

    void MeleeDestroy()
    {
        pv.RPC("ObjectDestroy", RpcTarget.AllBuffered);
    }

    [PunRPC]
    void ObjectDestroy()
    {
        Destroy(gameObject);
    }

}
