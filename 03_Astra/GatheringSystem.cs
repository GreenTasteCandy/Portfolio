using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GatheringSystem : MonoBehaviour
{
    public bool buttonOn;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("GatherObj"))
        {
            buttonOn = true;
            ObjectSystem obj = other.gameObject.GetComponent<ObjectSystem>();
            obj.RandomItem();

            GameManager.ins.inven.GetItem(GameManager.ins.getitem);
        }
    }

}
