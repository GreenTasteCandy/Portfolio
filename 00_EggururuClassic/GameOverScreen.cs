using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
//GameOverScreen은 게임오버시 실행되는 시스템입니다

public class GameOverScreen : MonoBehaviour
{

    //변수 초기화 부분
    public Text PointText;
    public Text HighPointText;

    public Move playable;

    void Start()
    {
        //점수 표시 초기화
        PointText.text = string.Format("{0:n0}", PlayerPrefs.GetInt("score"));
    }

    // Update is called once per frame
    void LateUpdate()
    {
        //달성한 기록 및 최고기록 표시
        PointText.text = string.Format("{0:n0}", "달성 기록 : " + Move.score);
    }
}
