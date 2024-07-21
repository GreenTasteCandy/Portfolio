using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BirdSkins : MonoBehaviour
{
    public GameObject[] hair;
    public GameObject[] face;
    public GameObject[] shoes;

    public Material[] Mat;

    GameObject hairNow;
    GameObject faceNow;
    GameObject shoesNow;

    private void LateUpdate()
    {
#if UNITY_ANDROID_
        if (SceneManager.GetActiveScene().name == "MainLobby")
        {
            if (UserData.ins.data.birdHairNow != 0)
            {
                int num = UserData.ins.data.birdHairNow;
                hairNow.GetComponent<MeshRenderer>().material = Mat[DataTable.ins.birdWear[num].matNum];
            }

            if (UserData.ins.data.birdFaceNow != 0)
            {
                int num = UserData.ins.data.birdFaceNow;
                faceNow.GetComponent<MeshRenderer>().material = Mat[DataTable.ins.birdWear[num].matNum];
            }

            if (UserData.ins.data.birdShoesNow != 0)
            {
                int num = UserData.ins.data.birdShoesNow;
                shoesNow.GetComponent<MeshRenderer>().material = Mat[DataTable.ins.birdWear[num].matNum];
            }
        }
#endif
    }
}
