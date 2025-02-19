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
            // 装枪动画完成，激活 WeaponIk 脚本并切换到攻击状态
            agent.weaponIk.enabled = true;
            Debug.Log("进入到装枪状态并进入到追击状态");
            agent.stateMachine.ChangeState(AiStateId.chase);

        }
        Debug.Log("装枪状态");
    }

    
}
