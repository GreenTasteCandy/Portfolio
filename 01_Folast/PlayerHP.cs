using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/*플레이어의 체력을 설정하는 스크립트*/
public class PlayerHP : MonoBehaviour
{
    [SerializeField]
    Image imageScreen;
    [SerializeField]
    float maxHP = 10;
    [SerializeField]
    ScoreBoard scoreBoard;
    [SerializeField]
    FadeScreen fadeScreen;

    float currentHP;

    public float MaxHP => maxHP;
    public float CurrentHP => currentHP;

    private void Awake()
    {
        currentHP = maxHP;
    }

    public void GainHP(float heal)
    {
        currentHP += heal;
    }

    public void TakeDamage(float damage) //데미지를 받게 함
    {
        //데미지를 받을시 효과
        currentHP -= damage;

        StopCoroutine("HitAlphaAnimation");
        StartCoroutine("HitAlphaAnimation");

        if (currentHP <= 0) //플레이어 체력이 0이 되면 게임 오버 화면으로
        {
            StopCoroutine("HitAlphaAnimation");
            StartCoroutine("GameOverFadeOut");
        }
    }

    private IEnumerator HitAlphaAnimation() //플레이어 체력 감소 시 이펙트를 주기 위함
    {
        Color color = imageScreen.color;
        color.a = 0.4f;
        imageScreen.color = color;

        while (color.a >= 0.0f)
        {
            color.a -= Time.deltaTime;
            imageScreen.color = color;

            yield return null;
        }
    }

    private IEnumerator GameOverFadeOut()
    {
        Color color = imageScreen.color;
        color = Color.black;
        color.a = 0.0f;
        imageScreen.color = color;

        while (color.a <= 1.0f)
        {
            color.a += Time.deltaTime;
            imageScreen.color = color;

            yield return null;
        }

        if (color.a >= 1.0f)
        {
            Time.timeScale = 1.0f;

            scoreBoard.score = GameManager.instance.enemySpawner.gamePoint;
            int gain = (int)(GameManager.instance.enemySpawner.gamePoint / 12000);

            scoreBoard.gainCoin = gain;
            if (GameManager.instance.enemySpawner.gamePoint > scoreBoard.highScore)
                scoreBoard.highScore = GameManager.instance.enemySpawner.gamePoint;

            SceneManager.LoadScene("GameOver");
        }
    }

    private IEnumerator GameClearFadeOut()
    {
        Color color = imageScreen.color;
        color = Color.black;
        color.a = 0.0f;
        imageScreen.color = color;

        while (color.a <= 1.0f)
        {
            color.a += Time.deltaTime;
            imageScreen.color = color;

            yield return null;
        }

        if (color.a >= 1.0f)
        {
            Time.timeScale = 1.0f;

            scoreBoard.score = GameManager.instance.enemySpawner.gamePoint;
            int gain = (int)(GameManager.instance.enemySpawner.gamePoint / 12000) + 2;

            scoreBoard.gainCoin = gain;

            if (GameManager.instance.enemySpawner.gamePoint > scoreBoard.highScore)
                scoreBoard.highScore = GameManager.instance.enemySpawner.gamePoint;

            SceneManager.LoadScene("GameClear");
        }
    }
}
