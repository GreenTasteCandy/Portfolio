using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Experimental.AI;

public class QuestShow : MonoBehaviour
{
    public TextMeshProUGUI textName;
    public TextMeshProUGUI textDocs;
    public TextMeshProUGUI textPrograss;
    public TextMeshProUGUI textReward;

    public void init(string name, string docs, int prograss, int clear, int reward)
    {
        textName.text = name;
        textDocs.text = docs;
        textReward.text = reward.ToString();

        if (prograss == -1)
        {
            textPrograss.text = "¿Ï·á";
        }
        else
        {
            textPrograss.text = prograss.ToString() + "/" + clear.ToString();
        }
    }
}
