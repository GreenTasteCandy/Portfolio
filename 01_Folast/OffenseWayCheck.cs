using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OffenseWayCheck : MonoBehaviour
{
    [SerializeField]
    TowerSpawner towerSpawner;
    [SerializeField]
    int pathNum;

    Image imageButton;
    private void Start()
    {
        imageButton = GetComponent<Image>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (towerSpawner.PathNum == pathNum)
            imageButton.color = Color.red;
        else
            imageButton.color = Color.white;
    }
}
