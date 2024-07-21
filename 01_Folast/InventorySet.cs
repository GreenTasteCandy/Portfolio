using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * ������ �κ��丮 ������ ���� ��ũ��Ʈ
 */
public class InventorySet : MonoBehaviour
{
    public static InventorySet instance;

    [SerializeField]
    ItemTemplate invenSlot;
    [SerializeField]
    ItemTemplate invenStatus;

    public ItemTemplate itemSlot => invenSlot;
    public ItemTemplate itemStatus => invenStatus;
    public float ItemDamage => itemStatus.item[0].damage + itemStatus.item[1].damage + itemStatus.item[2].damage + itemStatus.item[3].damage;
    public float ItemRate => itemStatus.item[0].rate + itemStatus.item[1].rate + itemStatus.item[2].rate + itemStatus.item[3].rate;
    public float ItemRange => itemStatus.item[0].range + itemStatus.item[1].range + itemStatus.item[2].range + itemStatus.item[3].range;
    public float ItemMaxHP => itemStatus.item[0].hp + itemStatus.item[1].hp + itemStatus.item[2].hp + itemStatus.item[3].hp;
    public float ItemSpeed => itemStatus.item[0].speed + itemStatus.item[1].speed + itemStatus.item[2].speed + itemStatus.item[3].speed;
    public float ItemCoolDown => itemStatus.item[0].cooldown + itemStatus.item[1].cooldown + itemStatus.item[2].cooldown + itemStatus.item[3].cooldown;
    public float UnitCostDown => itemStatus.item[0].unitCostDown + itemStatus.item[1].unitCostDown + itemStatus.item[2].unitCostDown + itemStatus.item[3].unitCostDown;
    public float ClearReward => itemStatus.item[0].clearReward + itemStatus.item[1].clearReward + itemStatus.item[2].clearReward + itemStatus.item[3].clearReward;
    public float DestroyRock => itemStatus.item[0].destroyRock + itemStatus.item[1].destroyRock + itemStatus.item[2].destroyRock + itemStatus.item[3].destroyRock;
    public float OffenceBounsTime => itemStatus.item[0].offenBounsTime + itemStatus.item[1].offenBounsTime + itemStatus.item[2].offenBounsTime + itemStatus.item[3].offenBounsTime;
    public float PercentReward => itemStatus.item[0].persentReward + itemStatus.item[1].persentReward + itemStatus.item[2].persentReward + itemStatus.item[3].persentReward;

    public void Awake()
    {
        instance = this;
    }

    public void InvenReset()
    {
        invenSlot = new ItemTemplate(30);

        for (int num = 0; num < 4; num++)
        {
            itemStatus.item[num].hp = 0;
            itemStatus.item[num].level = 0;
            itemStatus.item[num].offenBounsTime = 0;
            itemStatus.item[num].persentReward = 0;
            itemStatus.item[num].range = 0;
            itemStatus.item[num].rate = 0;
            itemStatus.item[num].speed = 0;
            itemStatus.item[num].unitCostDown = 0;
            itemStatus.item[num].damage = 0;
            itemStatus.item[num].cooldown = 0;
            itemStatus.item[num].clearReward = 0;
        }
    }

    /*
     * ������ �߰� ȿ�� �ɷ�ġ
     * 0 - ���� ��� ����
     * 1 - ���� Ŭ����� �߰� ����
     * 2 - ��ֹ� �ı��� �߰� ����
     * 3 - ���潺 ���ѽð� �߰� ����
     * 4 - ���� Ŭ����� ���� ������ 00% �߰� ����
     */

}
