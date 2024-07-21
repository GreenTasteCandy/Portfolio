using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChooseSlotView : MonoBehaviour
{
    public ChatManager chatManager;
    public TextMeshProUGUI textChoose;
    public int dialogNum;
    public int chatNum;
    public int slotNum;
    public int nextChatNum;

    private void LateUpdate()
    {
        if (GameSetting.ins.dialogTable.dialogs[dialogNum].contents[chatNum].choices[slotNum].content == "")
        {
            textChoose.text = "";
        }
        else
        {
            textChoose.text = GameSetting.ins.dialogTable.dialogs[dialogNum].contents[chatNum].choices[slotNum].content;
        }
    }

    public void NextChat()
    {
        if (nextChatNum == 0)
        {
            chatManager.chatNum++;
        }
        else
        {
            chatManager.chatNum = nextChatNum;
        }

        chatManager.ShowChat();
    }
}
