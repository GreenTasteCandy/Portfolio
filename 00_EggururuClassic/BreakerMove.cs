using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//BreakerMove는 길 생성 오브젝트의 시스템입니다

public class BreakerMove : MonoBehaviour
{

    //변수 초기화 부분
    bool moveC = false;
    GameObject player;
    public Vector3 playerLocate;

    Vector3 locateObj;

    void Awake()
    {
        //플레이어 감지
        player = GameObject.Find("egg");
        playerLocate = player.transform.position;
    }

    void Update()
    {
        //플레이어 이동하고 있는지 감지
        if (playerLocate != player.transform.position && moveC == false)
        {
            StartCoroutine(MoveObjectTranslate(new Vector3(0, 0, -5), 0.1f));
        }

        //캐주얼모드 : 유체화 사용시
        if (StartGame.seletMode == 1 && StartGame.seletSkill == 3 && moveC == false)
        {
            if (player.GetComponent<SphereCollider>().enabled == false)
            {
                StartCoroutine(MoveObjectTranslate(new Vector3(0, 0, -10), 0.1f));
            }
        }

        //화면 밖으로 벗어날시 파괴
        locateObj = transform.position;
        if (locateObj.z < -15)
        {
            Destroy(gameObject);
        }
    }

    //장애물과 부딫힐시 장애물 파괴
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Wall")
        {
            Destroy(other.gameObject);
        }
    }

    //이동 모션
    IEnumerator MoveObjectTranslate(Vector3 dir,float delay)
    {
        moveC = true;

        float elapsedTime = 0.0f;
        Vector3 currentPosition = transform.position;
        Vector3 targetPosition = transform.position + dir;

        while (elapsedTime < delay)
        {
            transform.position = Vector3.Lerp(currentPosition, targetPosition, elapsedTime / delay);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition;

        moveC = false;
        playerLocate = player.transform.position;
    }
}
