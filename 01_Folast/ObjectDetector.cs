using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/*Ÿ���� Ÿ���� Ŭ���ҽ� �۵��ϴ� ��ũ��Ʈ*/
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
        //����ī�޶� ����
        mainCamera = Camera.main;
    }

    void Update()
    {
        //Ÿ���̳� ���� �̿��� �ٸ� ������Ʈ ���ý� ����
        if (EventSystem.current.IsPointerOverGameObject() == true)
        {
            return;
        }

        //�غ� ���忡�� ȭ�� ��ġ�� Ÿ�� ��ġ ��� ����
        if (GameManager.instance.waveSystem.WaveType == WaveType.ReadyOffense || GameManager.instance.waveSystem.WaveType == WaveType.Offense)
        {
            seletObject.transform.position = new Vector3(9999, 1.1f, 9999);
            towerDataViewer.OffPanel();
            touchScreen = false;
            return;
        }

        //ȭ�� ��ġ��
        if (Input.GetMouseButtonDown(0))
        {
            //ȭ�� üũ�� ray �߻�
            /*
             * ���� ī�޶󿡼� ȭ�� ��ġ�������� ray�� �߻��ϴ°�����
             * �ش� ��ġ�� � ������Ʈ�� �ִ��� üũ�Ѵ�
             */
            ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            if (touchScreen == false)
            {
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerObject))
                {
                    hitTransform = hit.transform;

                    if (hitTransform.CompareTag("TowerTile")) //Ŭ���Ѱ� Ÿ���� ��
                    {
                        seletObject.transform.position = new Vector3(9999, 1.1f, 9999);
                        if (towerSpawner.IsOnTowerBuild) //Ÿ�� ��ġ ������ Ÿ���̸� Ÿ�� ����
                        {
                            towerSpawner.SpawnTower(hit.transform, towerSpawner.BuildTower);
                        }
                    }
                    else if (hitTransform.CompareTag("Tower")) //Ŭ���� �� Ÿ���� Ÿ�� ����â ����
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
