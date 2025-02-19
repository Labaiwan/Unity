using Invector;
using MoonSharp.VsCodeDebugger.SDK;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;

public class AiAgent : MonoBehaviour
{
    [HideInInspector] public AiStateMachine stateMachine;
    public AiStateId initialState;
    [HideInInspector] public NavMeshAgent navMeshAgent;
    [HideInInspector] public Transform player;
    [HideInInspector] public WeaponIk weaponIk;
    [HideInInspector] public Animator animator;
    [HideInInspector] public EnemyVision vision;
    [HideInInspector] public vHealthController healthController;
    //巡逻点
    public Transform[] patrolPoints;
    public AiAgentconfig config;
    private bool isChase = false;
    public int ScopeNotice = 5;
    void Start()
    {
        animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        weaponIk = GetComponent<WeaponIk>();
        vision = GetComponent<EnemyVision>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        healthController = GetComponent<vHealthController>();
        stateMachine = new AiStateMachine(this);
        stateMachine.RegisterState(new AichaseState());
        stateMachine.RegisterState(new AiIdleState());
        stateMachine.RegisterState(new AiAtackState());
        stateMachine.RegisterState(new AiEquipWeaponState());
        stateMachine.RegisterState(new AiPatrolState());
        stateMachine.ChangeState(initialState);
        weaponIk.enabled = false;
    }

    
    void Update()
    {
        stateMachine.Update();
        // 检测周围是否有其他AI代理
        Collider[] colliders = Physics.OverlapSphere(transform.position, ScopeNotice, 1 << LayerMask.NameToLayer("Enemy"));
        foreach (Collider collider in colliders)
        {
            if (collider != null && collider.gameObject != this.gameObject)
            {
                AiAgent otherAgent = collider.GetComponent<AiAgent>();
                if (otherAgent != null)
                {
                    if (isChase)
                    {
                        //通知其他AI代理激活武器
                        otherAgent.stateMachine.ChangeState(AiStateId.equipWeapon);
                        isChase = false;
                    }
                }
            }
        }
        if (healthController.isDead)
        {
            navMeshAgent.isStopped = true;
        }

    }

    public void ActivateWeaponIfNecessary()
    {
        if (vision.PlayerInSight || healthController.currentHealth < healthController.maxHealth)
        {
            stateMachine.ChangeState(AiStateId.equipWeapon);
            isChase = true;
        }
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, ScopeNotice);
    }
}
