using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * 오브젝트의 이동 기능을 담당하는 스크립트
 * 원래는 적의 이동기능을 담당하는 스크립트 였으나
 * 추후에 오펜스시 아군 유닛 역시 이동을 해야 하므로 해당 스크립트를 통해 이동기능을 구현할 예정
 */

//사망,생존 상태를 체크하는 타입
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

    //오브젝트가 생성될 때 EnemySpawner에게서 필요한 값들을 받아옴
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

    public IEnumerator OnMove() //오브젝트의 이동과 목표 지점 설정
    {
        NextMoveTo();

        while (true)
        {
            //다음 목표 지점과 충분히 가까워졌을 때
            if (Vector3.Distance(transform.position, wayPoints[currentIndex].position) < Time.deltaTime * movement2D.MoveSpeed)
            {
                NextMoveTo();
            }

            yield return null;
         }
    }

    void NextMoveTo() //다음으로 이동할 목표 지점 설정
    {
        if (currentIndex < wayPointCount - 1) //아직 이동할 목표 지점이 남았을 시
        {
            transform.position = wayPoints[currentIndex].position;
            currentIndex++;
            Vector3 direction = (wayPoints[currentIndex].position - transform.position).normalized;
            movement2D.MoveTo(direction);
        }
        else //목표 지점의 끝에 도달 시
        {
            //적 캐릭터 도달
            if (typeData == UnitTypeData.Enemy)
            {
                gold = 0;
                OnDie(EnemyDestroyType.Arrive);
            }
            //아군 유닛 도달
            else
            {
                towerWeapon.UnitDie();
            }
        }
    }

    public void OnDie(EnemyDestroyType type) //EnemySpawner에서 본인의 삭제를 처리하게 함
    {
        enemySpawn.DestroyEnemy(type, this, gold);
    }

}
