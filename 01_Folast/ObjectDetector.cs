using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/*타워나 타일을 클릭할시 작동하는 스크립트*/
public class ObjectDetector : MonoBehaviour
{
    public static bool touchScreen = false;

    [SerializeField]
    LayerMask layerObject;
    [SerializeField]
    TowerSpawner towerSpawner;
    [SerializeField]
    TowerDataViewer towerDataViewer;
    [SerializeField]
    public GameObject seletObject;

    Camera mainCamera;
    Ray ray;
    RaycastHit hit;
    Transform hitTransform = null;

    private void Awake()
    {
        //메인카메라 지정
        mainCamera = Camera.main;
    }

    void Update()
    {
        //타일이나 유닛 이외의 다른 오브젝트 선택시 종료
        if (EventSystem.current.IsPointerOverGameObject() == true)
        {
            return;
        }

        //준비 라운드에서 화면 터치시 타워 설치 기능 종료
        if (GameManager.instance.waveSystem.WaveType == WaveType.ReadyOffense || GameManager.instance.waveSystem.WaveType == WaveType.Offense)
        {
            seletObject.transform.position = new Vector3(9999, 1.1f, 9999);
            towerDataViewer.OffPanel();
            touchScreen = false;
            return;
        }

        //화면 터치시
        if (Input.GetMouseButtonDown(0))
        {
            //화면 체크용 ray 발사
            /*
             * 메인 카메라에서 화면 터치지점까지 ray를 발사하는것으로
             * 해당 위치에 어떤 오브젝트가 있는지 체크한다
             */
            ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            if (touchScreen == false)
            {
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerObject))
                {
                    hitTransform = hit.transform;

                    if (hitTransform.CompareTag("TowerTile")) //클릭한게 타일일 시
                    {
                        seletObject.transform.position = new Vector3(9999, 1.1f, 9999);
                        if (towerSpawner.IsOnTowerBuild) //타워 설치 가능한 타일이면 타워 생성
                        {
                            towerSpawner.SpawnTower(hit.transform, towerSpawner.BuildTower);
                        }
                    }
                    else if (hitTransform.CompareTag("Tower")) //클릭한 게 타워면 타워 정보창 생성
                    {
                        seletObject.transform.position = new Vector3(hit.transform.position.x, 1.1f, hit.transform.position.z);
                        towerDataViewer.OnPanel(hit.transform);
                        touchScreen = true;
                    }
                }
            }
        }

    }
}
