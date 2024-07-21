using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*타워의 공격 범위를 시각적으로 보여주기 위한 스크립트*/
public class TowerAttackRange : MonoBehaviour
{
    /*
     * 동영상 강좌에 따르면 awake 함수로 인해 맨처음 선택한 타워의 공격범위가 나오지 않는 버그 존재
     * 하지만 실제로 실행결과 해당 버그가 발생되지 않으므로 삭제하지 않고 냅둠
     * 관련 버그 발생시 awake 함수 삭제할것
     */
    private void Awake()
    {
        OffAttackRange();
    }

    public void OnAttackRange(Vector3 position,float range) //공격 범위를 보이게 함
    {
        gameObject.SetActive(true);

        float diameter = range * 2.0f;
        transform.localScale = Vector3.one * diameter;
        transform.position = position;
    }

    public void OffAttackRange() //공격 범위를 안 보이게 함
    {
        gameObject.SetActive(false);
    }
}
