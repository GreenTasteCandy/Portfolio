using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PostProcesing : MonoBehaviour
{
    public PostProcessLayer ppl;
    public PostProcessVolume ppv;
    private float delayTime = 0.5f;

    public void ZaWarudo()
    {
        StartCoroutine(TheWorld());
    }

    IEnumerator TheWorld()
    {
        ppl.enabled = true;
        ppv.enabled = true;
        float delay = 0f;
        yield return null;

        while (delay < delayTime)
        {
            delay += Time.deltaTime;

            ppv.profile.GetSetting<ColorGrading>().hueShift.value += Time.deltaTime * 360f;
            yield return new WaitForSeconds(Time.deltaTime);
        }

        delay = 0f;

        while (delay < delayTime)
        {
            delay += Time.deltaTime;

            ppv.profile.GetSetting<ColorGrading>().saturation.value -= Time.deltaTime * 200;
            yield return new WaitForSeconds(Time.deltaTime);
        }

    }

    public void Jotaro()
    {
        StartCoroutine(StarPlatina());
    }

    IEnumerator StarPlatina()
    {
        float delay = 0f;
        ppv.profile.GetSetting<ColorGrading>().hueShift.value = 0f;

        while (delay < delayTime)
        {
            delay += Time.deltaTime;

            ppv.profile.GetSetting<ColorGrading>().saturation.value += Time.deltaTime * 200;
            yield return new WaitForSeconds(Time.deltaTime);
        }
        ppv.profile.GetSetting<ColorGrading>().saturation.value = 0f;

        ppl.enabled = false;
        ppv.enabled = false;
    }
}
