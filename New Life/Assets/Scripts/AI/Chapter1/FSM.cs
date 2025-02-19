using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public enum State
{
    Idle, 
    Walk, 
    Attack, 
    Damage, 
    Dead
}

public class FSM : MonoBehaviour
{
    public State CurrentState;
    public Transform player;
    //判断敌人自身与玩家之间的距离
    public float distance;

    private void Start()
    {
        //默认状态下为站立
        CurrentState = State.Idle;
        player = GameObject.FindWithTag("Player").transform;
        
    }

    private void Update()
    {   //实时获取位置信息
        distance = Vector3.Distance(player.transform.position, transform.position);
        //当前状态是哪个就执行状态下的方法 并且是不断调用的
        switch (CurrentState)
        {
            case State.Idle:
                StateIdle();
                break;
            case State.Walk:
                StateWalk();
                break;
            case State.Attack:
                StateAttack();
                break;
            case State.Damage:
                StateDamage();
                break;
            case State.Dead:
                StateDead();
                break;
        }
    }

    //在不同状态下行为都是不同 通过virtual方便重写
    public virtual void StateIdle()
    {

    }

    public virtual void StateWalk()
    {

    }

    public virtual void StateAttack()
    {

    }

    public virtual void StateDamage()
    {

    }

    public virtual void StateDead()
    {

    }

    public virtual void ChangeIdle()
    {
        CurrentState = State.Idle;
    }
    public virtual void ChangeWalk()
    {
        CurrentState = State.Walk;
    }
    public virtual void ChangeAttack()
    {
        CurrentState = State.Attack;
    }
    public virtual void ChangeDamage()
    {
        CurrentState = State.Damage;
    }
    public virtual void ChangeDead()
    {
        CurrentState = State.Dead;
    }
}