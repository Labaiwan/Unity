using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Robot_FSM : FSM
{
    public GameObject[] Target;//用来存放敌人巡逻时的位置坐标，一般两个以上
    private Animator ani;//可以放在FSM里面，但是这里我还没确定下来后续的动画是不是都有5种。
    private NavMeshAgent agent;//我通过NavMesh导航
    private Coroutine async;//协程，很大作用
    private bool AttackLock;//攻击锁，防止重复攻击
    private int i = 0;
    private float time = 5.0f;
    private float CD = 2f;

    private void Awake()
    {
        ani = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        AttackLock = false;//开始时是锁住的
    }

    public override void StateIdle()
    {
        agent.speed = 2.0f;//AI导航速度为2.0F，一般每个状态下都需要编写不同的速度。
        time -= Time.deltaTime;
        if (time <= 0)
        {
            time = 5.0f;
            i = Random.Range(0, Target.Length);//计时器，再不同的巡逻点之间进行随机选择性巡逻
            agent.SetDestination(Target[i].transform.position);//朝向目的移动
        }


        //根据速度来决定是否走路
        ani.SetBool("Walk", agent.velocity != Vector3.zero);
        //0咆哮 0.5走路 1跑步
        ani.SetFloat("VSpeed", 0.5f);

        if (distance < 5)
        {
            ChangeWalk();
        }


    }

    public override void StateWalk()
    {
        float speedPercent = agent.velocity.magnitude / agent.speed;//算是取当前速度的标准化数值
        agent.speed = 5.0f;//追击状态下速度升到5.0f
        agent.SetDestination(player.transform.position);//敌人开始朝着玩家的位置
        ani.SetFloat("Speed", speedPercent, 0.1f, Time.deltaTime);//把速度传入动画控制器，
        if (distance < 2)//判断距离小于2，开始攻击
        {
            ChangeAttack();
        }
    }

    public override void StateAttack()
    {
        if (AttackLock == false)//没上锁的情况下才能进行攻击。
        {
            int j = Random.Range(0, 2);//随机数判断采用哪个攻击动画
            AttackLock = true;//攻击开始了就锁住
            agent.speed = 0f;//必须在每个动画设置速度，不然会保持前一个状态下的速度
            gameObject.transform.LookAt(player.transform.position);//朝向玩家攻击
            if (j == 0) ani.SetTrigger("Attack1");
            else ani.SetTrigger("Attack2");
            //注意：协程判断，如果当前async是为空，则直接打开携程，否则先停掉上一个携程再开启新协程
            //在受伤逻辑里说明这样做的目的
            if (async != null)
            {
                StopCoroutine(StateChange());
            }
            async = StartCoroutine(StateChange());
        }
    }
    //暂时还没写好受伤，因为目前敌人还没写好敌人的生命系统，属性系统，所以先暂时放着
    public override void StateDamage()
    {
        agent.speed = 0;
    }
    //同上
    public override void StateDead()
    {
        agent.speed = 0;
    }
    //变回巡逻状态只触发一次，所以settrigger只在change里面出现一次。
    public override void ChangeIdle()
    {
        ani.SetTrigger("Idle");
        base.ChangeIdle();
    }

    public override void ChangeWalk()
    {
        ani.SetTrigger("Walk");
        base.ChangeWalk();
    }
    //注：伤害这里说明一下协程的原因，主要是敌人有可能会一直受到玩家的攻击，但是我希望敌人
    //只记录最后一次攻击，然后如果玩家不攻击了再开始判断是追击还是回去巡逻。
    public override void ChangeDamage()
    {
        ani.SetTrigger("Damage");
        if (async != null)
        {
            StopCoroutine(async);
        }
        async = StartCoroutine(StateChange());
        base.ChangeDamage();
    }

    public override void ChangeDead()
    {
        ani.SetTrigger("Die");
        base.ChangeDead();
    }
    //协程说明：等待CD时间后，敌人再根据与玩家的距离做出判断。
    private IEnumerator StateChange()
    {
        yield return new WaitForSeconds(CD);
        AttackLock = false;//不管是攻击开启协程还是受伤开启，理论上都得锁住攻击
        async = null;//让协程为空，只触发最后一次调用的协程。
        if (distance > 1)//距离大于1就进入攻击
        {
            ChangeWalk();
        }
        else//距离小于1，也就是敌人与玩家贴脸，那么继续下一次攻击。
        {
            ChangeAttack();
        }
    }
}
