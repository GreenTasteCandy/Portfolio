using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ItemData : ScriptableObject
{
    public ItemStatus[] item;
}

[System.Serializable]
public class ItemStatus
{
    public Sprite icon;
    public string name;
    public string notice;
    public Status addStatus;
}