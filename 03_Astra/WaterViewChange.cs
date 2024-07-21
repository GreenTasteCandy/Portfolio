using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterViewChange : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Water"))
        {
            GameManager.ins.waterView.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Water"))
        {
            GameManager.ins.waterView.SetActive(false);
        }
    }
}
