using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
//EggShop은 상점 시스템입니다

public class EggShop : MonoBehaviour
{
    //변수 초기화 부분
    public Mesh[] MeshSet;

    public Material[] CharMesh;
    public Material[] CharMesh2;
    public Material[] CharMesh3;
    public Material[] CharMesh4;

    public static int EggNumModel = 0;
    public static int EggNum = 0;

    public TextMeshProUGUI Shop;
    public TextMeshProUGUI ShopText;

    string log;

    // Start is called before the first frame update
    void Update()
    {
        //알 마네킹 모양 설정
        gameObject.GetComponent<MeshFilter>().mesh = MeshSet[EggNumModel];

        if (EggNumModel == 0)
            gameObject.GetComponent<MeshRenderer>().material = CharMesh[EggNum];

        else if (EggNumModel == 1)
            gameObject.GetComponent<MeshRenderer>().material = CharMesh2[EggNum];

        else if (EggNumModel == 2)
            gameObject.GetComponent<MeshRenderer>().material = CharMesh3[EggNum];

        else if (EggNumModel == 3)
            gameObject.GetComponent<MeshRenderer>().material = CharMesh4[EggNum];
    }

    private void LateUpdate()
    {
        //상점 안내용 문구들 설정

        if (EggNumModel == 0)
        {
            if (EggNum == 0)
            {
                if (StartGame.CharSkin == EggNum)
                    ShopText.text = string.Format("{0:n0}", "현재 사용중");
                else
                    ShopText.text = string.Format("{0:n0}", "교체 하기");
            }
            else
            {
                if (StartGame.EggSkinNum[EggNum] == false)
                    ShopText.text = string.Format("{0:n0}", "필요 깃털 : " + 10);
                else if (StartGame.EggSkinNum[EggNum] == true)
                {
                    if (StartGame.CharSkin == EggNum)
                        ShopText.text = string.Format("{0:n0}", "현재 사용중");
                    else
                        ShopText.text = string.Format("{0:n0}", "교체 하기");
                }
            }
        }

        else if (EggNumModel == 1)
        {
            if (StartGame.EggSkinNum2[EggNum] == false)
                ShopText.text = string.Format("{0:n0}", "필요 깃털 : " + 15);
            else if (StartGame.EggSkinNum2[EggNum] == true)
            {
                if (StartGame.CharSkin == EggNum)
                    ShopText.text = string.Format("{0:n0}", "현재 사용중");
                else
                    ShopText.text = string.Format("{0:n0}", "교체 하기");
            }
        }

        else if (EggNumModel == 2)
        {
            if (StartGame.EggSkinNum3[EggNum] == false)
                ShopText.text = string.Format("{0:n0}", "필요 깃털 : " + 22);
            else if (StartGame.EggSkinNum3[EggNum] == true)
            {
                if (StartGame.CharSkin == EggNum)
                    ShopText.text = string.Format("{0:n0}", "현재 사용중");
                else
                    ShopText.text = string.Format("{0:n0}", "교체 하기");
            }
        }

        else if (EggNumModel == 3)
        {
            if (StartGame.EggSkinNum4[EggNum] == false)
                ShopText.text = string.Format("{0:n0}", "필요 깃털 : " + 30);
            else if (StartGame.EggSkinNum4[EggNum] == true)
            {
                if (StartGame.CharSkin == EggNum)
                    ShopText.text = string.Format("{0:n0}", "현재 사용중");
                else
                    ShopText.text = string.Format("{0:n0}", "교체 하기");
            }
        }

    }

    //상점 왼쪽 터치시
    public void ShopLeft()
    {
        EggNum -= 1;

        if (EggNumModel == 0)
        {
            if (EggNum < 0)
            {
                EggNum = 4;
                EggNumModel = 3;
            }
        }
        else if (EggNumModel == 1)
        {
            if (EggNum < 0)
            {
                EggNum = 9;
                EggNumModel = 0;
            }
        }
        else if (EggNumModel == 2)
        {
            if (EggNum < 0)
            {
                EggNum = 4;
                EggNumModel = 1;
            }
        }
        else if (EggNumModel == 3)
        {
            if (EggNum < 0)
            {
                EggNum = 4;
                EggNumModel = 2;
            }
        }
    }

    //상점 오른쪽 터치시
    public void ShopRight()
    {
        EggNum += 1;

        if (EggNumModel == 0)
        {
            if (EggNum >= 10)
            {
                EggNum = 0;
                EggNumModel = 1;
            }
        }
        else if (EggNumModel == 1)
        {
            if (EggNum >= 5)
            {
                EggNum = 0;
                EggNumModel = 2;
            }
        }
        else if (EggNumModel == 2)
        {
            if (EggNum >= 5)
            {
                EggNum = 0;
                EggNumModel = 3;
            }
        }
        else if (EggNumModel == 3)
        {
            if (EggNum >= 5)
            {
                EggNum = 0;
                EggNumModel = 0;
            }
        }
    }

