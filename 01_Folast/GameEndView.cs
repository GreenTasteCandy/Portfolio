using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameEndView : MonoBehaviour
{
    public ScoreBoard scoreSet;

    [SerializeField]
    TextMeshProUGUI scoreText;
    [SerializeField]
    TextMeshProUGUI highScoreText;
    [SerializeField]
    TextMeshProUGUI gainCoinText;
    [SerializeField]
    bool setClear;

    string log;

    private void Start()
    {
        //�������� ���� ���
        GPGSBinder.Inst.ReportLeaderboard(GPGSIds.leaderboard, (long)scoreSet.score, success => log = $"{success}");

        //���� Ŭ���� ��
        if (setClear == true)
        {
            SettingVariable.leaderboard[0] += 1;
            GPGSBinder.Inst.ReportLeaderboard(GPGSIds.leaderboard_2, (long)SettingVariable.leaderboard[0], success => log = $"{success}");
        }

        //���� �ر�
        AchieveUnlock();
    }

    private void LateUpdate()
    {
        scoreText.text = string.Format("{0:n}", "���� : " + scoreSet.score);
        highScoreText.text = string.Format("{0:n}", "�ְ����� : " + scoreSet.highScore);
        gainCoinText.text = string.Format("{0:n}", "ȹ����ȭ : " + scoreSet.gainCoin);
    }

    public void AchieveUnlock()
    {
        if (scoreSet.score >= 1000)
            GPGSBinder.Inst.UnlockAchievement(GPGSIds.achievement, success => log = $"{success}");
        else if (scoreSet.score >= 5000)
            GPGSBinder.Inst.UnlockAchievement(GPGSIds.achievement_2, success => log = $"{success}");
        else if (scoreSet.score >= 10000)
            GPGSBinder.Inst.UnlockAchievement(GPGSIds.achievement_3, success => log = $"{success}");
        else if (scoreSet.score >= 50000)
            GPGSBinder.Inst.UnlockAchievement(GPGSIds.achievement_4, success => log = $"{success}");
        else if (scoreSet.score >= 100000)
            GPGSBinder.Inst.UnlockAchievement(GPGSIds.achievement_5, success => log = $"{success}");
        else if (scoreSet.score >= 200000)
            GPGSBinder.Inst.UnlockAchievement(GPGSIds.achievement_6, success => log = $"{success}");
    }

    public void GoLobby()
    {
        SettingVariable.cash += scoreSet.gainCoin;
        InventorySet.instance.InvenReset();
        GameManager.instance.enemySpawner.GameEndEnemyStatus();

        CloudSave.Instance.SaveWithCloud(1);

        LoadingSceneManager.MoveScene("LobbyScene");
    }
}
