using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SoldierNpc : MonoBehaviour
{
    private Animator ani; 
    private NavMeshAgent agent;
    //Ѱ·��
    public Transform pos;
    // ��һ�ι�����ʱ��
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
                // ��¼��ι���ʱ��ʱ��
                frontTime = Time.time;
                // ����Ŀ�ĵ�
                ani.SetTrigger("Attack");
            }


        }

    }
}
