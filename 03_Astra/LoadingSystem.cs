using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Rendering.Universal;

public class LoadingSystem : MonoBehaviour
{
    //변수 초기화 부분
    public static string nextScene;

    public UniversalRendererData URPasset;
    [SerializeField]
    Image progressBar;

    private void Start()
    {
        StopCoroutine(LoadSceneProgress());
        StartCoroutine(LoadSceneProgress());
    }

    public static void MoveScene(string sceneName)
    {
        nextScene = sceneName;
        SceneManager.LoadScene("LoadingScene");
    }

    //로딩 처리 부분
    IEnumerator LoadSceneProgress()
    {
        yield return null;

        if (nextScene == "Planet01")
        {
            ScriptableRendererFeature srf = URPasset.rendererFeatures[0];
            srf.SetActive(true);
        }

        AsyncOperation op = SceneManager.LoadSceneAsync(nextScene);
        op.allowSceneActivation = false;

        float timer = 0.0f;
        while (!op.isDone)
        {
            yield return null;

            timer += Time.deltaTime;
            if (op.progress < 0.9f)
            {
                progressBar.fillAmount = Mathf.Lerp(progressBar.fillAmount, 1f, timer);
                if (progressBar.fillAmount >= op.progress)
                {
                    timer = 0.0f;
                }
            }
            else
            {
                progressBar.fillAmount = Mathf.Lerp(progressBar.fillAmount, 1f, timer);
                if (progressBar.fillAmount >= 0.9f)
                {
                    op.allowSceneActivation = true;
                }
            }
        }

    }
}
