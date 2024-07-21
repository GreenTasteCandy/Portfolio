using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EggSkins : MonoBehaviour
{
    public Material[] eggMat;
    public Mesh[] eggMesh;
    public SkinnedMeshRenderer eggSkin;

    private void Start()
    {
        eggSkin.material = eggMat[UserData.ins.data.skinNum];
        eggSkin.sharedMesh = eggMesh[UserData.ins.data.skinNum];
    }

    private void LateUpdate()
    {
        if (SceneManager.GetActiveScene().name == "MainLobby")
        {
            eggSkin.material = eggMat[UserData.ins.data.skinNum];
            eggSkin.sharedMesh = eggMesh[UserData.ins.data.skinNum];
        }
    }

}
