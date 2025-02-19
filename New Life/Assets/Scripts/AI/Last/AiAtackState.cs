using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiAtackState : AiState
{
    public void Enter(AiAgent agent)
    {
        agent.weaponIk.SetTargetTransform(agent.player);
        agent.navMeshAgent.stoppingDistance = 5.0f;
    }

    public void Exit(AiAgent agent)
    {
        agent.navMeshAgent.stoppingDistance = 0;
    }

    public AiStateId GetId()
    {
        return AiStateId.attack;
    }

    public void Update(AiAgent agent)
    {
        if (!agent.healthController.isDead)
        {
            agent.navMeshAgent.destination = agent.player.position;
            agent.weaponIk.Shoot();
        }



        //agent.stateMachine.ChangeState(AiStateId.chase);
        //UpdateFiring(agent);
    }

    //private void UpdateFiring(AiAgent agent)
    //{
    //    if (agent.pensor.IsInSight(agent.player.gameObject))
    //    {
    //        agent.ActivateWeaponIfNecessary();
    //    }

    //}
}