using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*체력 바를 나타내는 스크립트
 *적 말고 유닛의 체력 바도 여기서 처리*/
public class EnemyHPViewer : MonoBehaviour
{
    EnemyHP enemyHP;
    Slider hpSlider;

    public void Setup(EnemyHP enemyHP)
    {
        this.enemyHP = enemyHP;
        hpSlider = GetComponent<Slider>();
    }

    private void Update()
    {
        hpSlider.value = enemyHP.CurrentHP / enemyHP.MaxHP;
    }
}
