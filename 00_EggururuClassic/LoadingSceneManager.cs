using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
//LoadingSceneManager는 로딩 화면에 대한 시스템입니다

public class LoadingSceneManager : MonoBehaviour
{
    //변수 초기화 부분
    public static string nextScene;

    [SerializeField]
    Image progressBar;

    TextMeshProUGUI TextLoad;

    bool progressCheck = false;

    //로딩 시작시 클라우드 저장 시작
    private void Start()
    {
        CloudSave.Instance.SaveWithCloud(999);
    }

    //클라우드 저장 종료후 로딩 시작
    private void Update()
    {
        if (StartGame.GPGSsave == true)
        {
            StartCoroutine(LoadSceneProgress());
            progressCheck = true;
            StartGame.GPGSsave = false;
        }
    }

    //로딩시 화면에 보이는 문구
    private void LateUpdate()
    {
        if (progressCheck == true)
            TextLoad.text = string.Format("{0:n0}", "다음 화면으로 넘어가는 중.....");
        else if (progressCheck == false)
            TextLoad.text = string.Format("{0:n0}", "데이터를 저장하는 중......");
    }


    public static void LoadScene(string sceneName)
    {
        nextScene = sceneName;
        SceneManager.LoadScene("LoadingScreen");
    }

    //로딩 처리 부분
    IEnumerator LoadSceneProgress()
    {
        yield return null;

        AsyncOperation op = SceneManager.LoadSceneAsync(nextScene);
        op.allowSceneActivation = false;

        float timer = 0.0f;
        while (!op.isDone)
        {
            yield return null;

            timer += Time.deltaTime; 
            if (op.progress < 0.9f) 
            { 
                progressBar.fillAmount = Mathf.Lerp(progressBar.fillAmount, op.progress, timer); 
                if (progressBar.fillAmount >= op.progress) 
                { timer = 0f; } 
            } 
            else 
            { 
                progressBar.fillAmount = Mathf.Lerp(progressBar.fillAmount, 1f, timer); 
                if (progressBar.fillAmount == 1.0f) 
                { 
                    op.allowSceneActivation = true; 
                    yield break;
                } 
            }
        }

    }

}