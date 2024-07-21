using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * ������Ʈ Ǯ�� �Ŵ���
 * ���� ������Ʈ�� ����/�ı��Ҷ� ������ ���� ����/�ı���Ű�°� �ƴ϶�
 * ������Ʈ�� �ı���ų�� ��Ȱ��ȭ ��Ű��,���� ��ų���� ���� ����°� �ƴ϶�
 * ������ ��Ȱ��ȭ�� ������Ʈ�� Ȱ��ȭ ���� ����
 * 
 * �ʵ��� ���� �ý��۵� Ǯ�� �Ŵ��� ��ũ��Ʈ���� �����Ѵ�
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
        //�ʱ� ������Ʈ ����Ʈ ����
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

    //������Ʈ ����
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

    //�ʵ� ������Ʈ ����
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
