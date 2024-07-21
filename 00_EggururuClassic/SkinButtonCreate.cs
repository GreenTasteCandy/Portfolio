using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinButtonCreate : MonoBehaviour
{
    public GameObject ButtonSkin;

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < 10; i++)
        {
            GameObject button = Instantiate(ButtonSkin, this.transform.position, new Quaternion(0, 0, 0, 1));
            button.transform.SetParent(this.transform);
            button.GetComponent<SkinButtonSet>().skinNum = i;
            button.GetComponent<SkinButtonSet>().skinClass = 0;
        }

        for (int i = 0; i < 5; i++)
        {
            GameObject button = Instantiate(ButtonSkin, this.transform.position, new Quaternion(0, 0, 0, 1));
            button.transform.SetParent(this.transform);
            button.GetComponent<SkinButtonSet>().skinNum = i;
            button.GetComponent<SkinButtonSet>().skinClass = 1;
        }

        for (int i = 0; i < 5; i++)
        {
            GameObject button = Instantiate(ButtonSkin, this.transform.position, new Quaternion(0, 0, 0, 1));
            button.transform.SetParent(this.transform);
            button.GetComponent<SkinButtonSet>().skinNum = i;
            button.GetComponent<SkinButtonSet>().skinClass = 2;
        }

        for (int i = 0; i < 5; i++)
        {
            GameObject button = Instantiate(ButtonSkin, this.transform.position, new Quaternion(0, 0, 0, 1));
            button.transform.SetParent(this.transform);
            button.GetComponent<SkinButtonSet>().skinNum = i;
            button.GetComponent<SkinButtonSet>().skinClass = 3;
        }
    }
}
