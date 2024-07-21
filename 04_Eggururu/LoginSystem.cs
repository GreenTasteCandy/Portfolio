using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;
using GoogleMobileAds.Api;
using System.Collections;

public class LoginSystem : MonoBehaviour
{
    public TextMeshProUGUI logText;
    public AudioSource bgm;

    public TextMeshProUGUI errorText;
    public GameObject createObj;
    public TMP_InputField email_c;
    public TMP_InputField password_c;

    public GameObject loginObj;
    public TMP_InputField email;
    public TMP_InputField password;

    public GameObject nameSet;
    public TMP_InputField nickName;

    bool isNickName = false;

    private void Awake()
    {
        Application.targetFrameRate = 60;
        FirebaseAuthManager.ins.Init();

        if (FirebaseAuthManager.ins.isBuild == false)
        {
            FirebaseAuthManager.ins.Login();
        }
        else
        {
            //loginObj.SetActive(true);
        }

        MobileAds.Initialize((InitializationStatus initStatus) =>
        {
            if (initStatus != null)
                Debug.Log("Google Admob On");
        });
    }

    private void Start()
    {
        bgm.volume = UserData.ins.data.bgmVolume;
    }

    private void LateUpdate()
    {
        if (FirebaseAuthManager.ins.isBuild)
        {
            if (FirebaseAuthManager.ins.isLogin == true)
            {
                errorText.text = "화면을 터치하면 게임이 시작됩니다.";
            }
            else
            {
                errorText.text = FirebaseAuthManager.ins.logData;
            }
        }
        else
        {
            if (FirebaseAuthManager.ins.isLogin == true)
            {
                logText.text = "화면을 터치하면 게임이 시작됩니다.";
            }
            else
            {
                logText.text = FirebaseAuthManager.ins.logData;
            }
        }
    }

    private void Update()
    {
        if (FirebaseAuthManager.ins.isCheck == true && nameSet.activeSelf == false)
        {
            nameSet.SetActive(true);
            FirebaseAuthManager.ins.isCheck = false;
        }

        if (FirebaseAuthManager.ins.isLogin == false)
        {
            //return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            MoveLobby();
        }
    }

    public void CreateID()
    {
        string e = email_c.text;
        string p = password_c.text;

        FirebaseAuthManager.ins.CreateID(e, p);
    }

    public void LoginEmail()
    {
        string e = email.text;
        string p = password.text;

        FirebaseAuthManager.ins.LoginEmail(e, p);
    }

    public void LogoutEmail()
    {
        FirebaseAuthManager.ins.LogOut();
    }

    public void NickNameSet()
    {
        FirebaseAuthManager.ins.NickNameSet(nickName.text);
        nameSet.SetActive(false);
        createObj.SetActive(false);
    }

    public void MoveLobby()
    {
        if (FirebaseAuthManager.ins.isLogin)
        {
            DatabaseManager.ins.Init();
            FirestoreManager.ins.Init();

            LoadingSystem.MoveScene("MainLobby");
        }
    }
}