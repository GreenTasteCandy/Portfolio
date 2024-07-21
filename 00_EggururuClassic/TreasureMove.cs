using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureMove : MonoBehaviour
{
    //���� �ʱ�ȭ �κ�
    bool moveC = false;
    GameObject player;
    public Vector3 playerLocate;

    void Awake()
    {
        //�÷��̾� ����
        player = GameObject.Find("egg");
        playerLocate = player.transform.position;
    }

    void Update()
    {
        //�÷��̾� �̵��ϰ� �ִ��� ����
        if (playerLocate != player.transform.position && moveC == false)
        {
            StartCoroutine(MoveObjectTranslate(new Vector3(0, 0, -5), 0.1f));
        }

    }

    void OnTriggerEnter(Collider target)
    {
        Destroy(gameObject);
    }
    
    //�̵� ���
    IEnumerator MoveObjectTranslate(Vector3 dir, float delay)
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
