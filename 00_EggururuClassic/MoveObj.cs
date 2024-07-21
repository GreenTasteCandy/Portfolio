using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//MoveObj는 오브젝트(장애물 및 장식)들의 이동 시스템입니다

public class MoveObj : MonoBehaviour
{

    //변수 초기화 부분
    bool moveC = false;
    Vector3 locateObj;

    public int scoreCheck = 0;
    GameObject player;
    public GameObject checkDestroy;

    public Vector3 playerLocate;
    public bool rotate = true;

    void Awake()
    {
        //플레이어 감지
        player = GameObject.Find("egg");
        playerLocate = player.transform.position;
    }

    private void Start()
    {
        //생성시 오브젝트 회전
        if (rotate == true)
            transform.rotation = Quaternion.Euler(0, Random.Range(0, 359), 0);
    }

    void Update()
    {
        //플레이어 이동하고 있는지 감지
        if (playerLocate != player.transform.position && moveC == false)
        {
            StartCoroutine(MoveObjectTranslate(new Vector3(0, 0, -5), 0.1f));
        }

        //캐주얼모드 : 유체화 사용시
        if (StartGame.seletMode == 1 && StartGame.seletSkill == 3)
        {
            //플레이어 이동하고 있는지 감지
            if (moveC == false && player.GetComponent<SphereCollider>().enabled == false)
            {
                StartCoroutine(MoveObjectTranslate(new Vector3(0, 0, -5), 0.05f));
            }
        }

        //화면 밖으로 벗어날시 파괴
        locateObj = transform.position;
        if (locateObj.z < -15)
        {
            Destroy(gameObject);
        }
    }

    //장애물 이동시 모션
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