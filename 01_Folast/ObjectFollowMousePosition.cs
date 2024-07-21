using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * 오브젝트가 마우스를 따라오게 하는 스크립트
 * followTowerPrefab에서 사용하지만 모바일에선 딱히 필요하진 않을듯
 * 추후에 다른 기능으로 수정하거나 없앨 예정
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
