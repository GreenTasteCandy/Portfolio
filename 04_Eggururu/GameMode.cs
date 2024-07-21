using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMode
{
    //½ºÅ©¸³Æ® ½Ì±ÛÅæ ¼±¾ð
    private static GameMode Ins = null;
    public static GameMode ins
    {
        get
        {
            if (Ins == null)
            {
                Ins = new GameMode();
            }

            return Ins;
        }
    }

    public int gameMode = 1;
    public int selectMap;
}
