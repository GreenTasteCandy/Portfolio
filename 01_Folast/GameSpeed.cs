using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/*게임 배속 기능을 위한 스크립트*/
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
            speedText.text = "게임 시작";
        }
        else
        {
            if (gameSpeed == 0)
                speedText.text = "정상 속도";
            else if (gameSpeed == 1)
                speedText.text = "2배속";
            else if (gameSpeed == 2)
                speedText.text = "3배속";
        }
    }

    public void GameSpeedSet() //배속 버튼 클릭 시 게임 속도 변화
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