    //상점 캐릭터 구매시
    public void ShopEggBT()
    {
        //노말 스킨 설정
        if (EggNumModel == 0)
        {
            //기본 알 설정
            if (EggNum == 0)
            {
                StartGame.CharMesh = 0;
                StartGame.CharSkin = 0;
            }
            //추가 캐릭터 구매
            else
            {
                //캐릭터를 아직 구매하지 않았다
                if (StartGame.EggSkinNum[EggNum] == false)
                {
                    //돈 확인 후 구매
                    if (StartGame.diamonds >= 10)
                    {
                        StartGame.AchievePoint[15] += 1;
                        AchieveShop();
                        StartGame.CharMesh = 0;
                        StartGame.EggSkinNum[EggNum] = true;
                        StartGame.CharSkin = EggNum;

                        StartGame.diamonds -= 10;
                        CloudSave.Instance.SaveWithCloud(999);
                    }
                }
                //캐릭터를 이미 구매했다
                else if (StartGame.EggSkinNum[EggNum] == true)
                {
                    StartGame.CharMesh = 0;
                    StartGame.CharSkin = EggNum;
                    CloudSave.Instance.SaveWithCloud(999);
                }
            }
        }
        else if (EggNumModel == 1)
        {
            //캐릭터를 아직 구매하지 않았다
            if (StartGame.EggSkinNum2[EggNum] == false)
            {
                if (StartGame.diamonds >= 15)
                {
                    StartGame.AchievePoint[16] += 1;
                    AchieveShop();
                    StartGame.CharMesh = 1;
                    StartGame.EggSkinNum2[EggNum] = true;
                    StartGame.CharSkin = EggNum;
                    
                    StartGame.diamonds -= 15;
                    CloudSave.Instance.SaveWithCloud(999);
                }
            }
            else if (StartGame.EggSkinNum2[EggNum] == true)
            {
                StartGame.CharMesh = 1;
                StartGame.CharSkin = EggNum;
                CloudSave.Instance.SaveWithCloud(999);
            }
         }
        else if (EggNumModel == 2)
        {
            //캐릭터를 아직 구매하지 않았다
            if (StartGame.EggSkinNum3[EggNum] == false)
            {
                if (StartGame.diamonds >= 22)
                {
                    StartGame.AchievePoint[17] += 1;
                    AchieveShop();
                    StartGame.CharMesh = 2;
                    StartGame.EggSkinNum3[EggNum] = true;
                    StartGame.CharSkin = EggNum;

                    StartGame.diamonds -= 22;
                    CloudSave.Instance.SaveWithCloud(999);
                }
            }
            else if (StartGame.EggSkinNum3[EggNum] == true)
            {
                StartGame.CharMesh = 2;
                StartGame.CharSkin = EggNum;
                CloudSave.Instance.SaveWithCloud(999);
            }
        }
        else if (EggNumModel == 3)
        {
            //캐릭터를 아직 구매하지 않았다
            if (StartGame.EggSkinNum4[EggNum] == false)
            {
                if (StartGame.diamonds >= 30)
                {
                    StartGame.AchievePoint[18] += 1;
                    AchieveShop();
                    StartGame.CharMesh = 3;
                    StartGame.EggSkinNum4[EggNum] = true;
                    StartGame.CharSkin = EggNum;

                    StartGame.diamonds -= 30;
                    CloudSave.Instance.SaveWithCloud(999);
                }
            }
            else if (StartGame.EggSkinNum4[EggNum] == true)
            {
                StartGame.CharMesh = 3;
                StartGame.CharSkin = EggNum;
                CloudSave.Instance.SaveWithCloud(999);
            }
        }
    }

    //상점 구매시 업적 해금
    public void AchieveShop()
    {
        if (StartGame.AchievePoint[15] >= 5)
            GPGSBinder.Inst.UnlockAchievement(GPGSIds.achievement_26, success => log = $"{success}");

        if (StartGame.AchievePoint[16] >= 5)
            GPGSBinder.Inst.UnlockAchievement(GPGSIds.achievement_27, success => log = $"{success}");

        if (StartGame.AchievePoint[17] >= 5)
            GPGSBinder.Inst.UnlockAchievement(GPGSIds.achievement_28, success => log = $"{success}");

        if (StartGame.AchievePoint[18] >= 5)
            GPGSBinder.Inst.UnlockAchievement(GPGSIds.achievement_29, success => log = $"{success}");
    }
}
