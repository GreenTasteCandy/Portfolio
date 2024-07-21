using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/*
 * 아이템 슬롯 표시 스크립트
 */

public class PlanetSlotView : MonoBehaviour
{
    public Image imagePlanet;
    public TextMeshProUGUI textPlanet;
    public int planetNum;
    public string planetText;

    private void LateUpdate()
    {
        if (planetNum == 0) { imagePlanet.sprite = Resources.Load<Sprite>("Planets/Metre"); }
        else if (planetNum == 1) { imagePlanet.sprite = Resources.Load<Sprite>("Planets/Def"); }
        else if (planetNum == 2) { imagePlanet.sprite = Resources.Load<Sprite>("Planets/Cocytus"); }

        textPlanet.text = planetText;
    }
}
