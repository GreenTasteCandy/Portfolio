using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * 오브젝트 풀링 매니저
 * 대충 오브젝트를 생성/파괴할때 무조건 새로 생성/파괴시키는게 아니라
 * 오브젝트를 파괴시킬때 비활성화 시키고,생성 시킬때도 새로 만드는게 아니라
 * 기존에 비활성화된 오브젝트를 활성화 시켜 재사용
 * 
 * 필드의 구역 시스템도 풀링 매니저 스크립트에서 동작한다
 */

public class PoolManager : MonoBehaviour
{
    [Header("Field Summon")]
    public Terrain field;
    public LayerMask checkLayer;
    public Collider[] area;

    [Header("prefabs")]
    public GameObject[] prefabs;
    public GameObject[] prefabsField;

    List<GameObject>[] pools;
    List<GameObject>[] poolsField;

    RaycastHit hit;

    private void Awake()
    {
        //초기 오브젝트 리스트 설정
        pools = new List<GameObject>[prefabs.Length];
        poolsField = new List<GameObject>[prefabsField.Length];

        for (int i = 0; i < pools.Length; i++)
        {
            pools[i] = new List<GameObject>();
        }

        for (int i = 0; i < poolsField.Length; i++)
        {
            poolsField[i] = new List<GameObject>();
        }
    }

    private void Start()
    {
        foreach (Collider collider in area)
        {
            int objLenght = collider.gameObject.GetComponent<AreaData>().obj.Length;

            for (int ind = 0; ind < objLenght; ind++)
            {
                int obj = collider.gameObject.GetComponent<AreaData>().obj[ind];
                int objNum = collider.gameObject.GetComponent<AreaData>().objNum[ind];

                while (objNum > 0)
                {
                    Vector3 newPos = CreatePosition(collider);
                    Collider[] check = Physics.OverlapSphere(newPos, 1.5f, checkLayer);

                    if (check.Length == 0)
                    {
                        Transform Envir = GetPoolObjects(obj).transform;
                        Envir.rotation = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);
                        Envir.position = newPos;
                        objNum--;
                    }
                }
            }

            print("area create");
            collider.gameObject.SetActive(false);
        }

        StartCoroutine(PoolOptimizer());
    }

    Vector3 CreatePosition(Collider collider)
    {
        float rangeX = collider.bounds.size.x;
        float rangeZ = collider.bounds.size.z;

        rangeX = Random.Range((rangeX / 2) * -1, rangeX / 2) + collider.transform.position.x;
        rangeZ = Random.Range((rangeZ / 2) * -1, rangeZ / 2) + collider.transform.position.z;

        Vector3 randPos = new Vector3(rangeX, 0f, rangeZ);

        float terrainY = field.SampleHeight(randPos);
        Vector3 newPos = new Vector3(rangeX, terrainY, rangeZ);

        return newPos;
    }

    IEnumerator PoolOptimizer()
    {
        while (true)
        {
            Transform[] poolObj = gameObject.GetComponentsInChildren<Transform>(true);
            foreach (Transform obj in poolObj)
            {
                if (obj.gameObject.name == gameObject.name)
                    continue;

                if (Vector3.Distance(obj.position, GameManager.ins.player.transform.position) > 250f)
                {
                    obj.gameObject.SetActive(false);
                }
                else
                {
                    obj.gameObject.SetActive(true);
                }

            }

            yield return new WaitForSeconds(3f);
        }
    }

    //오브젝트 생성
    public GameObject GetPool(int index)
    {
        GameObject select = null;

        foreach (GameObject item in pools[index])
        {
            if (!item.activeSelf)
            {
                select = item;
                select.SetActive(true);
                break;
            }
        }

        if (!select)
        {
            select = Instantiate(prefabs[index], transform);
            pools[index].Add(select);
        }

        return select;
    }

    //필드 오브젝트 생성
    public GameObject GetPoolObjects(int index)
    {
        GameObject select = null;

        foreach (GameObject item in poolsField[index])
        {
            if (!item.activeSelf)
            {
                select = item;
                select.SetActive(true);
                break;
            }
        }

        if (!select)
        {
            select = Instantiate(prefabsField[index], transform);
            poolsField[index].Add(select);
        }

        return select;
    }
}
