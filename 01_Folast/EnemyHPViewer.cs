using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*ü�� �ٸ� ��Ÿ���� ��ũ��Ʈ
 *�� ���� ������ ü�� �ٵ� ���⼭ ó��*/
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
