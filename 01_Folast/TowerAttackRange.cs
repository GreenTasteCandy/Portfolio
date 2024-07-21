using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*Ÿ���� ���� ������ �ð������� �����ֱ� ���� ��ũ��Ʈ*/
public class TowerAttackRange : MonoBehaviour
{
    /*
     * ������ ���¿� ������ awake �Լ��� ���� ��ó�� ������ Ÿ���� ���ݹ����� ������ �ʴ� ���� ����
     * ������ ������ ������ �ش� ���װ� �߻����� �����Ƿ� �������� �ʰ� ����
     * ���� ���� �߻��� awake �Լ� �����Ұ�
     */
    private void Awake()
    {
        OffAttackRange();
    }

    public void OnAttackRange(Vector3 position,float range) //���� ������ ���̰� ��
    {
        gameObject.SetActive(true);

        float diameter = range * 2.0f;
        transform.localScale = Vector3.one * diameter;
        transform.position = position;
    }

    public void OffAttackRange() //���� ������ �� ���̰� ��
    {
        gameObject.SetActive(false);
    }
}
