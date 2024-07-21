using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamLocation : MonoBehaviour
{
    public GameObject[] vc;
    public GameObject[] birds;

    public void CamLocationMove(int num)
    {
        foreach (GameObject cam in vc)
        {
            cam.SetActive(false);
        }
        foreach (GameObject bird in birds)
        {
            bird.SetActive(false);
        }

        vc[num].SetActive(true);
        birds[num].SetActive(true);
    }

}
