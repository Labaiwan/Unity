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
        // 获取射线发射源的名字
        string raySourceName = raySource.name;
        Debug.Log("The name of the object that shot the ray is: " + raySourceName);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // 检查碰撞物体是否挂载了vDecalManager脚本
        vDecalManager decalManager = collision.gameObject.GetComponent<vDecalManager>();
        if (decalManager != null)
        {
            // 获取挂载vDecalManager脚本的物体的名字
            string decalManagerObjectName = decalManager.gameObject.name;
            Debug.Log("Cube被挂载vDecalManager的对象碰撞，对象名为: " + decalManagerObjectName);
        }
    }

}
