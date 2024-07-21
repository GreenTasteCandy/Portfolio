using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public GameObject PlayerEgg;
    public Vector3 offset;

    // Update is called once per frame
    void Update()
    {
        transform.position = PlayerEgg.transform.position + offset;
    }
}
