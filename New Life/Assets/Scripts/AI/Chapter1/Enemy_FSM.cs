using Invector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy1_FSM : FSM
{
    //用来存放敌人巡逻时的位置坐标
    public GameObject[] Target;
    private Animator ani;
    private NavMeshAgent agent;
    private Coroutine async;
    //防止重复攻击
    private bool AttackLock;
    private int i = 0;
    private float time = 2.0f;
    private float CD = 2f;
    private vHealthController Health;
    private float hp;
    //记录最后一次被攻击的时间
    private float lastAttackTime;
    //是否追击后脱离
    private bool isCreate = false;
    private GameObject[] newTarget;

    private bool hasGenerated = false;
    //是否增加仇恨值
    private bool isAddthreat;
    private GameObject randomPos;
    private int childCount;
    //随机的点的数组
    private GameObject[] randomGameObjects;
    private void Awake()
    {
        ani = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        Health = GetComponent<vHealthController>();
        AttackLock = false;
        hp = Health.maxHealth;
        //初始化lastAttackTime为当前时间
        lastAttackTime = Time.time;
        //获取场景的AgentPosition里的所有子物体 就是敌人寻路的点
        randomPos = GameObject.Find("AgentPosition");
        //获取AgentPosition子物体的数量
        childCount = randomPos.transform.childCount;
        //如果存放点为空 那么就随机从AgentPosition里的点随机几个去巡逻
        if (Target.Length == 0)
        {
            randomGameObjects = new GameObject[childCount];
            // 遍历并添加所有子物体到数组
            for (int i = 0; i < randomGameObjects.Length; i++)
            {
                // 获取子物体
                Transform childTransform = randomPos.transform.GetChild(i);
                // 将子物体的GameObject添加到数组
                randomGameObjects[i] = childTransform.gameObject;
            }
            //随机生成1-10的巡逻的点
            Target = new GameObject[UnityEngine.Random.Range(1, 10)];

            //根据随机生成的点来遍历
            for (int i = 0; i < Target.Length; i++)
            {
                //生成一个随机索引
                int randomIndex = UnityEngine.Random.Range(0, randomGameObjects.Length);
                //将从AgentPosition中的点赋值给巡逻点
                Target[i] = randomGameObjects[randomIndex];

                // 使用一个HashSet来跟踪已选择的索引 避免重复
                HashSet<int> chosenIndices = new HashSet<int>();
                bool unique = false;
                while (!unique)
                {
                    randomIndex = UnityEngine.Random.Range(0, randomGameObjects.Length);
                    if (!chosenIndices.Contains(randomIndex))
                    {
                        chosenIndices.Add(randomIndex);
                        unique = true;
                    }
                }
            }          
        }
    }

    public override void StateIdle()
    {
        // 设置巡逻速度
        agent.speed = 2.0f;
        time -= Time.deltaTime;
        if (time <= 0)
        {
            time = 2.0f;
            if (!isCreate)
            {
                //再不同的巡逻点之间进行随机选择性巡逻
               i = UnityEngine.Random.Range(0, Target.Length);               
               agent.SetDestination(Target[i].transform.position);
            }
            //如果在追击后失去目标 就以敌人自身的10米范围内随机生成三个物体
            else if(isCreate)
            {
                if (!hasGenerated)
                {
                    newTarget = new GameObject[UnityEngine.Random.Range(1, 3)];
                    for (int i = 0; i < newTarget.Length; i++)
                    {
                        newTarget[i] = new GameObject("RandomObject" + i);
                        Vector3 randomPosition = new Vector3(transform.position.x + UnityEngine.Random.Range(-10f, 10f), transform.position.y, transform.position.z + UnityEngine.Random.Range(-10f, 10f));
                        newTarget[i].transform.position = randomPosition;
                    }
                    hasGenerated = true;
                }                
                i = UnityEngine.Random.Range(0, newTarget.Length);
                agent.SetDestination(newTarget[i].transform.position);
            }

        }
        //根据速度来决定是否走路
        ani.SetBool("Walk", agent.velocity != Vector3.zero);
        //0咆哮 0.5走路 1跑步
        ani.SetFloat("VSpeed", 0.5f);

        //如果玩家距离敌人12米内 敌人追击玩家
        if (distance < 12)
        {
            ChangeWalk();
            //清除原有巡逻的点
            ClearTarget();
        }

        if (Health.isDead)
        {
            ChangeDead();
        }
        else
        {
            if (Health.currentHealth != hp)
            {
                if (Health.currentHealth < hp)
                {
                    ChangeDamage();
                }
            }
        }

        hp = Health.currentHealth;
    }

    public override void StateWalk()
    {
        if (!Health.isDead)
        {
            // 敌人开始朝着玩家的位置
            agent.SetDestination(player.transform.position);
            // 追击状态下速度升到5
            agent.speed = 5; 
            float speedPercent = agent.velocity.magnitude / agent.speed;
            ani.SetFloat("VSpeed", speedPercent, 0.1f, Time.deltaTime); // 把速度传入动画控制器
            //如果玩家距离敌人2米 就开始攻击
            if (distance < 2)
            {
                ChangeAttack();
            }
            //如果超过12秒没有被攻击并且距离大于追击范围并且仇恨值不为满 则返回巡逻状态
            else if (Time.time - lastAttackTime > 15 && distance > 20 && ThreatManager.instance.threatLevel < 5)
            {
                ClearTarget();
                ChangeIdle();
            }
        }
        else
        {
            ChangeDead();
        }
    }

    public override void StateAttack()
    {
        if (AttackLock == false) 
        {
            ani.SetBool("Walk", false);
            //随机数判断采用哪个攻击动画
            int j = UnityEngine.Random.Range(0, 2);
            //攻击了就设置true
            AttackLock = true;
            //在每个动画设置速度 不然会保持前一个状态下的速度
            agent.speed = 0f;
            //朝向玩家攻击
            gameObject.transform.LookAt(player.transform.position);
            if (j == 0) 
                ani.SetTrigger("Attack1");
            else 
                ani.SetTrigger("Attack2");
            //如果当前async是为空 则直接打开携程 否则先停掉上一个携程再开启新协程
            if (async != null)
            {
                StopCoroutine(StateChange());
            }
            async = StartCoroutine(StateChange());
        }
    }

    public override void StateDamage()
    {
        ani.SetBool("Walk", false);
        agent.speed = 0;
        //如果敌人死亡 改变状态到死亡状态
        if (Health.isDead)
        {
            ChangeDead();
        }
    }

    public override void StateDead()
    {
        ani.SetBool("Walk", false);
        //停止移动
        agent.speed = 0;
        //禁用NavMeshAgent
        agent.enabled = false; 
        if (!isAddthreat)
        {
            ThreatManager.instance.AddThreat();
            isAddthreat = true;
        }

    }

    public override void ChangeIdle()
    {
        ani.SetTrigger("Idle");
        base.ChangeIdle();
    }

    public override void ChangeWalk()
    {
        ani.SetBool("Walk", true);
        base.ChangeWalk();
    }

    public override void ChangeDamage()
    {
        ani.SetTrigger("Damage");

        if (async != null)
        {
            StopCoroutine(async);
        }
        async = StartCoroutine(StateChange());
        base.ChangeDamage();

        // 重置最后一次被攻击的时间
        lastAttackTime = Time.time;
    }

    public override void ChangeDead()
    {

        ani.SetTrigger("Dead");
        Destroy(gameObject, 3);
        base.ChangeDead();
    }

    private IEnumerator StateChange()
    {
        yield return new WaitForSeconds(CD);
        AttackLock = false;
        //让协程为空 只触发最后一次调用的协程 防止重复调用
        async = null;
        //距离大于1就继续追击
        if (distance > 1) 
        {
            ChangeWalk();
        }
        //距离小于1 那么继续下一次攻击
        else
        {
            ChangeAttack();
        }
    }

    //清空在敌人追击后失去目标后以自身附近随机生成的点
    private void ClearTarget()
    {
        isCreate = true;
        hasGenerated = false;

        if (newTarget != null)
        {
            foreach (var obj in newTarget)
            {
                if (obj != null)
                {
                    Destroy(obj);
                }
            }
            // 清空数组
            newTarget = new GameObject[0];
        }
    }   
}