using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
//GameOverScreen�� ���ӿ����� ����Ǵ� �ý����Դϴ�

public class GameOverScreen : MonoBehaviour
{

    //���� �ʱ�ȭ �κ�
    public Text PointText;
    public Text HighPointText;

    public Move playable;

    void Start()
    {
        //���� ǥ�� �ʱ�ȭ
        PointText.text = string.Format("{0:n0}", PlayerPrefs.GetInt("score"));
    }

    // Update is called once per frame
    void LateUpdate()
    {
        //�޼��� ��� �� �ְ��� ǥ��
        PointText.text = string.Format("{0:n0}", "�޼� ��� : " + Move.score);
    }
}
