using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;

public class RankShow : MonoBehaviour
{
    public bool isPersonal;
    public List<GameObject> gameObjects = new List<GameObject>();
    public GameObject rankList;
    public GameObject rank;
    public GameObject rankPlayer;
    public TextMeshProUGUI textRank;
    public bool isRank = true;

    private void OnEnable() { ShowRank(); }
    public void ShowRank()
    {
        if (isPersonal)
        {
            RankSet(UserData.ins.data.personalRank);
        }
        else
        {
            if (isRank == true)
            {
                textRank.text = "클래식모드 기록";
                DatabaseManager.ins.LoadRank("totalRankClassic");
                RankSet(DatabaseManager.ins.dbDict2);
            }
            else if (isRank == false)
            {
                textRank.text = "캐주얼모드 기록";
                DatabaseManager.ins.LoadRank("totalRank");
                RankSet(DatabaseManager.ins.dbDict);
            }
        }
    }


    public void RankSet(Dictionary<string, int> ranklist)
    {
        int num = isPersonal ? 20 : 100;
        int i = 1;
        if (ranklist.Count < num)
            num = ranklist.Count;

        var sortedDict = ranklist.OrderByDescending(x => x.Value);

        foreach (KeyValuePair<string, int> pair in sortedDict)
        {
            if (pair.Key == FirebaseAuthManager.ins.userID)
            {
                rankPlayer.GetComponent<RankingList>().Init(i.ToString(), pair.Key, pair.Value.ToString());
            }

            if (i <= num)
            {
                if (gameObjects.Count < i)
                {
                    GameObject obj = Instantiate(rank, rankList.transform.position, Quaternion.identity);
                    obj.transform.parent = rankList.transform;
                    obj.GetComponent<RankingList>().Init(i.ToString(), pair.Key, pair.Value.ToString());
                    gameObjects.Add(obj);
                }
                else
                {
                    gameObjects[i - 1].SetActive(true);
                    gameObjects[i - 1].GetComponent<RankingList>().Init(i.ToString(), pair.Key, pair.Value.ToString());
                }

                i += 1;
            }
        }
    }

    public void ListOff()
    {
        foreach (GameObject obj in gameObjects)
        {
            obj.SetActive(false);
        }
    }
}
