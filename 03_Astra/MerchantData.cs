using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MerchantType { Normal = 0, Special }
public enum MerchantStatus { Tradeable = 0, Untradeable }

[CreateAssetMenu]
public class MerchantData : ScriptableObject
{
    public MerchantStruct[] merchants;
}

[System.Serializable]
public class MerchantStruct
{
    public string name;
    public Sprite sprite;
    public string from;
    public int merchantOpenLevel;

    public MerchantType type;
    public MerchantStatus merchantStatus;

    public MerchantItemStruct[] merchantItems;
}

[System.Serializable]
public class MerchantItemStruct
{
    public int tradeInd;
    public int tradeCost;

    public MerchantItemStruct(int ind)
    {
        tradeInd = ind;

        int cost = Random.Range(GameSetting.ins.itemTable.items[ind].priceMin, GameSetting.ins.itemTable.items[ind].priceMax);
        tradeCost = cost;
    }
}