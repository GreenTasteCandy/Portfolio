using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//meshEgg�� �÷��̾� ĳ������ ���� �Դϴ�

public class meshEgg : MonoBehaviour
{
    public Mesh[] MeshSet;

    public Material[] CharMesh;
    public Material[] CharMesh2;
    public Material[] CharMesh3;
    public Material[] CharMesh4;

    void Update()
    {
        gameObject.GetComponent<MeshFilter>().mesh = MeshSet[StartGame.CharMesh];

        if (StartGame.CharMesh == 0)
            gameObject.GetComponent<MeshRenderer>().material = CharMesh[StartGame.CharSkin];

        else if (StartGame.CharMesh == 1)
            gameObject.GetComponent<MeshRenderer>().material = CharMesh2[StartGame.CharSkin];

        else if (StartGame.CharMesh == 2)
            gameObject.GetComponent<MeshRenderer>().material = CharMesh3[StartGame.CharSkin];

        else if (StartGame.CharMesh == 3)
            gameObject.GetComponent<MeshRenderer>().material = CharMesh4[StartGame.CharSkin];
    }
}
 