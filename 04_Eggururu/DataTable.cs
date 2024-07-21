using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;

public class DataTable : MonoBehaviour
{
    //스크립트 싱글톤 선언
    public static DataTable ins;

    const string tableLink = "https://docs.google.com/spreadsheets/d/1s3Sj6WqBqisNcia4-wWC9lcJXmmd_pl6Meeh4DJgjIY/export?format=csv&gid=927485722&range=A2:G7";
    const string tableLink2 = "https://docs.google.com/spreadsheets/d/1s3Sj6WqBqisNcia4-wWC9lcJXmmd_pl6Meeh4DJgjIY/export?format=csv&gid=0&range=A2:D21";
    const string tableLink3 = "https://docs.google.com/spreadsheets/d/1s3Sj6WqBqisNcia4-wWC9lcJXmmd_pl6Meeh4DJgjIY/export?format=csv&gid=1811481469&range=A2:F13";
    const string tableLink4 = "https://docs.google.com/spreadsheets/d/1s3Sj6WqBqisNcia4-wWC9lcJXmmd_pl6Meeh4DJgjIY/export?format=csv&gid=1749278220&range=A2:D11";

    public TextAsset[] tables;

    public List<ShopSkins> skinList;
    public List<QuestData> questList;
    public List<Ability> abilityTable;
    public List<BirdWear> birdWear;

    private void Awake()
    {
        ins = this;

        skinList = new List<ShopSkins>();
        questList = new List<QuestData>();
        birdWear = new List<BirdWear>();
        abilityTable = new List<Ability>();
    }

    public void init()
    {
        if (Application.internetReachability != NetworkReachability.NotReachable)
        {
            StartCoroutine(DownloadTable(tableLink, 0));
            StartCoroutine(DownloadTable(tableLink2, 1));
            StartCoroutine(DownloadTable(tableLink3, 2));
            StartCoroutine(DownloadTable(tableLink4, 3));
        }
        else if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            for (int i = 0; i < tables.Length; i++)
            {
                SetTables(tables[i].text, i);
            }
        }
    }

    IEnumerator DownloadTable(string url, int num)
    {
        UnityWebRequest www = UnityWebRequest.Get(url);
        yield return www.SendWebRequest();
        SetTables(www.downloadHandler.text, num);
    }

    //스프레드시트 값을 데이터 테이블로 넣는 메소드
    //tsv는 불러오는 스프레드시트 url,num은 넣을 데이터테이블의 인덱스 번호를 의미한다
    public void SetTables(string tsv, int num)
    {
        string[] row = tsv.Split('\n');
        int rowSize = row.Length;
        int columnSize = row[0].Split(',').Length;

        for (int i = 0; i < rowSize; i++)
        {
            Debug.Log(row[i]);
            string[] column = row[i].Split(',');

            if (num == 0)
            {
                LinkedTableQuest(column);
            }
            else if (num == 1)
            {
                LinkedTableSkin(column);
            }
            else if (num == 2)
            {
                //LinkedTableWear(column);
            }
            else if (num == 3)
            {
                LinkedTableAbility(column);
            }
        }

    }


    public void LinkedTableSkin(string[] column)
    {
        ShopSkins skins = new ShopSkins(int.Parse(column[0]), column[1], column[2].ToEnum<ItemType>(), int.Parse(column[3]));

        skinList.Add(skins);
    }

    public void LinkedTableQuest(string[] column)
    {
        QuestData quest = new QuestData(column[0], column[1].ToEnum<QuestType>(), column[3], int.Parse(column[4]), int.Parse(column[5]), int.Parse(column[6]), column[2].ToEnum<QuestCase>());

        questList.Add(quest);
    }

    public void LinkedTableWear(string[] column)
    {
        BirdWear wear = new BirdWear(int.Parse(column[0]), column[1], column[2].ToEnum<ItemType>(), column[3].ToEnum<WearType>(), int.Parse(column[4]), int.Parse(column[5]));

        birdWear.Add(wear);
    }

    public void LinkedTableAbility(string[] column)
    {
        Ability data = new Ability(int.Parse(column[0]), column[2].ToEnum<AbilityType>(), column[1], column[3]);

        abilityTable.Add(data);
    }

}

public enum WearType { Hair = 0, Face, Shoes }
[System.Serializable]
public class BirdWear
{
    public int num;
    public int objNum;
    public int matNum;
    public ItemType rank;
    public WearType type;
    public string name;

    public BirdWear(int num, string name, ItemType rank, WearType type, int objNum, int matNum)
    {
        this.num = num;
        this.name = name;
        this.objNum = objNum;
        this.matNum = matNum;
        this.type = type;
        this.rank = rank;
    }
}

public enum AbilityType { Normal = 0, Special }
[System.Serializable]
public class Ability
{
    public int num;
    public AbilityType type;
    public string name;
    public string docs;

    public Ability(int num, AbilityType type, string name, string docs)
    {
        this.num = num;
        this.type = type;
        this.name = name;
        this.docs = docs;
    }
}