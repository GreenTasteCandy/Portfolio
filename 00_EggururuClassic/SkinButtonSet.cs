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
        //��� ��Ų
        if (skinClass == 0)
        {
            buttonText.color = Color.black;

            if (skinNum == 0)
                buttonText.text = "��";
            else if (skinNum == 1)
                buttonText.text = "���";
            else if (skinNum == 2)
                buttonText.text = "�ػ���";
            else if (skinNum == 3)
                buttonText.text = "������";
            else if (skinNum == 4)
                buttonText.text = "����";
            else if (skinNum == 5)
                buttonText.text = "������";
            else if (skinNum == 6)
                buttonText.text = "���̿÷�";
            else if (skinNum == 7)
                buttonText.text = "����ũ";
            else if (skinNum == 8)
                buttonText.text = "����Ÿ";
            else if (skinNum == 9)
                buttonText.text = "�׷�������";
        }

        else if (skinClass == 1)
        {
            buttonText.color = Color.blue;

            if (skinNum == 0)
                buttonText.text = "����";
            else if (skinNum == 1)
                buttonText.text = "�ڸ�";
            else if (skinNum == 2)
                buttonText.text = "����ƾ";
            else if (skinNum == 3)
                buttonText.text = "���ľ�";
            else if (skinNum == 4)
                buttonText.text = "���ڳ�";
        }

        else if (skinClass == 2)
        {
            buttonText.color = new Color(1, 0, 1);

            if (skinNum == 0)
                buttonText.text = "����";
            else if (skinNum == 1)
                buttonText.text = "�ͷ�";
            else if (skinNum == 2)
                buttonText.text = "�����";
            else if (skinNum == 3)
                buttonText.text = "���̹�";
            else if (skinNum == 4)
                buttonText.text = "�巹��ũ";
        }

        else if (skinClass == 3)
        {
            buttonText.color = Color.yellow;

            if (skinNum == 0)
                buttonText.text = "��ũ";
            else if (skinNum == 1)
                buttonText.text = "������";
            else if (skinNum == 2)
                buttonText.text = "������";
            else if (skinNum == 3)
                buttonText.text = "ī��Ʈ";
            else if (skinNum == 4)
                buttonText.text = "�跱";
        }
    }

    public void EggSkinSelet()
    {
        EggShop.EggNumModel = skinClass;
        EggShop.EggNum = skinNum;
    }
}
