using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*플레이어의 돈을 설정하는 스크립트*/
public class PlayerGold : MonoBehaviour
{
    [SerializeField]
    int currentGold = 100;

    public int CurrentGold
    {
        set => currentGold = Mathf.Max(0, value);
        get => currentGold;
    }

    public void PayGold(int price)
    {
        currentGold -= price;
    }

    public void MaxGold()
    {
        if (currentGold > 150000)
            currentGold = 150000;
    }
}
