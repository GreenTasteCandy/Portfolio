using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*아이템마다 들어가는 템플릿*/
public enum ItemRank { Common = 0, Cursed, Epic, Legendary }

[CreateAssetMenu]
public class ItemTemplate : ScriptableObject
{
    public Item[] item;

    [System.Serializable]
    public struct Item
    {
        public ItemRank rank;

        public Sprite sprite;
        public string name;

        public int level;
        public int maxLevel;

        public float damage;
        public float hp;
        public float rate;
        public float range;
        public float speed;
        public float cooldown;
        public float unitCostDown;
        public float clearReward;
        public float destroyRock;
        public float offenBounsTime;
        public float persentReward;

        public string notice; //아이템 설명
    }

    public ItemTemplate(int num)
    {
        item = new Item[num];
    }
}