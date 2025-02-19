using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SoldierNpc : MonoBehaviour
{
    private Animator ani; 
    private NavMeshAgent agent;
    //寻路点
    public Transform pos;
    // 上一次攻击的时间
    private float frontTime;
    void Start()
    {
        ani = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        agent.SetDestination(pos.position);
        ani.SetInteger("State", 2);
        if (!Chapter2Mgr.Instance.isBegin)
        {
            
        }

        if (Vector3.Distance(agent.destination, agent.nextPosition) <= 0.05f)
        {

            ani.SetInteger("State", 0);
            if (Time.time - frontTime >= 2)
            {
                // 记录这次攻击时的时间
                frontTime = Time.time;
                // 到达目的地
                ani.SetTrigger("Attack");
            }


        }

    }
}
