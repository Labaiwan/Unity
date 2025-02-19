using Invector;
using MoonSharp.VsCodeDebugger.SDK;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class test : MonoBehaviour
{
    private Animator ani;
    private NavMeshAgent agent;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        ani = GetComponent<Animator>();
    }
    void Update()
    {
        if (agent.hasPath)
        {
            ani.SetFloat("Speed", agent.velocity.magnitude);
        }
        else
        {
            ani.SetFloat("Speed", 0);
        }
    }

    public void RealDead()
    {
        agent.isStopped = true;
        ani.SetFloat("Speed", 0);
        ani.SetBool("Dead",true);
        Destroy(gameObject,3.5f);
    }
}
