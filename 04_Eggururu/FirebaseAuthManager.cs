using UnityEngine;
using Firebase.Auth;
using Firebase.Firestore;
using System;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using Firebase.Extensions;
using Firebase;
using UnityEngine.SceneManagement;
using System.Runtime.CompilerServices;

public class FirebaseAuthManager
{
    private static FirebaseAuthManager Ins = null;
    public static FirebaseAuthManager ins
    {
        get
        {
            if (Ins == null)
            {
                Ins = new FirebaseAuthManager();
            }

            return Ins;
        }
    }

    private FirebaseAuth auth; //�α���,ȸ������ � ���
    private FirebaseUser user;  //������ �Ϸ�� ���� ����
    private FirebaseApp app;
    private FirebaseFirestore db;

    public bool isBuild = false;
    public string userID = "TestUser";//=> user.DisplayName;
    public string logData;

    public bool isCheck = false;
    public bool isLogin = false;
    public Action<bool> LoginState;

    public void Init()
    {

        if (isBuild)
        {
            auth = FirebaseAuth.DefaultInstance;

            if (auth.CurrentUser != null)
            {
                LogOut();
            }
            auth.StateChanged += OnChanged;
        }
        else
        {
            logData = "Play Games 로그인을 시작하는 중....";
            PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder().RequestServerAuthCode(false).Build();
            PlayGamesPlatform.InitializeInstance(config);
            PlayGamesPlatform.Activate();

            FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
            {
                var dependencyStatus = task.Result;
                if (dependencyStatus == DependencyStatus.Available)
                {
                    logData = "로그인을 성공적으로 완료했습니다.";
                    // Create and hold a reference to your FirebaseApp,
                    // where app is a Firebase.FirebaseApp property of your application class.
                    app = FirebaseApp.DefaultInstance;
                    isLogin = true;
                    // Set a flag here to indicate whether Firebase is ready to use by your app.
                }
                else
                {
                    logData = string.Format("Could not resolve all Firebase dependencies: {0}", dependencyStatus);
                    isLogin = false;
                }
            });
        }
    }

    private void OnChanged(object sender, EventArgs e)
    {
        if (auth.CurrentUser != user)
        {
            bool signed = (auth.CurrentUser != user && auth.CurrentUser != null);
            if (!signed && user != null)
            {
                Debug.Log("�α׾ƿ�");
                LoginState?.Invoke(false);
            }

            user = auth.CurrentUser;
            if (signed)
            {
                Debug.Log("�α���");
                LoginState?.Invoke(true);
            }
        }
    }

    public void CreateID(string email, string password)
    {
        auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                logData = "계정 생성을 취소하였습니다.";
                Debug.Log("계정생성 취소");
                isCheck = false;
                return;
            }
            if (task.IsFaulted)
            {
                logData = "계정 생성을 실패하였습니다.";
                Debug.Log("계정생성 실패");
                isCheck = false;
                return;
            }

            user = task.Result.User;
            logData = "계정 생성이 정상적으로 완료 되었습니다.";
            isCheck = true;
            Debug.Log("계정생성 성공");
        });
    }

    public void Login()
    {
        Social.localUser.Authenticate((bool success) => 
        {
            if (success)
            {
                string authCode = PlayGamesPlatform.Instance.GetServerAuthCode();
                auth = FirebaseAuth.DefaultInstance;
                Credential credential = PlayGamesAuthProvider.GetCredential(authCode);
                auth.SignInAndRetrieveDataWithCredentialAsync(credential).ContinueWith(task =>
                {
                    if (task.IsCanceled)
                    {
                        logData = "SignInAndRetrieveDataWithCredentialAsync was canceled.";
                        return;
                    }
                    if (task.IsFaulted)
                    {
                        logData = "SignInAndRetrieveDataWithCredentialAsync encountered an error: " + task.Exception;
                        return;
                    }
                    AuthResult result = task.Result;
                    logData = string.Format("User signed in successfully: {0} ({1})", result.User.DisplayName, result.User.UserId);
                });

                user = auth.CurrentUser;
                isLogin = true;
            }
            else
            {
                logData = "로그인을 다시 시작하는 중....";
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        });
    }

    public void LoginEmail(string email, string password)
    {
        auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                logData = "로그인을 취소하였습니다.";
                Debug.Log("로그인 취소");
                return;
            }
            if (task.IsFaulted)
            {
                logData = "로그인을 하는데 실패하였습니다.";
                Debug.Log("로그인 실패");
                return;
            }
            FirebaseUser newUser = task.Result.User;
            logData = "로그인이 완료 되었습니다.";
            Debug.Log("로그인 완료");
            isLogin = true;
        });
    }

    public string GetUserID() { return "TestUser"; }//user.UserId; }

    public void LogOut()
    {
        isLogin = false;
        auth.SignOut();
        Debug.Log("�α׾ƿ�");
    }

    public void NickNameSet(string nickname)
    {
        if (user != null)
        {
            UserProfile profile = new UserProfile
            {
                DisplayName = nickname
            };

            user.UpdateUserProfileAsync(profile).ContinueWith(task =>
            {

                if (task.IsCanceled)
                {
                    Debug.LogError("UpdateUserProfileAsync was canceled.");
                    return;
                }
                if (task.IsFaulted)
                {
                    Debug.LogError("UpdateUserProfileAsync encountered an error: " + task.Exception);
                    return;
                }

                Debug.Log("User profile updated successfully.");

            });
        }

    }
}