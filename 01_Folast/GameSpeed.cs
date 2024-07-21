using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/*���� ��� ����� ���� ��ũ��Ʈ*/
public class GameSpeed : MonoBehaviour
{
    int gameSpeed;
    [SerializeField]
    TextMeshProUGUI speedText;
    [SerializeField]
    WaveSystem waveSystem;

    private void LateUpdate()
    {
        if (waveSystem.WaveType == WaveType.ReadyWave || waveSystem.WaveType == WaveType.ReadyOffense)
        {
            speedText.text = "���� ����";
        }
        else
        {
            if (gameSpeed == 0)
                speedText.text = "���� �ӵ�";
            else if (gameSpeed == 1)
                speedText.text = "2���";
            else if (gameSpeed == 2)
                speedText.text = "3���";
        }
    }

    public void GameSpeedSet() //��� ��ư Ŭ�� �� ���� �ӵ� ��ȭ
    {
        if (waveSystem.WaveType == WaveType.ReadyWave || waveSystem.WaveType == WaveType.ReadyOffense)
        {
            waveSystem.StartWave();
        }
        else
        {
            if (Time.timeScale == 1f)
            {
                gameSpeed = 1;
                Time.timeScale = 2f;
            }
            else if (Time.timeScale == 2f)
            {
                gameSpeed = 2;
                Time.timeScale = 3f;
            }
            else if (Time.timeScale == 3f)
            {
                gameSpeed = 0;
                Time.timeScale = 1f;
            }
        }
    }
}
