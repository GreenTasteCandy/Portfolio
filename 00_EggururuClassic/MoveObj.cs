using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//MoveObj�� ������Ʈ(��ֹ� �� ���)���� �̵� �ý����Դϴ�

public class MoveObj : MonoBehaviour
{

    //���� �ʱ�ȭ �κ�
    bool moveC = false;
    Vector3 locateObj;

    public int scoreCheck = 0;
    GameObject player;
    public GameObject checkDestroy;

    public Vector3 playerLocate;
    public bool rotate = true;

    void Awake()
    {
        //�÷��̾� ����
        player = GameObject.Find("egg");
        playerLocate = player.transform.position;
    }

    private void Start()
    {
        //������ ������Ʈ ȸ��
        if (rotate == true)
            transform.rotation = Quaternion.Euler(0, Random.Range(0, 359), 0);
    }

    void Update()
    {
        //�÷��̾� �̵��ϰ� �ִ��� ����
        if (playerLocate != player.transform.position && moveC == false)
        {
            StartCoroutine(MoveObjectTranslate(new Vector3(0, 0, -5), 0.1f));
        }

        //ĳ�־��� : ��üȭ ����
        if (StartGame.seletMode == 1 && StartGame.seletSkill == 3)
        {
            //�÷��̾� �̵��ϰ� �ִ��� ����
            if (moveC == false && player.GetComponent<SphereCollider>().enabled == false)
            {
                StartCoroutine(MoveObjectTranslate(new Vector3(0, 0, -5), 0.05f));
            }
        }

        //ȭ�� ������ ����� �ı�
        locateObj = transform.position;
        if (locateObj.z < -15)
        {
            Destroy(gameObject);
        }
    }

    //��ֹ� �̵��� ���
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