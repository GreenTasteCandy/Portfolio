using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/*UI���� �ؽ�Ʈ�� ���� ���� ��ũ��Ʈ*/
public class TextTMPViewer : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI textPlayerHP;
    [SerializeField]
    PlayerHP playerHP;
    [SerializeField]
    TextMeshProUGUI textPlayerGold;
    [SerializeField]
    PlayerGold playerGold;
    [SerializeField]
    TextMeshProUGUI textWave;
    [SerializeField]
    WaveSystem waveSystem;
    [SerializeField]
    TextMeshProUGUI textPoint;
    [SerializeField]
    TextMeshProUGUI textEnemyCount;
    [SerializeField]
    EnemySpawn enemySpawner;

    private void Update()
    {
        textPlayerHP.text = playerHP.CurrentHP + "/" + playerHP.MaxHP;
        textPlayerGold.text = string.Format("{0:n0}", playerGold.CurrentGold);
        textWave.text = "Round " + waveSystem.CurrentWave + "/" + waveSystem.MaxWave;

        if (waveSystem.WaveType == WaveType.ReadyWave || waveSystem.WaveType == WaveType.ReadyOffense)
            textEnemyCount.text = "�غ� �ܰ�";
        else if (waveSystem.WaveType == WaveType.Defense)
            textEnemyCount.text = enemySpawner.CurrentEnemyCount + "/" + enemySpawner.MaxEnemyCount;
        else if (waveSystem.WaveType == WaveType.Offense)
            textEnemyCount.text = string.Format("{0:N2}", enemySpawner.TimeOut);

        textPoint.text = string.Format("{0:n0}", enemySpawner.gamePoint);
    }
}
