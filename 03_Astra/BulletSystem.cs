using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * 투사체 스크립트
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

    //초기 데이터 설정
    public void Setup(float range)
    {
        this.range = range;
    }

    //충돌 감지
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
        //사거리
        delay += Time.deltaTime;

        if (delay >= range)
        {
            DeadBullet();
            delay = 0;
        }
    }

    //파괴시
    void DeadBullet()
    {
        if (trail != null)
            trail.Clear();

        gameObject.SetActive(false);
    }
}
