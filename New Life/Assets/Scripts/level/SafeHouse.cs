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
            //֪ͨ�������ҽ��밲ȫ����
            Chapter3.Instance.isSafe = true;
            Debug.Log("��ȫ��");
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
