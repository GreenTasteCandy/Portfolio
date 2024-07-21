using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class EffectSystem : MonoBehaviourPun
{
    PhotonView pv;
    void Start()
    {
        pv = GetComponent<PhotonView>();

        Invoke("DestroyEffect", 1f);
    }

    void DestroyEffect()
    {
        pv.RPC("ObjectDestroy", RpcTarget.AllBuffered);
    }

    [PunRPC]
    void ObjectDestroy()
    {
        Destroy(gameObject);
    }

}
