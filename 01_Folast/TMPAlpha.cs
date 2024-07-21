using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/*�ý��� �ؽ�Ʈ ȿ���� ��ũ��Ʈ*/
public class TMPAlpha : MonoBehaviour
{
    [SerializeField]
    float lerpTime = 3.0f;
    TextMeshProUGUI text;

    private void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    public void FadeOut()
    {
        StartCoroutine(AlphaLerp(1, 0));
    }

    private IEnumerator AlphaLerp(float start, float end)
    {
        float currentTime = 0.0f;
        float percent = 0.0f;

        while (percent < 1)
        {
            currentTime += Time.fixedDeltaTime;
            percent = currentTime / lerpTime;

            Color color = text.color;
            color.a = Mathf.Lerp(start, end, percent);
            text.color = color;

            yield return null;
        }
    }
}
