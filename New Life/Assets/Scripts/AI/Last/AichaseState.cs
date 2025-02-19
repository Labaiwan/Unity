using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AichaseState : AiState
{
    private float timer = 0;

    public AiStateId GetId()
    {
        return AiStateId.chase;
    }

    public void Enter(AiAgent agent)
    {
    }

    public void Exit(AiAgent agent)
    {
    }

    public void Update(AiAgent agent)
    {
        
        if (!agent.enabled)
        {
            return;
        }

        timer -= Time.deltaTime;
        if (!agent.navMeshAgent.hasPath)
        {
            agent.navMeshAgent.destination = agent.player.position;
        }

        Vector3 playerDirection = agent.player.position - agent.transform.position;
        //agent.ActivateWeaponIfNecessary();
        agent.stateMachine.ChangeState(AiStateId.attack);
        if (timer < 0)
        {
            Vector3 direction = (agent.player.position - agent.navMeshAgent.destination);
            direction.y = 0;
            if (direction.sqrMagnitude > agent.config.maxDistance * agent.config.maxDistance)
            {
                if (agent.navMeshAgent.pathStatus != NavMeshPathStatus.PathPartial)
                {
                    agent.navMeshAgent.destination = agent.player.position;
                }
            }
            timer = agent.config.maxTime;
        }
        Debug.Log("×·»÷×´Ì¬");
    }
}