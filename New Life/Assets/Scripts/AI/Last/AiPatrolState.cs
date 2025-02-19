using System.Collections;
using UnityEngine;

public class AiPatrolState : AiState
{
    private int currentPointIndex = 0;
    private float waitTime = 2.0f;
    private float waitTimer;
    //用于存储原始速度
    private float originalSpeed;
    public AiStateId GetId()
    {
        return AiStateId.patrol;
    }

    public void Enter(AiAgent agent)
    {
        //保存原始速度
        originalSpeed = agent.navMeshAgent.speed; 
        agent.navMeshAgent.speed = 1.76f;
        if (agent.patrolPoints.Length > 0)
        {
            agent.navMeshAgent.destination = agent.patrolPoints[currentPointIndex].position;
        }
    }

    public void Exit(AiAgent agent)
    {
        agent.navMeshAgent.speed = originalSpeed;    
    }

    public void Update(AiAgent agent)
    {
        if (agent.navMeshAgent.remainingDistance <= agent.navMeshAgent.stoppingDistance)
        {
            waitTimer += Time.deltaTime;
            if (waitTimer >= waitTime)
            {
                waitTimer = 0f;
                currentPointIndex = (currentPointIndex + 1) % agent.patrolPoints.Length;
                agent.navMeshAgent.destination = agent.patrolPoints[currentPointIndex].position;
            }
        }
        agent.ActivateWeaponIfNecessary();
    }
}