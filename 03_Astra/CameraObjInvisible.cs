using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * 플레이어와 카메라 사이에 오브젝트가 있을 때
 * 그 오브젝트를 체크하고 반투명하게 바꾸는 스크립트
 * 
 * 출처 : https://daekyoulibrary.tistory.com/entry/Charon-8-%ED%94%8C%EB%A0%88%EC%9D%B4%EC%96%B4%EC%99%80-%EC%B9%B4%EB%A9%94%EB%9D%BC-%EC%82%AC%EC%9D%B4%EC%9D%98-%EC%98%A4%EB%B8%8C%EC%A0%9D%ED%8A%B8-%ED%88%AC%EB%AA%85%ED%99%94%ED%95%98%EA%B8%B0
 */

public class CameraObjInvisible : MonoBehaviour
{
    private void LateUpdate()
    {
        Vector3 direction = GameManager.ins.player.transform.position - transform.position;
        RaycastHit[] hits = Physics.RaycastAll(transform.position, direction, Mathf.Infinity, 1 << LayerMask.NameToLayer("Environments"));
    
        for (int i = 0; i < hits.Length; i++)
        {
            TransparentObject[] obj = hits[i].transform.GetComponents<TransparentObject>();

            for (int j = 0; j < obj.Length; j++)
            {
                obj[j]?.BecomeTransparent();
            }
        }
    }
}
