using MoonSharp.VsCodeDebugger.SDK;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiEquipWeaponState : AiState
{
    

    public AiStateId GetId()
    {
        return AiStateId.equipWeapon;
    }

    public void Enter(AiAgent agent)
    {
        agent.animator.SetTrigger("Equip");
    }

    public void Exit(AiAgent agent)
    {
    }

    public void Update(AiAgent agent)
    {
        if (agent.animator.GetCurrentAnimatorStateInfo(1).normalizedTime >= 1f)
        {
            // װǹ������ɣ����� WeaponIk �ű����л�������״̬
            agent.weaponIk.enabled = true;
            Debug.Log("���뵽װǹ״̬�����뵽׷��״̬");
            agent.stateMachine.ChangeState(AiStateId.chase);

        }
        Debug.Log("װǹ״̬");
    }

    
}
