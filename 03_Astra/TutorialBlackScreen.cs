using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialBlackScreen : MonoBehaviour
{
    public ChatManager chatManager;
    public int spawnNum;

    private void LateUpdate()
    {
        if (chatManager.chatNum != spawnNum || chatManager.isChatOn != true)
        {
            Destroy(this.gameObject);
        }
    }
}
