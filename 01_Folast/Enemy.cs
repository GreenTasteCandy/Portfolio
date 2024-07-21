using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * ������Ʈ�� �̵� ����� ����ϴ� ��ũ��Ʈ
 * ������ ���� �̵������ ����ϴ� ��ũ��Ʈ ������
 * ���Ŀ� ���潺�� �Ʊ� ���� ���� �̵��� �ؾ� �ϹǷ� �ش� ��ũ��Ʈ�� ���� �̵������ ������ ����
 */

//���,���� ���¸� üũ�ϴ� Ÿ��
public enum EnemyDestroyType { Kill = 0, Arrive }

public class Enemy : MonoBehaviour
{

    int wayPointCount;
    Transform[] wayPoints;
    int currentIndex = 0;
    Movement2D movement2D;
    EnemySpawn enemySpawn;
    TowerWeapon towerWeapon;
    UnitTypeData typeData;

    [SerializeField]
    int gold = 10;

    //������Ʈ�� ������ �� EnemySpawner���Լ� �ʿ��� ������ �޾ƿ�
    public void Setup(EnemySpawn enemySpawn, Transform[] wayPoint, WaveType type, UnitTypeData unit) 
    {
        this.enemySpawn = enemySpawn;

        typeData = unit;

        if (unit == UnitTypeData.Enemy)
        {
            if (type == WaveType.Defense)
            {
                movement2D = GetComponent<Movement2D>();
                wayPointCount = wayPoint.Length;
                wayPoints = new Transform[wayPointCount];
                wayPoints = wayPoint;
                transform.position = wayPoint[currentIndex].position;
                StartCoroutine("OnMove");
            }
        }

        else if (unit == UnitTypeData.Unit)
        {
            if (type == WaveType.Offense)
            {
                towerWeapon = GetComponent<TowerWeapon>();
                movement2D = GetComponent<Movement2D>();
                wayPointCount = wayPoint.Length;
                wayPoints = new Transform[wayPointCount];
                wayPoints = wayPoint;
                transform.position = wayPoint[currentIndex].position;
                StartCoroutine("OnMove");
            }
        }

        else if (unit == UnitTypeData.Hero)
        {
            if (type == WaveType.Offense)
            {
                towerWeapon = GetComponent<TowerWeapon>();
                movement2D = GetComponent<Movement2D>();
                wayPointCount = wayPoint.Length;
                wayPoints = new Transform[wayPointCount];
                wayPoints = wayPoint;
                transform.position = wayPoint[currentIndex].position;
                StartCoroutine("OnMove");
            }
        }

    }

    public IEnumerator OnMove() //������Ʈ�� �̵��� ��ǥ ���� ����
    {
        NextMoveTo();

        while (true)
        {
            //���� ��ǥ ������ ����� ��������� ��
            if (Vector3.Distance(transform.position, wayPoints[currentIndex].position) < Time.deltaTime * movement2D.MoveSpeed)
            {
                NextMoveTo();
            }

            yield return null;
         }
    }

    void NextMoveTo() //�������� �̵��� ��ǥ ���� ����
    {
        if (currentIndex < wayPointCount - 1) //���� �̵��� ��ǥ ������ ������ ��
        {
            transform.position = wayPoints[currentIndex].position;
            currentIndex++;
            Vector3 direction = (wayPoints[currentIndex].position - transform.position).normalized;
            movement2D.MoveTo(direction);
        }
        else //��ǥ ������ ���� ���� ��
        {
            //�� ĳ���� ����
            if (typeData == UnitTypeData.Enemy)
            {
                gold = 0;
                OnDie(EnemyDestroyType.Arrive);
            }
            //�Ʊ� ���� ����
            else
            {
                towerWeapon.UnitDie();
            }
        }
    }

    public void OnDie(EnemyDestroyType type) //EnemySpawner���� ������ ������ ó���ϰ� ��
    {
        enemySpawn.DestroyEnemy(type, this, gold);
    }

}
