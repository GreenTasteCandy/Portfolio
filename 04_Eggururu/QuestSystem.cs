using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuestSystem : MonoBehaviour
{
    public Slider questPrograss;
    public Sprite[] icons;
    public Image[] prograssIcon;
    public GameObject questList;
    public GameObject questSlot;
    public List<GameObject> questSlots;

    private void LateUpdate()
    {
        if (UserData.ins.data.isQuestComplete != -1)
            questPrograss.value = UserData.ins.data.isQuestComplete / 100f;
        else
            questPrograss.value = 1;
    }

    private void OnEnable()
    {
        QuestShow();
    }

    public void QuestShow()
    {
        int num = 0;

        foreach(QuestData data in DataTable.ins.questList)
        {
            if ((num+1) > (questSlots.Count - 1))
            {
                GameObject slot = Instantiate(questSlot, questList.transform.position, Quaternion.identity);
                slot.transform.parent = questList.transform;
                slot.GetComponent<QuestShow>().init(data.name, data.docs, UserData.ins.data.questPrograss[num], data.cost, data.reward);
                questSlots.Add(slot);
            }
            else
            {
                questSlots[num].SetActive(true);
                questSlots[num].GetComponent<QuestShow>().init(data.name, data.docs, UserData.ins.data.questPrograss[num], data.cost, data.reward);
            }

            num += 1;
        }
    }

    public void QuestClose()
    {
        foreach (GameObject obj in questSlots)
        {
            obj.SetActive(false);
        }
    }
}
