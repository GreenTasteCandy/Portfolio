using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*�̵��� ��ũ��Ʈ*/
public class Movement2D : MonoBehaviour
{

    [SerializeField]
    float moveSpeed = 0.0f;
    [SerializeField]
    Vector3 moveDirection = Vector3.zero;
    public bool isMove = true; //�̵� ������ üũ��

    float speedDebuff;

    public float MoveSpeed => moveSpeed;

    private void Start()
    {
        if (gameObject.CompareTag("Enemy") || gameObject.CompareTag("Tower"))
        {
            float speed = GetComponent<TowerWeapon>().Speed;
            moveSpeed = speed;
        }
    }

    void Update()
    {
        if (isMove)
            transform.position += moveDirection * (moveSpeed - speedDebuff) * Time.deltaTime;
    }

    public void MoveDebuff(float skillTime, float debuff)
    {
        StartCoroutine(DebuffSpeedDown(skillTime, debuff));
    }

    IEnumerator DebuffSpeedDown(float skillTime, float debuff)
    {
        float skillRate = skillTime;
        float skillAct = debuff;
        float debuffTime = 0f;

        while(debuffTime < skillRate)
        {
            debuffTime += Time.deltaTime;
            speedDebuff = moveSpeed * skillAct;

            yield return new WaitForSeconds(Time.deltaTime);
        }

        speedDebuff = 0f;
        StopCoroutine("DebuffSpeedDown");
    }

    public void MoveTo(Vector3 direction) //�̵� ���� ����
    {
        moveDirection = direction;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
            isMove = false;
    }

}
