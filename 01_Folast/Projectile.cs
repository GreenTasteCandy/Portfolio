using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*�߻�ü�� �߻�ǰ� �浹 �� ������� �ְ� �ϴ� ��ũ��Ʈ*/
public class Projectile : MonoBehaviour
{
    Movement2D movement2D;
    Transform target;
    float damage;
    Vector3 direction;
    TypeObject typeBullet; //���� �߻�ü���� �� �߻�ü���� ����
    GameObject effect;

    //�߻�ü �⺻ ����
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
        //Ÿ���� �����ϴ��� üũ
        //�����ҽ� �߻� �������� �̵��Ѵ�
        if (target != null)
        {
            Vector3 direction = (target.position - transform.position).normalized;
            movement2D.MoveTo(direction);
        }
        //Ÿ���� �������� ������ �����ȴ�
        else 
        {
            Destroy(gameObject);
        }
    }

    //�浹 ����
    private void OnTriggerEnter(Collider collision)
    {
        //�߻�ü�� ���ֿ��Լ� �߻�� ��� Ÿ�� �̿��� ������Ʈ���� �浹 ����
        if (typeBullet == TypeObject.Unit)
        {
            if (!collision.CompareTag("Enemy")) return;
            if (collision.transform != target) return;
        }
        //�߻�ü�� �����Լ� �߻�� ��� Ÿ�� �̿��� ������Ʈ���� �浹 ����
        else if (typeBullet == TypeObject.Enemy)
        {
            if (!collision.CompareTag("Tower")) return;
            if (collision.transform != target) return;
        }

        //�浹�� Ÿ�ٿ��� �������� �ְ� �߻�ü�� �����ȴ�
        collision.GetComponent<EnemyHP>().TakeDamage(damage, effect);
        Destroy(gameObject);
    }

}
