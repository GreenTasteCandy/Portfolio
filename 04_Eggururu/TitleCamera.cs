using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleCamera : MonoBehaviour
{
    public float rotateSpeed = 5f;
    public Transform target;

    private void Start()
    {
        transform.rotation = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);
    }

    void Update()
    {
        transform.RotateAround(target.position, Vector3.up, rotateSpeed * Time.deltaTime);
    }
}
