using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeScreen : MonoBehaviour
{
    [SerializeField]
    bool fadeBool;

    [SerializeField]
    float fadeSpeed;

    [SerializeField]
    Image fadeImage;

    [SerializeField]
    bool fadeEnd = false;

    public bool FadeBool => fadeBool;
    public float FadeSpeed => fadeSpeed;
    public Image FadeImage => fadeImage;
    public bool FadeEnd => fadeEnd;

    public void FadeSetEnd()
    {
        fadeEnd = true;
    }

    private void Awake()
    {
        if (fadeBool == true)
            StartCoroutine("FadeOut");
        else if (fadeBool == false)
            StartCoroutine("FadeIn");
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

        fadeEnd = true;
    }

    private IEnumerator FadeOut()
    {
        Color color = fadeImage.color;
        
        while (color.a >= 0.0f)
        {
            color.a -= Time.deltaTime * fadeSpeed;
            fadeImage.color = color;

            yield return null;
        }

        if (color.a <= 0.0f)
        {
            fadeEnd = true;
            Destroy(gameObject);
        }
    }


}
