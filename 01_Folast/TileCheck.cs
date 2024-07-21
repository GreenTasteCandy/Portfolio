using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*선택된 타일의 각종 정보를 확인하는 스크립트*/
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

        //towerSpawner를 확인하고 해당 오브젝트의 컴포넌트값을 저장한다
        towerSpawner = GameObject.Find("TowerSpawner");
        towerSpawn = towerSpawner.GetComponent<TowerSpawner>();
    }

    private void LateUpdate()
    {
        //타워 설치 준비시 설치 가능한 타일을 표시하는 기능
        if (towerSpawn.IsOnTowerBuild == true && isBuildTower == false)
            checkTile.SetActive(true);
        else
            checkTile.SetActive(false);
    }


}
