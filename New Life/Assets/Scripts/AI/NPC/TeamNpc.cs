using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TeamNpc : MonoBehaviour
{
    private Animator ani;
    private NavMeshAgent agent;
    public Transform targetPos;

    void Start()
    {
        ani = this.GetComponent<Animator>();
        agent = this.GetComponent<NavMeshAgent>();
    }

   
    void Update()
    {
        if (Chapter2Mgr.Instance.isBegin)
        {
            agent.SetDestination(targetPos.position);
            ani.SetBool("run", agent.velocity != Vector3.zero);

            if ((agent.transform.position - targetPos.position).magnitude < 1)
            {
                agent.speed = 0;
                ani.SetBool("crouch", true);
                transform.rotation = targetPos.rotation;
            }
        }
       
    }
}
