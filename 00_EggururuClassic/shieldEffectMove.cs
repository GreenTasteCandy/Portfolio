using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//shieldEffectMove�� ��ȣ�� ��ų �۵��� �����Ǵ� ��ȣ�� ������Ʈ�� ����Դϴ�

public class shieldEffectMove : MonoBehaviour
{
    //���� �ʱ�ȭ �κ�
    GameObject player;
    public Vector3 playerLocate;

    void Awake()
    {
        //�÷��̾� ����
        player = GameObject.Find("egg");
        playerLocate = player.transform.position;
    }

    //ĳ���� ���󰡱�
    void Update()
    {
        transform.position = player.transform.position;

        if (Move.shieldCheck == false)
            Destroy(gameObject);
    }

    //��ֹ��� �΋H���� �ı�
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Wall")
        {
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }

}
