using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * ����ü ��ũ��Ʈ
 */
public class BulletSystem : MonoBehaviour
{
    [SerializeField]
    TrailRenderer trail;
    public bool isStun;
    [SerializeField]
    int penet;
    [SerializeField]
    GameObject effect;

    float range;
    float delay;

    //�ʱ� ������ ����
    public void Setup(float range)
    {
        this.range = range;
    }

    //�浹 ����
    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.CompareTag("Enemy"))
            penet--;

        if (other.gameObject.layer != 8 && effect != null)
            StartCoroutine(EffectOn());

        if (penet <= 0)
        {
            DeadBullet();
        }

    }

    IEnumerator EffectOn()
    {
        effect.SetActive(true);

        yield return new WaitForSeconds(1f);

        effect.SetActive(false);
        DeadBullet();
    }
    
    void Update()
    {
        //��Ÿ�
        delay += Time.deltaTime;

        if (delay >= range)
        {
            DeadBullet();
            delay = 0;
        }
    }

    //�ı���
    void DeadBullet()
    {
        if (trail != null)
            trail.Clear();

        gameObject.SetActive(false);
    }
}
