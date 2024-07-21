using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//shieldEffectMove는 보호막 스킬 작동시 생성되는 보호막 오브젝트의 기능입니다

public class shieldEffectMove : MonoBehaviour
{
    //변수 초기화 부분
    GameObject player;
    public Vector3 playerLocate;

    void Awake()
    {
        //플레이어 감지
        player = GameObject.Find("egg");
        playerLocate = player.transform.position;
    }

    //캐릭터 따라가기
    void Update()
    {
        transform.position = player.transform.position;

        if (Move.shieldCheck == false)
            Destroy(gameObject);
    }

    //장애물과 부딫힐시 파괴
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Wall")
        {
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }

}
