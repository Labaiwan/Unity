using Invector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    public bool isDead = false;
    
    // 获取塔的所有子物体
    public List<Transform> GetChildren()
    {
        List<Transform> children = new List<Transform>();
        foreach (Transform child in transform)
        {
            children.Add(child);
        }
        return children;
    }


    public void Dead()
    {
        isDead = true;
        UIDataMgr.Instance.showPanel<FailPanel>();
    }
}
