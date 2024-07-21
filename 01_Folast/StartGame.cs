using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    [SerializeField]
    Image fadeImage;
    [SerializeField]
    float fadeSpeed;
    [SerializeField]
    FadeScreen fadeScreen;
    [SerializeField]
    FadeScreen fadeLogo2;
    [SerializeField]
    GameObject titleText;

    // Update is called once per frame
    void Update()
    {
        if (fadeScreen.FadeEnd == true && fadeLogo2.FadeEnd == false)
        {
            fadeLogo2.gameObject.SetActive(true);
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (fadeLogo2.FadeEnd == true)
                StartCoroutine("FadeIn");
            else
            {
                if (SettingVariable.instance.gameStart == true && SettingVariable.instance.gpgsLogin == true)
                {
                    fadeScreen.StopCoroutine("FadeIn");
                    fadeLogo2.gameObject.SetActive(true);
                    fadeLogo2.StopCoroutine("FadeIn");
                    Color color = fadeScreen.FadeImage.color;
                    color.a = 1.0f;
                    fadeScreen.FadeImage.color = color;
                    fadeLogo2.FadeImage.color = color;
                    fadeScreen.FadeSetEnd();
                    fadeLogo2.FadeSetEnd();
                }
            }
        }
    }

    private IEnumerator FadeIn()
    {
        Color color = fadeImage.color;

        while (color.a <= 1.0f)
        {
            color.a += Time.deltaTime * fadeSpeed;
            fadeImage.color = color;

            yield return null;
        }

        LoadingSceneManager.MoveScene("LobbyScene");
    }
}
