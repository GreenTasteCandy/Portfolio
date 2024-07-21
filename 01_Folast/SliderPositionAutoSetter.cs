using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * 체력 바의 위치를 자동으로 설정해주는 스크립트
 */
public class SliderPositionAutoSetter : MonoBehaviour
{
    Vector3 distance = Vector3.down * 20.0f;
    Transform targetTransform;
    RectTransform rectTransform;

    public void Setup(Transform target)
    {
        targetTransform = target;
        rectTransform = GetComponent<RectTransform>();
    }

    private void LateUpdate()
    {

        if (targetTransform == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 screenPosition = Camera.main.WorldToScreenPoint(targetTransform.position);
        rectTransform.position = screenPosition + distance;
    }
}
