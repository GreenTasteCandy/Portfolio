using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * ������ ���̺� ��ũ��Ʈ
 * �ϴ� ���� �ʿ��� ������ ���ַ� ����� ���� ������
 * �ý��� �����ϸ鼭 �ʿ��Ѱ� ������ ���ߵ��ؼ� �߰��Ұ�
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

    public int price; //���� ����
    public int pricePast; //���� ����
    public int priceMax; //���� �ִ�ġ
    public int priceMin; //���� �ּ�ġ
    public int priceAverage; //���� ���ġ

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
