using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * ������Ʈ�� ���콺�� ������� �ϴ� ��ũ��Ʈ
 * followTowerPrefab���� ��������� ����Ͽ��� ���� �ʿ����� ������
 * ���Ŀ� �ٸ� ������� �����ϰų� ���� ����
 */
public class ObjectFollowMousePosition : MonoBehaviour
{
    Camera mainCamera;

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        Vector3 position = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z);
        transform.position = mainCamera.ScreenToWorldPoint(position);
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
    }
}
