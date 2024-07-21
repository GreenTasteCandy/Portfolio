using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DialogType { Chat, Choose }
public enum SpeakerType { Player, NPC, PlayerOnly, NPCOnly, None }

[CreateAssetMenu]
public class DialogData : ScriptableObject
{
    public DialogStruct[] dialogs;
}

[System.Serializable]
public class DialogStruct
{
    public string dialogIndex;
    public ContentStruct[] contents;
}

[System.Serializable]
public class ContentStruct
{
    public DialogType dialogType;
    public SpeakerType speakerType;
    public bool endDialog;

    [Header("Contents")]
    public string speakerName;
    [TextArea]
    public string chat;
    public int nextChatNum;
    public bool saw;
    public ChoiceStruct[] choices = new ChoiceStruct[4];

}

[System.Serializable]
public class ChoiceStruct
{
    [TextArea]
    public string content;
    public int nextChatNum;

    public ChoiceStruct(string cont, int num)
    {
        content = cont;
        nextChatNum = num;
    }
}