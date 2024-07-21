using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*���õ� Ÿ���� ���� ������ Ȯ���ϴ� ��ũ��Ʈ*/
public class TileCheck : MonoBehaviour
{
    [SerializeField]
    GameObject checkTile;
    GameObject towerSpawner;
    TowerSpawner towerSpawn;

    public bool isBuildTower { set; get; }

    private void Awake()
    {
        isBuildTower = false;

        //towerSpawner�� Ȯ���ϰ� �ش� ������Ʈ�� ������Ʈ���� �����Ѵ�
        towerSpawner = GameObject.Find("TowerSpawner");
        towerSpawn = towerSpawner.GetComponent<TowerSpawner>();
    }

    private void LateUpdate()
    {
        //Ÿ�� ��ġ �غ�� ��ġ ������ Ÿ���� ǥ���ϴ� ���
        if (towerSpawn.IsOnTowerBuild == true && isBuildTower == false)
            checkTile.SetActive(true);
        else
            checkTile.SetActive(false);
    }


}
