using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ChatManager : MonoBehaviour
{
    public Request request;

    [Header("Chat")]
    [SerializeField]
    Image imageBackground;
    [SerializeField]
    Image imageTip;
    [SerializeField]
    Image imagePlayer;
    [SerializeField]
    Image imageNPC;
    [SerializeField]
    TextMeshProUGUI characterName;
    [SerializeField]
    TextMeshProUGUI textChat;
    [SerializeField]
    GameObject next;
    [SerializeField]
    GameObject skip;
    [SerializeField]
    Image chatBackplate;
    [SerializeField]
    GameObject introBackplate;
    [SerializeField]
    GameObject blackScreenPrefab;

    [Header("Choose")]
    [SerializeField]
    GameObject chooseUI;
    [SerializeField]
    GameObject chooseSlotPrefab;
    [SerializeField]
    GameObject chooseScrollView;

    [Header("Dialog")]
    int dialogNum;
    public int chatNum;
    public float typingSpeed;
    public bool isChatOn = false;
    bool skiped = false;
    bool clicked = false;
    bool blackScreenIsSpawned = false;
    public List<GameObject> chooseSlotNotice;

    /*대화창 세팅 및 띄우기*/
    public void SetDialog(string requestIndex)
    {
        for (int i = 0; i < GameSetting.ins.dialogTable.dialogs.Length; i++)
        {
            //인덱스가 일치할 경우
            if (requestIndex == GameSetting.ins.dialogTable.dialogs[i].dialogIndex)
            {
                dialogNum = i;
                chatNum = 0;

                for (int j = 0; j < GameSetting.ins.dialogTable.dialogs[i].contents.Length; j++)
                {
                    GameSetting.ins.dialogTable.dialogs[i].contents[j].nextChatNum = 0;
                    GameSetting.ins.dialogTable.dialogs[i].contents[j].saw = false;
                }

                imageBackground.color = new Color(1, 1, 1, 0.875f);
                imageTip.color = new Color(1, 1, 1, 0);
                GameSetting.ins.UI_OnOff(UI.Chat, true);
                if (GameSetting.ins.dialogTable.dialogs[dialogNum].dialogIndex != "Prolog")
                {
                    chatBackplate.color = new Color(1, 1, 1, 1);
                    introBackplate.SetActive(false);
                    textChat.horizontalAlignment = HorizontalAlignmentOptions.Left;
                    textChat.verticalAlignment = VerticalAlignmentOptions.Top;
                }
                else
                {
                    chatBackplate.color = new Color(1, 1, 1, 0);
                    introBackplate.SetActive(true);
                    textChat.horizontalAlignment = HorizontalAlignmentOptions.Center;
                    textChat.verticalAlignment = VerticalAlignmentOptions.Middle;
                }

                if (GameSetting.ins.dialogTable.dialogs[dialogNum].dialogIndex != "Prolog" && GameSetting.ins.dialogTable.dialogs[dialogNum].dialogIndex != "MQ_001" && GameSetting.ins.dialogTable.dialogs[dialogNum].dialogIndex != "MQ_002" && GameSetting.ins.dialogTable.dialogs[dialogNum].dialogIndex != "MQ_003")
                {
                    imageBackground.sprite = Resources.Load<Sprite>("Dialog Backgrounds/Lobby");
                    GameSetting.ins.UI_OnOff(UI.LobbyUI, false);
                    GameSetting.ins.UI_OnOff(UI.Hud, false);
                    GameSetting.ins.UI_OnOff(UI.Ship, false);
                    GameSetting.ins.UI_OnOff(UI.Deal, false);
                    GameSetting.ins.UI_OnOff(UI.Equip, false);
                    GameSetting.ins.UI_OnOff(UI.Explore, false);
                    GameSetting.ins.UI_OnOff(UI.PlanetInfo, false);
                    GameSetting.ins.UI_OnOff(UI.Storage, false);
                    GameSetting.ins.UI_OnOff(UI.Planet, true);
                    GameSetting.ins.UI_OnOff(UI.Request, false);
                }

                isChatOn = true;
                skiped = false;
                clicked = false;
                blackScreenIsSpawned = false;
                ShowChat();
                break;
            }
        }
    }

    /*대화창 내용들 출력*/
    public void ShowChat()
    {
        if (GameSetting.ins.dialogTable.dialogs[dialogNum].contents[chatNum].speakerName == "워커")
        {
            imageNPC.color = new Color(1, 1, 1, 1);
            imageNPC.sprite = Resources.Load<Sprite>("NPC/Worker");
        }
        else if (GameSetting.ins.dialogTable.dialogs[dialogNum].contents[chatNum].speakerName == "하나")
        {
            imageNPC.color = new Color(1, 1, 1, 1);
            imageNPC.sprite = Resources.Load<Sprite>("NPC/Hana");
        }
        else
        {
            imageNPC.color = new Color(1, 1, 1, 0);
        }

        if (GameSetting.ins.dialogTable.dialogs[dialogNum].contents[chatNum].dialogType == DialogType.Chat)
        {
            //화자가 누구냐에 따라 캐릭터 이미지들에 변화 주기
            if (GameSetting.ins.dialogTable.dialogs[dialogNum].contents[chatNum].speakerType == SpeakerType.Player)
            {
                imagePlayer.color = new Color(1f, 1f, 1f, 1f);
                imageNPC.color = new Color(0.6f, 0.6f, 0.6f, 1f);
            }
            else if (GameSetting.ins.dialogTable.dialogs[dialogNum].contents[chatNum].speakerType == SpeakerType.NPC)
            {
                imagePlayer.color = new Color(0.6f, 0.6f, 0.6f, 1f);
                imageNPC.color = new Color(1f, 1f, 1f, 1f);
            }
            else if (GameSetting.ins.dialogTable.dialogs[dialogNum].contents[chatNum].speakerType == SpeakerType.PlayerOnly)
            {
                imagePlayer.color = new Color(1f, 1f, 1f, 1f);
                imageNPC.color = new Color(1f, 1f, 1f, 0);
            }
            else if (GameSetting.ins.dialogTable.dialogs[dialogNum].contents[chatNum].speakerType == SpeakerType.NPCOnly)
            {
                imagePlayer.color = new Color(1f, 1f, 1f, 0);
                imageNPC.color = new Color(1f, 1f, 1f, 1f);
            }
            else if (GameSetting.ins.dialogTable.dialogs[dialogNum].contents[chatNum].speakerType == SpeakerType.None)
            {
                imagePlayer.color = new Color(0, 0, 0, 0);
                imageNPC.color = new Color(0, 0, 0, 0);
            }

            characterName.text = GameSetting.ins.dialogTable.dialogs[dialogNum].contents[chatNum].speakerName;
            StartCoroutine(TypeText());
            chooseUI.SetActive(false);
        }

        else if (GameSetting.ins.dialogTable.dialogs[dialogNum].contents[chatNum].dialogType == DialogType.Choose)
        {
            //선택지가 나와야 한다면 선택지 슬롯을 띄운다
            if (chooseSlotNotice != null)
            {
                for (int i = 0; i < chooseSlotNotice.Count; i++)
                {
                    Destroy(chooseSlotNotice[i]);
                }
            }
            chooseSlotNotice.Clear();

            for (int i = 0; i < GameSetting.ins.dialogTable.dialogs[dialogNum].contents[chatNum].choices.Length; i++)
            {
                if (GameSetting.ins.dialogTable.dialogs[dialogNum].contents[chatNum].choices[i].content != "")
                {
                    GameObject chooseSlot = Instantiate(chooseSlotPrefab);
                    chooseSlot.GetComponent<ChooseSlotView>().chatManager = this;
                    chooseSlot.GetComponent<ChooseSlotView>().dialogNum = dialogNum;
                    chooseSlot.GetComponent<ChooseSlotView>().chatNum = chatNum;
                    chooseSlot.GetComponent<ChooseSlotView>().slotNum = i;
                    chooseSlot.GetComponent<ChooseSlotView>().nextChatNum = GameSetting.ins.dialogTable.dialogs[dialogNum].contents[chatNum].choices[i].nextChatNum;
                    chooseSlot.GetComponent<RectTransform>().sizeDelta = new Vector2(1200, 80);
                    chooseSlot.transform.SetParent(chooseScrollView.transform);
                    chooseSlot.GetComponent<RectTransform>().localScale = Vector3.one;
                    chooseSlotNotice.Add(chooseSlot);
                }
            }

            chooseUI.SetActive(true);
        }

        next.SetActive(false);
    }

    /*대화가 타이핑되듯이 출력시키는 코루틴*/
    IEnumerator TypeText()
    {
        skiped = false;
        textChat.text = "";
        string text_temp = GameSetting.ins.dialogTable.dialogs[dialogNum].contents[chatNum].chat;
        yield return new WaitForSeconds(typingSpeed);

        if (GameSetting.ins.chatUI.activeInHierarchy == false)
        {
            yield break;
        }

        for (int i = 0; i < GameSetting.ins.dialogTable.dialogs[dialogNum].contents[chatNum].chat.Length+1; i++)
        {
            if (text_temp == GameSetting.ins.dialogTable.dialogs[dialogNum].contents[chatNum].chat)
            {
                if (skiped == true)
                {
                    textChat.text = GameSetting.ins.dialogTable.dialogs[dialogNum].contents[chatNum].chat;
                    break;
                }
                else
                {
                    textChat.text = GameSetting.ins.dialogTable.dialogs[dialogNum].contents[chatNum].chat.Substring(0, i);
                }
                yield return new WaitForSeconds(typingSpeed);
            }
            else
            {
                yield break;
            }
        }

        next.SetActive(true);
    }

    /*대화창 다음으로 넘기기*/
    public void NextChat()
    {
        if (textChat.text.Length < GameSetting.ins.dialogTable.dialogs[dialogNum].contents[chatNum].chat.Length)
        {
            skiped = true;
        }
        else
        {
            //다이얼로그 끝내기
            if (chatNum >= GameSetting.ins.dialogTable.dialogs[dialogNum].contents.Length - 1 || GameSetting.ins.dialogTable.dialogs[dialogNum].contents[chatNum].endDialog == true)
            {
                if (GameSetting.ins.dialogTable.dialogs[dialogNum].dialogIndex == "Prolog")
                {
                    GameSetting.ins.requestTable.requests[0].requestStatus = RequestStatus.Progressing;
                    SetDialog("MQ_001");
                }
                else if (GameSetting.ins.dialogTable.dialogs[dialogNum].dialogIndex == "MQ_001")
                {
                    GameSetting.ins.requestTable.requests[0].requestStatus = RequestStatus.Completed;
                    GameSetting.ins.requestTable.requests[1].requestStatus = RequestStatus.Progressing;
                    SetDialog("MQ_002");
                }
                else if (GameSetting.ins.dialogTable.dialogs[dialogNum].dialogIndex == "MQ_002")
                {
                    GameSetting.ins.requestTable.requests[1].requestStatus = RequestStatus.Completed;
                    GameSetting.ins.requestTable.requests[2].requestStatus = RequestStatus.Progressing;
                    SetDialog("MQ_003");
                }
                else if (GameSetting.ins.dialogTable.dialogs[dialogNum].dialogIndex == "MQ_003")
                {
                    GameSetting.ins.requestTable.requests[2].requestStatus = RequestStatus.Completed;
                    GameSetting.ins.requestTable.requests[3].requestStatus = RequestStatus.Progressing;
                    SetDialog("MQ_004");
                }
                else if (GameSetting.ins.dialogTable.dialogs[dialogNum].dialogIndex == "MQ_004")
                {
                    isChatOn = false;
                    GameSetting.ins.UI_OnOff(UI.LobbyUI, true);
                    GameSetting.ins.UI_OnOff(UI.Hud, true);
                    GameSetting.ins.UI_OnOff(UI.Request, false);
                    GameSetting.ins.UI_OnOff(UI.Chat, false);
                }
                else
                {
                    isChatOn = false;
                    GameSetting.ins.UI_OnOff(UI.LobbyUI, false);
                    GameSetting.ins.UI_OnOff(UI.Hud, true);
                    GameSetting.ins.UI_OnOff(UI.Ship, false);
                    GameSetting.ins.UI_OnOff(UI.Deal, false);
                    GameSetting.ins.UI_OnOff(UI.Equip, false);
                    GameSetting.ins.UI_OnOff(UI.Explore, false);
                    GameSetting.ins.UI_OnOff(UI.PlanetInfo, false);
                    GameSetting.ins.UI_OnOff(UI.Storage, false);
                    GameSetting.ins.UI_OnOff(UI.Planet, true);
                    GameSetting.ins.UI_OnOff(UI.Request, true);
                    GameSetting.ins.UI_OnOff(UI.Chat, false);
                }
            }
            //다이얼로그 계속 진행
            else
            {
                if (GameSetting.ins.dialogTable.dialogs[dialogNum].contents[chatNum].nextChatNum == 0)
                {
                    GameSetting.ins.dialogTable.dialogs[dialogNum].contents[chatNum].saw = true;
                    blackScreenIsSpawned = false;
                    clicked = false;
                    chatNum++;
                }
                else
                {
                    for (int i = 0; i < GameSetting.ins.dialogTable.dialogs[dialogNum].contents[chatNum].nextChatNum; i++)
                    {
                        if (GameSetting.ins.dialogTable.dialogs[dialogNum].contents[i].saw == false)
                        {
                            GameSetting.ins.dialogTable.dialogs[dialogNum].contents[i].saw = true;
                        }
                    }
                    chatNum = GameSetting.ins.dialogTable.dialogs[dialogNum].contents[chatNum].nextChatNum;
                }

                StopCoroutine(TypeText());
                ShowChat();
            }
        }
    }

    /*스킵 기능*/
    public void Skip()
    {
        StopCoroutine(TypeText());

        if (GameSetting.ins.dialogTable.dialogs[dialogNum].dialogIndex == "Prolog" || GameSetting.ins.dialogTable.dialogs[dialogNum].dialogIndex == "MQ_001" || GameSetting.ins.dialogTable.dialogs[dialogNum].dialogIndex == "MQ_002" || GameSetting.ins.dialogTable.dialogs[dialogNum].dialogIndex == "MQ_003" || GameSetting.ins.dialogTable.dialogs[dialogNum].dialogIndex == "MQ_004")
        {
            GameSetting.ins.UI_OnOff(UI.Ship, false);
            GameSetting.ins.UI_OnOff(UI.Deal, false);
            GameSetting.ins.UI_OnOff(UI.Equip, false);
            GameSetting.ins.UI_OnOff(UI.Storage, false);
            GameSetting.ins.UI_OnOff(UI.Explore, false);
            GameSetting.ins.UI_OnOff(UI.PlanetInfo, false);
            GameSetting.ins.UI_OnOff(UI.Planet, true);

            //튜토리얼과 의뢰 진행 상황 매치시키기
            if (GameSetting.ins.dialogTable.dialogs[dialogNum].dialogIndex == "Prolog")
            {
                GameSetting.ins.requestTable.requests[0].requestStatus = RequestStatus.Progressing;
                SetDialog("MQ_001");
            }
            else if (GameSetting.ins.dialogTable.dialogs[dialogNum].dialogIndex == "MQ_001")
            {
                GameSetting.ins.requestTable.requests[0].requestStatus = RequestStatus.Completed;
                GameSetting.ins.requestTable.requests[1].requestStatus = RequestStatus.Completed;
                GameSetting.ins.requestTable.requests[2].requestStatus = RequestStatus.Completed;
                GameSetting.ins.requestTable.requests[3].requestStatus = RequestStatus.Progressing;
                SetDialog("MQ_004");
            }
            else if (GameSetting.ins.dialogTable.dialogs[dialogNum].dialogIndex == "MQ_002")
            {
                GameSetting.ins.requestTable.requests[1].requestStatus = RequestStatus.Completed;
                GameSetting.ins.requestTable.requests[2].requestStatus = RequestStatus.Completed;
                GameSetting.ins.requestTable.requests[3].requestStatus = RequestStatus.Progressing;
                SetDialog("MQ_004");
            }
            else if (GameSetting.ins.dialogTable.dialogs[dialogNum].dialogIndex == "MQ_003")
            {
                GameSetting.ins.requestTable.requests[2].requestStatus = RequestStatus.Completed;
                GameSetting.ins.requestTable.requests[3].requestStatus = RequestStatus.Progressing;
                SetDialog("MQ_004");
            }
            else if (GameSetting.ins.dialogTable.dialogs[dialogNum].dialogIndex == "MQ_004")
            {
                isChatOn = false;
                GameSetting.ins.UI_OnOff(UI.LobbyUI, true);
                GameSetting.ins.UI_OnOff(UI.Hud, true);
                GameSetting.ins.UI_OnOff(UI.Request, false);
                GameSetting.ins.UI_OnOff(UI.Chat, false);
            }
        }
        else
        {
            isChatOn = false;
            GameSetting.ins.UI_OnOff(UI.LobbyUI, false);
            GameSetting.ins.UI_OnOff(UI.Hud, true);
            GameSetting.ins.UI_OnOff(UI.Ship, false);
            GameSetting.ins.UI_OnOff(UI.Deal, false);
            GameSetting.ins.UI_OnOff(UI.Equip, false);
            GameSetting.ins.UI_OnOff(UI.Explore, false);
            GameSetting.ins.UI_OnOff(UI.Storage, false);
            GameSetting.ins.UI_OnOff(UI.PlanetInfo, false);
            GameSetting.ins.UI_OnOff(UI.Planet, true);
            GameSetting.ins.UI_OnOff(UI.Request, true);
            GameSetting.ins.UI_OnOff(UI.Chat, false);
        }
    }

    /*대화창에서 따로 작동해야 하는 것들은 여기서 처리*/
    private void LateUpdate()
    {
        if (isChatOn == true)
        {
            /*Prolog - 프롤로그*/
            if (GameSetting.ins.dialogTable.dialogs[dialogNum].dialogIndex == "Prolog")
            {
                if (chatNum == 0)
                {
                    imageBackground.color = new Color(0, 0, 0, 1);
                    GameSetting.ins.UI_OnOff(UI.LobbyUI, false);
                }
                if (chatNum >= 1)
                {
                    imageBackground.color = new Color(1, 1, 1, 1);
                }
                if (chatNum == 1) { imageBackground.sprite = Resources.Load<Sprite>("Dialog Backgrounds/intro_bg_01"); }
                if (chatNum >= 2 && chatNum < 6) { imageBackground.sprite = Resources.Load<Sprite>("Dialog Backgrounds/intro_bg_02"); }
                if (chatNum >= 6 && chatNum < 8) { imageBackground.sprite = Resources.Load<Sprite>("Dialog Backgrounds/intro_bg_03"); }
                if (chatNum == 8) { imageBackground.sprite = Resources.Load<Sprite>("Dialog Backgrounds/intro_bg_04"); }
                if (chatNum >= 9 && chatNum < 11) { imageBackground.sprite = Resources.Load<Sprite>("Dialog Backgrounds/intro_bg_10"); }
                if (chatNum == 12) { imageBackground.sprite = Resources.Load<Sprite>("Dialog Backgrounds/intro_bg_06"); }
                if (chatNum == 13) { imageBackground.sprite = Resources.Load<Sprite>("Dialog Backgrounds/intro_bg_07"); }
                if (chatNum == 14) { imageBackground.sprite = Resources.Load<Sprite>("Dialog Backgrounds/intro_bg_09_1"); }
            }
            /*MQ_001 - 훈련의 시작 1*/
            if (GameSetting.ins.dialogTable.dialogs[dialogNum].dialogIndex == "MQ_001")
            {
                if (chatNum == 0)
                {
                    imageBackground.color = new Color(0, 0, 0, 0.875f);
                    GameSetting.ins.UI_OnOff(UI.Request, false);
                    GameSetting.ins.UI_OnOff(UI.LobbyUI, true);
                    GameSetting.ins.UI_OnOff(UI.Hud, true);
                }
                if (chatNum >= 3 && chatNum < 5)
                {
                    if (chatNum == 3 && !blackScreenIsSpawned)
                    {
                        GameObject blackScreen = Instantiate(blackScreenPrefab);
                        blackScreen.transform.SetParent(GameSetting.ins.lobbyUI.transform);
                        blackScreen.transform.SetAsLastSibling();
                        blackScreen.GetComponent<TutorialBlackScreen>().chatManager = this;
                        blackScreen.GetComponent<TutorialBlackScreen>().spawnNum = chatNum;
                        GameSetting.ins.equipButton.transform.SetAsLastSibling();
                        blackScreenIsSpawned = true;
                    }

                    if (GameSetting.ins.dialogTable.dialogs[dialogNum].contents[chatNum].saw == false && clicked == false)
                    {
                        if (GameSetting.ins.equipUI.activeInHierarchy == false) { GameSetting.ins.UI_OnOff(UI.Chat, false); }
                        else if (GameSetting.ins.equipUI.activeInHierarchy == true)
                        {
                            GameSetting.ins.UI_OnOff(UI.Chat, true);
                            clicked = true;
                            ShowChat();
                        }
                    }
                    else if (GameSetting.ins.dialogTable.dialogs[dialogNum].contents[chatNum].saw == true)
                    {
                        GameSetting.ins.UI_OnOff(UI.LobbyUI, false);
                        GameSetting.ins.UI_OnOff(UI.Equip, true);
                    }
                }
                if (chatNum == 5)
                {
                    GameSetting.ins.UI_OnOff(UI.Equip, false);
                    GameSetting.ins.UI_OnOff(UI.LobbyUI, true);
                }
                if (chatNum >= 6 && chatNum < 8)
                {
                    if (chatNum == 6 && !blackScreenIsSpawned)
                    {
                        GameObject blackScreen = Instantiate(blackScreenPrefab);
                        blackScreen.transform.SetParent(GameSetting.ins.lobbyUI.transform);
                        blackScreen.transform.SetAsLastSibling();
                        blackScreen.GetComponent<TutorialBlackScreen>().chatManager = this;
                        blackScreen.GetComponent<TutorialBlackScreen>().spawnNum = chatNum;
                        GameSetting.ins.storageButton.transform.SetAsLastSibling();
                        blackScreenIsSpawned = true;
                    }

                    if (GameSetting.ins.dialogTable.dialogs[dialogNum].contents[chatNum].saw == false && clicked == false)
                    {
                        if (GameSetting.ins.storageUI.activeInHierarchy == false) { GameSetting.ins.UI_OnOff(UI.Chat, false); }
                        else if (GameSetting.ins.storageUI.activeInHierarchy == true)
                        {
                            GameSetting.ins.UI_OnOff(UI.Chat, true);
                            clicked = true;
                            ShowChat();
                        }
                    }
                    else if (GameSetting.ins.dialogTable.dialogs[dialogNum].contents[chatNum].saw == true)
                    {
                        GameSetting.ins.UI_OnOff(UI.LobbyUI, false);
                        GameSetting.ins.UI_OnOff(UI.Storage, true);
                    }
                }
                if (chatNum == 8)
                {
                    GameSetting.ins.UI_OnOff(UI.Equip, false);
                    GameSetting.ins.UI_OnOff(UI.Storage, false);
                    GameSetting.ins.UI_OnOff(UI.LobbyUI, true);
                }
                if (chatNum >= 8)
                {
                    GameSetting.ins.dialogTable.dialogs[dialogNum].contents[4].nextChatNum = 8;
                }
            }
            /*MQ_002 - 훈련의 시작 2*/
            if (GameSetting.ins.dialogTable.dialogs[dialogNum].dialogIndex == "MQ_002")
            {
                if (chatNum == 0)
                {
                    imageBackground.color = new Color(0, 0, 0, 0.875f);
                    GameSetting.ins.UI_OnOff(UI.Request, false);
                    GameSetting.ins.UI_OnOff(UI.LobbyUI, true);
                    GameSetting.ins.UI_OnOff(UI.Hud, true);
                }
                if (chatNum >= 3 && chatNum < 5)
                {
                    if (chatNum == 3 && !blackScreenIsSpawned)
                    {
                        GameObject blackScreen = Instantiate(blackScreenPrefab);
                        blackScreen.transform.SetParent(GameSetting.ins.lobbyUI.transform);
                        blackScreen.transform.SetAsLastSibling();
                        blackScreen.GetComponent<TutorialBlackScreen>().chatManager = this;
                        blackScreen.GetComponent<TutorialBlackScreen>().spawnNum = chatNum;
                        GameSetting.ins.shipButton.transform.SetAsLastSibling();
                        blackScreenIsSpawned = true;
                    }

                    if (GameSetting.ins.dialogTable.dialogs[dialogNum].contents[chatNum].saw == false && clicked == false)
                    {
                        if (GameSetting.ins.shipUI.activeInHierarchy == false) { GameSetting.ins.UI_OnOff(UI.Chat, false); }
                        else if (GameSetting.ins.shipUI.activeInHierarchy == true)
                        {
                            GameSetting.ins.UI_OnOff(UI.Chat, true);
                            clicked = true;
                            ShowChat();
                        }
                    }
                    else if (GameSetting.ins.dialogTable.dialogs[dialogNum].contents[chatNum].saw == true)
                    {
                        GameSetting.ins.UI_OnOff(UI.LobbyUI, false);
                        GameSetting.ins.UI_OnOff(UI.Ship, true);
                    }
                }
                if (chatNum == 5)
                {
                    GameSetting.ins.UI_OnOff(UI.Ship, false);
                    GameSetting.ins.UI_OnOff(UI.LobbyUI, true);
                }
            }
            /*MQ_003 - 훈련의 시작 3*/
            if (GameSetting.ins.dialogTable.dialogs[dialogNum].dialogIndex == "MQ_003")
            {
                if (chatNum == 0)
                {
                    imageBackground.color = new Color(0, 0, 0, 0.875f);
                    GameSetting.ins.UI_OnOff(UI.Request, false);
                    GameSetting.ins.UI_OnOff(UI.LobbyUI, true);
                    GameSetting.ins.UI_OnOff(UI.Hud, true);
                }
                if (chatNum >= 2 && chatNum < 4)
                {
                    if (chatNum == 2 && !blackScreenIsSpawned)
                    {
                        GameObject blackScreen = Instantiate(blackScreenPrefab);
                        blackScreen.transform.SetParent(GameSetting.ins.lobbyUI.transform);
                        blackScreen.transform.SetAsLastSibling();
                        blackScreen.GetComponent<TutorialBlackScreen>().chatManager = this;
                        blackScreen.GetComponent<TutorialBlackScreen>().spawnNum = chatNum;
                        GameSetting.ins.requestButton.transform.SetAsLastSibling();
                        blackScreenIsSpawned = true;
                    }

                    if (GameSetting.ins.dialogTable.dialogs[dialogNum].contents[chatNum].saw == false && clicked == false)
                    {
                        if (GameSetting.ins.requestUI.activeInHierarchy == false) { GameSetting.ins.UI_OnOff(UI.Chat, false); }
                        else if (GameSetting.ins.requestUI.activeInHierarchy == true)
                        {
                            GameSetting.ins.UI_OnOff(UI.Chat, true);
                            clicked = true;
                            ShowChat();
                        }
                    }
                    else if (GameSetting.ins.dialogTable.dialogs[dialogNum].contents[chatNum].saw == true)
                    {
                        GameSetting.ins.UI_OnOff(UI.LobbyUI, false);
                        GameSetting.ins.UI_OnOff(UI.Request, true);
                    }
                }
                if (chatNum >= 4 && chatNum < 6)
                {
                    GameSetting.ins.UI_OnOff(UI.Request, false);
                    GameSetting.ins.UI_OnOff(UI.LobbyUI, true);
                }
                if (chatNum == 6)
                {
                    if (!blackScreenIsSpawned)
                    {
                        GameObject blackScreen = Instantiate(blackScreenPrefab);
                        blackScreen.transform.SetParent(GameSetting.ins.lobbyUI.transform);
                        blackScreen.transform.SetAsLastSibling();
                        blackScreen.GetComponent<TutorialBlackScreen>().chatManager = this;
                        blackScreen.GetComponent<TutorialBlackScreen>().spawnNum = chatNum;
                        GameSetting.ins.exploreButton.transform.SetAsLastSibling();
                        blackScreenIsSpawned = true;
                    }

                    if (clicked == false)
                    {
                        if (GameSetting.ins.exploreUI.activeInHierarchy == false) { GameSetting.ins.UI_OnOff(UI.Chat, false); }
                        else if (GameSetting.ins.exploreUI.activeInHierarchy == true)
                        {
                            GameSetting.ins.UI_OnOff(UI.Chat, true);
                            clicked = true;
                            ShowChat();
                        }
                    }
                }
                if (chatNum == 7)
                {
                    if (clicked == false)
                    {
                        GameSetting.ins.UI_OnOff(UI.Planet, false);
                        GameSetting.ins.exploreUI.GetComponent<Explore>().OnPlanetInfo(0);
                        clicked = true;
                    }
                }
                if (chatNum >= 9 && chatNum < 12)
                {
                    GameSetting.ins.UI_OnOff(UI.Explore, false);
                    GameSetting.ins.UI_OnOff(UI.Planet, true);
                    GameSetting.ins.UI_OnOff(UI.PlanetInfo, false);
                    GameSetting.ins.UI_OnOff(UI.LobbyUI, true);
                }
                if (chatNum >= 12 && chatNum < 15)
                {
                    if (chatNum == 12 && !blackScreenIsSpawned)
                    {
                        GameObject blackScreen = Instantiate(blackScreenPrefab);
                        blackScreen.transform.SetParent(GameSetting.ins.lobbyUI.transform);
                        blackScreen.transform.SetAsLastSibling();
                        blackScreen.GetComponent<TutorialBlackScreen>().chatManager = this;
                        blackScreen.GetComponent<TutorialBlackScreen>().spawnNum = chatNum;
                        GameSetting.ins.dealButton.transform.SetAsLastSibling();
                        blackScreenIsSpawned = true;
                    }

                    if (GameSetting.ins.dialogTable.dialogs[dialogNum].contents[chatNum].saw == false && clicked == false)
                    {
                        if (GameSetting.ins.dealUI.activeInHierarchy == false) { GameSetting.ins.UI_OnOff(UI.Chat, false); }
                        else if (GameSetting.ins.dealUI.activeInHierarchy == true)
                        {
                            GameSetting.ins.UI_OnOff(UI.Chat, true);
                            clicked = true;
                            ShowChat();
                        }
                    }
                    else if (GameSetting.ins.dialogTable.dialogs[dialogNum].contents[chatNum].saw == true)
                    {
                        GameSetting.ins.UI_OnOff(UI.LobbyUI, false);
                        GameSetting.ins.UI_OnOff(UI.Deal, true);
                    }
                }
                if (chatNum >= 15)
                {
                    GameSetting.ins.UI_OnOff(UI.Request, false);
                    GameSetting.ins.UI_OnOff(UI.Explore, false);
                    GameSetting.ins.UI_OnOff(UI.Planet, true);
                    GameSetting.ins.UI_OnOff(UI.PlanetInfo, false);
                    GameSetting.ins.UI_OnOff(UI.Deal, false);
                    GameSetting.ins.UI_OnOff(UI.LobbyUI, true);
                    GameSetting.ins.dialogTable.dialogs[dialogNum].contents[2].nextChatNum = 17;
                    GameSetting.ins.dialogTable.dialogs[dialogNum].contents[8].nextChatNum = 17;
                    GameSetting.ins.dialogTable.dialogs[dialogNum].contents[13].nextChatNum = 17;
                }
            }
            /*MQ_004 - 첫 번째 의뢰*/
            if (GameSetting.ins.dialogTable.dialogs[dialogNum].dialogIndex == "MQ_004")
            {
                if (chatNum >= 11 && chatNum < 16)
                {
                    imageTip.color = new Color(1, 1, 1, 1);
                    if (chatNum == 11) { imageTip.sprite = Resources.Load<Sprite>("Dialog Backgrounds/move"); }
                    if (chatNum == 12) { imageTip.sprite = Resources.Load<Sprite>("Dialog Backgrounds/jump"); }
                    if (chatNum == 13) { imageTip.sprite = Resources.Load<Sprite>("Dialog Backgrounds/gather"); }
                    if (chatNum == 14) { imageTip.sprite = Resources.Load<Sprite>("Dialog Backgrounds/status"); }
                    if (chatNum == 15) { imageTip.sprite = Resources.Load<Sprite>("Dialog Backgrounds/inventory"); }
                }
                else
                {
                    imageTip.color = new Color(1, 1, 1, 0);
                }
            }
            /*MQ_005 - 강력하잖아?*/
            if (GameSetting.ins.dialogTable.dialogs[dialogNum].dialogIndex == "MQ_005")
            {
                if (chatNum >= 12 && chatNum < 16)
                {
                    imageTip.color = new Color(1, 1, 1, 1);
                    if (chatNum == 12) { imageTip.sprite = Resources.Load<Sprite>("Dialog Backgrounds/attack"); }
                    if (chatNum == 13) { imageTip.sprite = Resources.Load<Sprite>("Dialog Backgrounds/swapweapon"); }
                    if (chatNum == 14) { imageTip.sprite = Resources.Load<Sprite>("Dialog Backgrounds/skill"); }
                    if (chatNum == 15) { imageTip.sprite = Resources.Load<Sprite>("Dialog Backgrounds/status"); }
                }
                else
                {
                    imageTip.color = new Color(1, 1, 1, 0);
                }
            }
            /*MQ_009 - 메테르 광물 조사*/
            if (GameSetting.ins.dialogTable.dialogs[dialogNum].dialogIndex == "MQ_009")
            {
                GameSetting.ins.dialogTable.dialogs[dialogNum].contents[8].nextChatNum = 4;
                GameSetting.ins.dialogTable.dialogs[dialogNum].contents[11].nextChatNum = 4;
                GameSetting.ins.dialogTable.dialogs[dialogNum].contents[15].nextChatNum = 4;
            }
        }
    }
}