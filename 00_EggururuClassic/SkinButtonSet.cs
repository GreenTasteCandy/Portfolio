using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkinButtonSet : MonoBehaviour
{
    public int skinNum;
    public int skinClass;
    public GameObject buttonEdge;

    TextMeshProUGUI buttonText;
    RectTransform buttonRect;
    Image buttonColor;
    Image imageEdge;

    // Start is called before the first frame update
    void Start()
    {
        buttonText = GetComponentInChildren<TextMeshProUGUI>();
        imageEdge = buttonEdge.GetComponent<Image>();
        buttonColor = GetComponent<Image>();
        buttonRect = GetComponent<RectTransform>();
        buttonRect.localRotation = new Quaternion(0, 0, 0, 1);
        buttonRect.localScale = new Vector3(1, 1, 1);
    }

    private void Update()
    {
        if (EggShop.EggNumModel == skinClass && EggShop.EggNum == skinNum)
            imageEdge.enabled = true;
        else
            imageEdge.enabled = false;

        if (skinClass == 0)
        {
            if (skinNum == 0)
                buttonColor.color = Color.white;
            else
            {
                if (StartGame.EggSkinNum[skinNum] == false)
                    buttonColor.color = Color.gray;
                else if (StartGame.EggSkinNum[skinNum] == true)
                    buttonColor.color = Color.white;
            }
        }
        else if (skinClass == 1)
        {
            if (StartGame.EggSkinNum2[skinNum] == false)
                buttonColor.color = Color.gray;
            else if (StartGame.EggSkinNum2[skinNum] == true)
                buttonColor.color = Color.white;
        }
        else if (skinClass == 2)
        {
            if (StartGame.EggSkinNum3[skinNum] == false)
                buttonColor.color = Color.gray;
            else if (StartGame.EggSkinNum3[skinNum] == true)
                buttonColor.color = Color.white;
        }
        else if (skinClass == 3)
        {
            if (StartGame.EggSkinNum4[skinNum] == false)
                buttonColor.color = Color.gray;
            else if (StartGame.EggSkinNum4[skinNum] == true)
                buttonColor.color = Color.white;
        }
    }

    private void LateUpdate()
    {
        //노멀 스킨
        if (skinClass == 0)
        {
            buttonText.color = Color.black;

            if (skinNum == 0)
                buttonText.text = "알";
            else if (skinNum == 1)
                buttonText.text = "계란";
            else if (skinNum == 2)
                buttonText.text = "솜사탕";
            else if (skinNum == 3)
                buttonText.text = "오목눈이";
            else if (skinNum == 4)
                buttonText.text = "고무공";
            else if (skinNum == 5)
                buttonText.text = "베이지";
            else if (skinNum == 6)
                buttonText.text = "바이올렛";
            else if (skinNum == 7)
                buttonText.text = "핫핑크";
            else if (skinNum == 8)
                buttonText.text = "마젠타";
            else if (skinNum == 9)
                buttonText.text = "그레이퍼플";
        }

        else if (skinClass == 1)
        {
            buttonText.color = Color.blue;

            if (skinNum == 0)
                buttonText.text = "망고";
            else if (skinNum == 1)
                buttonText.text = "자몽";
            else if (skinNum == 2)
                buttonText.text = "망고스틴";
            else if (skinNum == 3)
                buttonText.text = "파파야";
            else if (skinNum == 4)
                buttonText.text = "코코넛";
        }

        else if (skinClass == 2)
        {
            buttonText.color = new Color(1, 0, 1);

            if (skinNum == 0)
                buttonText.text = "공룡";
            else if (skinNum == 1)
                buttonText.text = "익룡";
            else if (skinNum == 2)
                buttonText.text = "수장룡";
            else if (skinNum == 3)
                buttonText.text = "와이번";
            else if (skinNum == 4)
                buttonText.text = "드레이크";
        }

        else if (skinClass == 3)
        {
            buttonText.color = Color.yellow;

            if (skinNum == 0)
                buttonText.text = "듀크";
            else if (skinNum == 1)
                buttonText.text = "프린스";
            else if (skinNum == 2)
                buttonText.text = "마퀴스";
            else if (skinNum == 3)
                buttonText.text = "카운트";
            else if (skinNum == 4)
                buttonText.text = "배런";
        }
    }

    public void EggSkinSelet()
    {
        EggShop.EggNumModel = skinClass;
        EggShop.EggNum = skinNum;
    }
}
