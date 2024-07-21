using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*발사체가 발사되고 충돌 시 대미지를 주게 하는 스크립트*/
public class Projectile : MonoBehaviour
{
    Movement2D movement2D;
    Transform target;
    float damage;
    Vector3 direction;
    TypeObject typeBullet; //유닛 발사체인지 적 발사체인지 구분
    GameObject effect;

    //발사체 기본 설정
    public void Setup(Transform target, float damage, TypeObject type, GameObject effect)
    {
        movement2D = GetComponent<Movement2D>();
        this.target = target;
        this.damage = damage;
        this.effect = effect;
        typeBullet = type;
    }

    private void Update()
    {
        //타겟이 존재하는지 체크
        //존재할시 발사 방향으로 이동한다
        if (target != null)
        {
            Vector3 direction = (target.position - transform.position).normalized;
            movement2D.MoveTo(direction);
        }
        //타겟이 존재하지 않으면 삭제된다
        else 
        {
            Destroy(gameObject);
        }
    }

    //충돌 판정
    private void OnTriggerEnter(Collider collision)
    {
        //발사체가 유닛에게서 발사된 경우 타겟 이외의 오브젝트에겐 충돌 무시
        if (typeBullet == TypeObject.Unit)
        {
            if (!collision.CompareTag("Enemy")) return;
            if (collision.transform != target) return;
        }
        //발사체가 적에게서 발사된 경우 타겟 이외의 오브젝트에겐 충돌 무시
        else if (typeBullet == TypeObject.Enemy)
        {
            if (!collision.CompareTag("Tower")) return;
            if (collision.transform != target) return;
        }

        //충돌한 타겟에게 데미지를 주고 발사체는 삭제된다
        collision.GetComponent<EnemyHP>().TakeDamage(damage, effect);
        Destroy(gameObject);
    }

}
