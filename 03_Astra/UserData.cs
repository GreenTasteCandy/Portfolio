using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class UserData : ScriptableObject
{
    [Header("Status")]
    public UserStatus user;

    [Header("Supply")]
    public PlayerInven inven;

    [Header("Request")]
    public PlayerRequest request;

    public int Money => user.money;
}

[System.Serializable]
public class UserStatus
{
    public bool isFirst;

    public int money;
    public int helmetLv;
    public int armorLv;
    public int gloveLv;
    public int bootLv;
    public int LoopShipLv;

    public int[] weaponLv;
    public int[] toolLv;
}

[System.Serializable]
public class PlayerInven
{
    public int[] itemNum;
}

[System.Serializable]
public class PlayerRequest
{
    public RequestStatus[] requestStatus;
}