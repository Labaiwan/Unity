using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeHouse : MonoBehaviour
{

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {


    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //通知龙卷风玩家进入安全区域
            Chapter3.Instance.isSafe = true;
            Debug.Log("安全中");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Chapter3.Instance.isSafe = false;
        }
    }
}
