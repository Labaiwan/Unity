using Invector;
using Invector.vShooter;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cubeTest : MonoBehaviour
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
        GameObject raySource = other.GetComponentInParent<Rigidbody>().gameObject;
        // ��ȡ���߷���Դ������
        string raySourceName = raySource.name;
        Debug.Log("The name of the object that shot the ray is: " + raySourceName);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // �����ײ�����Ƿ������vDecalManager�ű�
        vDecalManager decalManager = collision.gameObject.GetComponent<vDecalManager>();
        if (decalManager != null)
        {
            // ��ȡ����vDecalManager�ű������������
            string decalManagerObjectName = decalManager.gameObject.name;
            Debug.Log("Cube������vDecalManager�Ķ�����ײ��������Ϊ: " + decalManagerObjectName);
        }
    }

}
