using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * 아이템 테이블 스크립트
 * 일단 대충 필요한 변수들 위주로 만들어 놓긴 했으나
 * 시스템 제작하면서 필요한게 있으면 알잘딱해서 추가할것
 */

public enum ItemType { Ore = 0, Life, Energy, Relic, Plant }
public enum ItemRank { Normal = 0, Rare, Epic, Legendary, Special }

[CreateAssetMenu]
public class ItemData : ScriptableObject
{
    public itemStruct[] items;
}

[System.Serializable]
public class itemStruct
{
    public string name;
    public Sprite sprite;
    public AppearingPlanet appearingPlanet;
    [TextArea]
    public string desc;

    public ItemType type;
    public ItemRank rank;
    public int count;

    public int price; //현재 가격
    public int pricePast; //이전 가격
    public int priceMax; //가격 최대치
    public int priceMin; //가격 최소치
    public int priceAverage; //가격 평균치

    public itemStruct(itemStruct item)
    {
        name = item.name;
        sprite = item.sprite;
        type = item.type;
        rank = item.rank;
        price = item.price;
        priceMax = item.priceMax;
        priceMin = item.priceMin;
    }
}
