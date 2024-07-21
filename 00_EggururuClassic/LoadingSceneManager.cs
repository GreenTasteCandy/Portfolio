using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
//LoadingSceneManager�� �ε� ȭ�鿡 ���� �ý����Դϴ�

public class LoadingSceneManager : MonoBehaviour
{
    //���� �ʱ�ȭ �κ�
    public static string nextScene;

    [SerializeField]
    Image progressBar;

    TextMeshProUGUI TextLoad;

    bool progressCheck = false;

    //�ε� ���۽� Ŭ���� ���� ����
    private void Start()
    {
        CloudSave.Instance.SaveWithCloud(999);
    }

    //Ŭ���� ���� ������ �ε� ����
    private void Update()
    {
        if (StartGame.GPGSsave == true)
        {
            StartCoroutine(LoadSceneProgress());
            progressCheck = true;
            StartGame.GPGSsave = false;
        }
    }

    //�ε��� ȭ�鿡 ���̴� ����
    private void LateUpdate()
    {
        if (progressCheck == true)
            TextLoad.text = string.Format("{0:n0}", "���� ȭ������ �Ѿ�� ��.....");
        else if (progressCheck == false)
            TextLoad.text = string.Format("{0:n0}", "�����͸� �����ϴ� ��......");
    }


    public static void LoadScene(string sceneName)
    {
        nextScene = sceneName;
        SceneManager.LoadScene("LoadingScreen");
    }

    //�ε� ó�� �κ�
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