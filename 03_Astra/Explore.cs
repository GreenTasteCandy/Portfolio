using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering.Universal;

public class Explore : MonoBehaviour
{
    public UniversalRendererData URPasset;
    public int planetNum;
    [SerializeField]
    GameObject buttonBacktoLobby;
    [SerializeField]
    GameObject planetInfoUI;

    [Header("Planet Info")]
    [SerializeField]
    TextMeshProUGUI textPlanetName;
    [SerializeField]
    TextMeshProUGUI textPlanetDifficulty;
    [SerializeField]
    Image imagePlanet;
    [SerializeField]
    GameObject itemScrollView;
    [SerializeField]
    GameObject requestScrollView;
    [SerializeField]
    GameObject textRequestIsNothing;

    public GameObject itemSlotPrefab;
    public List<int> itemNum;
    public List<GameObject> itemNotice;
    public GameObject requestSlotPrefab;
    public List<int> requestNum;
    public List<GameObject> requestNotice;
    bool isPut;

    /*탐사 시작*/
    public void GoPlanet()
    {
        if (planetNum == 0)
        {
            LoadingSystem.MoveScene("Planet01");
        }
        else if (planetNum == 1)
        {
            LoadingSystem.MoveScene("Planet02");
        }
        else if (planetNum == 2)
        {
            LoadingSystem.MoveScene("Planet03");
        }
    }

    /*행성 정보 표시*/
    public void OnPlanetInfo(int num)
    {
        planetNum = num;
        planetInfoUI.SetActive(true);
        buttonBacktoLobby.SetActive(false);

        if (planetNum == 0)
        {
            textPlanetName.text = "메테르";
            textPlanetDifficulty.text = "난이도 : 하";
            imagePlanet.sprite = Resources.Load<Sprite>("Planets/Metre");
        }
        else if (planetNum == 1)
        {
            textPlanetName.text = "디프";
            textPlanetDifficulty.text = "난이도 : 중";
            imagePlanet.sprite = Resources.Load<Sprite>("Planets/Def");
        }
        else if (planetNum == 2)
        {
            textPlanetName.text = "코퀴토스";
            textPlanetDifficulty.text = "난이도 : 상";
            imagePlanet.sprite = Resources.Load<Sprite>("Planets/Cocytus");
        }

        SortItemSlot();
        SortRequestSlot();
    }

    /*행성 출현 아이템 표시*/
    public void SortItemSlot()
    {
        if (itemNotice != null)
        {
            for (int j = 0; j < itemNotice.Count; j++)
            {
                Destroy(itemNotice[j]);
            }
        }

        itemNum.Clear();

        for (int i = 0; i < GameSetting.ins.itemTable.items.Length; i++)
        {
            if (GameSetting.ins.itemTable.items[i].name != "")
            {
                if (planetNum == 0 && GameSetting.ins.itemTable.items[i].appearingPlanet == AppearingPlanet.Meter) { itemNum.Add(i); }
                else if (planetNum == 1 && GameSetting.ins.itemTable.items[i].appearingPlanet == AppearingPlanet.Def) { itemNum.Add(i); }
                else if (planetNum == 2 && GameSetting.ins.itemTable.items[i].appearingPlanet == AppearingPlanet.Cocytus) { itemNum.Add(i); }
                else if (GameSetting.ins.itemTable.items[i].appearingPlanet == AppearingPlanet.None) { itemNum.Add(i); }
            }
        }

        for (int i = 0; i < itemNum.Count; i++)
        {
            GameObject itemSlot = Instantiate(itemSlotPrefab);
            itemSlot.GetComponent<RectTransform>().sizeDelta = new Vector2(110, 110);
            itemSlot.GetComponent<ItemSlotView>().slotNum = itemNum[i];
            itemSlot.transform.SetParent(itemScrollView.transform);
            itemSlot.GetComponent<RectTransform>().localScale = Vector3.one;
            itemNotice.Add(itemSlot);
        }
    }

    /*행성에서 수행 가능한 의뢰 표시*/
    public void SortRequestSlot()
    {
        if (requestNotice != null)
        {
            for (int j = 0; j < requestNotice.Count; j++)
            {
                Destroy(requestNotice[j]);
            }
        }

        requestNum.Clear();

        for (int i = 0; i < GameSetting.ins.requestTable.requests.Length; i++)
        {
            isPut = false;

            if (GameSetting.ins.requestTable.requests[i].requestStatus == RequestStatus.Progressing && GameSetting.ins.requestTable.requests[i].requestIndex != "" && isPut == false)
            {
                for (int j = 0; j < GameSetting.ins.requestTable.requests[i].requestTargets.Length; j++)
                {
                    if (GameSetting.ins.requestTable.requests[i].requestTargets[j].targetType == RequestTargetType.Collect && GameSetting.ins.requestTable.requests[i].requestTargets[j].targetItemCountGoal > 0)
                    {
                        int num = GameSetting.ins.requestTable.requests[i].requestTargets[j].targetItemIndex;
                        if (planetNum == 0 && GameSetting.ins.itemTable.items[num].appearingPlanet == AppearingPlanet.Meter)
                        {
                            requestNum.Add(i);
                            isPut = true;
                            break;
                        }
                        else if (planetNum == 1 && GameSetting.ins.itemTable.items[num].appearingPlanet == AppearingPlanet.Def)
                        {
                            requestNum.Add(i);
                            isPut = true;
                            break;
                        }
                        else if (planetNum == 2 && GameSetting.ins.itemTable.items[num].appearingPlanet == AppearingPlanet.Cocytus)
                        {
                            requestNum.Add(i);
                            isPut = true;
                            break;
                        }
                        else if (GameSetting.ins.itemTable.items[num].appearingPlanet == AppearingPlanet.None)
                        {
                            requestNum.Add(i);
                            isPut = true;
                            break;
                        }
                    }

                    else if (GameSetting.ins.requestTable.requests[i].requestTargets[j].targetType == RequestTargetType.Hunt && GameSetting.ins.requestTable.requests[i].requestTargets[j].targetEnemyCountGoal > 0)
                    {
                        int num = GameSetting.ins.requestTable.requests[i].requestTargets[j].targetEnemyIndex;
                        if (planetNum == 0 && num >= 0 && num < 3)
                        {
                            requestNum.Add(i);
                            isPut = true;
                            break;
                        }
                        else if (planetNum == 1 && num >= 3 && num < 6)
                        {
                            requestNum.Add(i);
                            isPut = true;
                            break;
                        }
                        else if (planetNum == 2 && num >= 6 && num < 9)
                        {
                            requestNum.Add(i);
                            isPut = true;
                            break;
                        }
                    }
                }
            }
        }

        for (int i = 0; i < requestNum.Count; i++)
        {
            GameObject requestSlot = Instantiate(requestSlotPrefab);
            requestSlot.GetComponent<RequestSlotView>().slotNum = requestNum[i];
            requestSlot.transform.SetParent(requestScrollView.transform);
            requestSlot.GetComponent<RectTransform>().localScale = Vector3.one;
            requestSlot.GetComponent<Button>().interactable = false;
            requestNotice.Add(requestSlot);
        }

        if (requestNum.Count > 0) { textRequestIsNothing.SetActive(false); }
        else { textRequestIsNothing.SetActive(true); }
    }

    /*행성 정보 팝업 닫기*/
    public void OffPlanetInfo()
    {
        buttonBacktoLobby.SetActive(true);
        planetInfoUI.SetActive(false);
    }
}
