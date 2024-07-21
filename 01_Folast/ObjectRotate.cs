using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectRotate : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        float rand = Random.Range(0, 360);
        transform.Rotate(0, rand, 0);
    }

}
