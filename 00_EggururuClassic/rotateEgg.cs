using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//rotateEgg�� ����ȭ�鿡�� ȸ���ϴ� ĳ���� ����ŷ�Դϴ�

public class rotateEgg : MonoBehaviour
{
    float rotSpeed = 100.0f;

    // Start is called before the first frame update
    void Start()
    {
        transform.localEulerAngles = new Vector3(0, 0, 60);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(1,1,0) * Time.deltaTime * rotSpeed);
    }
}
