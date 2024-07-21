using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RankingList : MonoBehaviour
{
    public TextMeshProUGUI textRank;
    public TextMeshProUGUI textScore;
    public TextMeshProUGUI textDocs;

    public void Init(string rank, string docs, string score)
    {
        textRank.text = rank;
        textScore.text = score;
        textDocs.text = docs;

    }
}
