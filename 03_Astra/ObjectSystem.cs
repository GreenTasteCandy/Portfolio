using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSystem : MonoBehaviour
{
    public ItemData objectItem;

    bool buttonOn;

    public void RandomItem()
    {
        int i = Random.Range(0, objectItem.items.Length);

        GameManager.ins.getitem = objectItem.items[i];
    }
}
