using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class Lobby : MonoBehaviour
{
    public TextMeshProUGUI textNickname;
    public TextMeshProUGUI textGold;
    public TextMeshProUGUI textCash;
    public static Lobby lobby;

    public GameObject ranks;
    public GameObject option;
    public bool isOption = false;
    public int isRank = 0;

    private void Awake()
    {
        lobby = this;

        FirestoreManager.ins.LoadData();
    }

    private void Start()
    {
        DataTable.ins.init();
        QuestManager.ins.ResetDailyQuest();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isOption)
            {
                isOption = false;
            }
            else
            {
                isOption = true;
            }

            option.SetActive(isOption);
        }
    }

    public void LateUpdate()
    {
        textNickname.text = UserData.ins.data.nickName;
        textGold.text = UserData.ins.data.gold.ToString();
        textCash.text = UserData.ins.data.cash.ToString();
    }

    public void SelectMode(int num)
    {
        GameMode.ins.gameMode = num;
    }

    public void SelectMap(int num)
    {
        GameMode.ins.selectMap = num;
    }

    public void StartGame() 
    {

        switch (GameMode.ins.selectMap)
        {
            case 0:
                LoadingSystem.MoveScene("Play");
                break;

            case 1:
                LoadingSystem.MoveScene("City");
                break;

            case 2:
                LoadingSystem.MoveScene("Beach");
                break;
        }
    }

    public void ChangeRank(int num)
    {
        isRank = num;
        ranks.SetActive(true);
    }

    public void ExitGame()
    {
        StartCoroutine(GameSaveEnd());
    }

    IEnumerator GameSaveEnd()
    {
        FirestoreManager.ins.NewFieldMerge();

        yield return null;

        Application.Quit();
    }


}
