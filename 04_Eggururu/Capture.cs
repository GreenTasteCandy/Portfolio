using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class Capture : MonoBehaviour
{
    public Camera cam;
    public RenderTexture rt;
    public Image bg;

    public GameObject[] obj;

    private void Start()
    {
        cam = Camera.main;
    }

    public void Create()
    {
        StartCoroutine(CaptureImage());
    }

    public void Create_()
    {
        StartCoroutine(AllCaptureImage());
    }

    IEnumerator CaptureImage()
    {
        yield return null;

        Texture2D tex = new Texture2D(rt.width, rt.height, TextureFormat.ARGB32, false, true);
        RenderTexture.active = rt;
        tex.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);

        yield return null;

        var data = tex.EncodeToPNG();
        string name = "Thumbnail";
        string extention = ".png";
        string path = Application.persistentDataPath + "/Tumbnail/";

        Debug.Log(path);

        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);

        File.WriteAllBytes(path + name + extention, data);
    }


    IEnumerator AllCaptureImage()
    {
        int num = 0;
        while (num < obj.Length) 
        {
            obj[num].SetActive(true);

            yield return null;

            Texture2D tex = new Texture2D(rt.width, rt.height, TextureFormat.ARGB32, false, true);
            RenderTexture.active = rt;
            tex.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);

            yield return null;

            var data = tex.EncodeToPNG();
            string name = $"Thumbnail_{obj[num].gameObject.name}";
            string extention = ".png";
            string path = Application.persistentDataPath + "/Tumbnail/";

            Debug.Log(path);

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            File.WriteAllBytes(path + name + extention, data);

            yield return null;

            obj[num].SetActive(false);
            num++;

            yield return null;
        }
    }
}
