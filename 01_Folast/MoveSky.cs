using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveSky : MonoBehaviour
{
    [SerializeField]
    GameObject sky;
    [SerializeField]
    float skySpeed;

    // Update is called once per frame
    void Update()
    {
        sky.transform.Rotate(new Vector3(0, skySpeed, 0));
    }
}
