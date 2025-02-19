using Invector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy1_FSM : FSM
{
    //������ŵ���Ѳ��ʱ��λ������
    public GameObject[] Target;
    private Animator ani;
    private NavMeshAgent agent;
    private Coroutine async;
    //��ֹ�ظ�����
    private bool AttackLock;
    private int i = 0;
    private float time = 2.0f;
    private float CD = 2f;
    private vHealthController Health;
    private float hp;
    //��¼���һ�α�������ʱ��
    private float lastAttackTime;
    //�Ƿ�׷��������
    private bool isCreate = false;
    private GameObject[] newTarget;

    private bool hasGenerated = false;
    //�Ƿ����ӳ��ֵ
    private bool isAddthreat;
    private GameObject randomPos;
    private int childCount;
    //����ĵ������
    private GameObject[] randomGameObjects;
    private void Awake()
    {
        ani = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        Health = GetComponent<vHealthController>();
        AttackLock = false;
        hp = Health.maxHealth;
        //��ʼ��lastAttackTimeΪ��ǰʱ��
        lastAttackTime = Time.time;
        //��ȡ������AgentPosition������������� ���ǵ���Ѱ·�ĵ�
        randomPos = GameObject.Find("AgentPosition");
        //��ȡAgentPosition�����������
        childCount = randomPos.transform.childCount;
        //�����ŵ�Ϊ�� ��ô�������AgentPosition��ĵ��������ȥѲ��
        if (Target.Length == 0)
        {
            randomGameObjects = new GameObject[childCount];
            // ������������������嵽����
            for (int i = 0; i < randomGameObjects.Length; i++)
            {
                // ��ȡ������
                Transform childTransform = randomPos.transform.GetChild(i);
                // ���������GameObject��ӵ�����
                randomGameObjects[i] = childTransform.gameObject;
            }
            //�������1-10��Ѳ�ߵĵ�
            Target = new GameObject[UnityEngine.Random.Range(1, 10)];

            //����������ɵĵ�������
            for (int i = 0; i < Target.Length; i++)
            {
                //����һ���������
                int randomIndex = UnityEngine.Random.Range(0, randomGameObjects.Length);
                //����AgentPosition�еĵ㸳ֵ��Ѳ�ߵ�
                Target[i] = randomGameObjects[randomIndex];

                // ʹ��һ��HashSet��������ѡ������� �����ظ�
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
        // ����Ѳ���ٶ�
        agent.speed = 2.0f;
        time -= Time.deltaTime;
        if (time <= 0)
        {
            time = 2.0f;
            if (!isCreate)
            {
                //�ٲ�ͬ��Ѳ�ߵ�֮��������ѡ����Ѳ��
               i = UnityEngine.Random.Range(0, Target.Length);               
               agent.SetDestination(Target[i].transform.position);
            }
            //�����׷����ʧȥĿ�� ���Ե��������10�׷�Χ�����������������
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
        //�����ٶ��������Ƿ���·
        ani.SetBool("Walk", agent.velocity != Vector3.zero);
        //0���� 0.5��· 1�ܲ�
        ani.SetFloat("VSpeed", 0.5f);

        //�����Ҿ������12���� ����׷�����
        if (distance < 12)
        {
            ChangeWalk();
            //���ԭ��Ѳ�ߵĵ�
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
            // ���˿�ʼ������ҵ�λ��
            agent.SetDestination(player.transform.position);
            // ׷��״̬���ٶ�����5
            agent.speed = 5; 
            float speedPercent = agent.velocity.magnitude / agent.speed;
            ani.SetFloat("VSpeed", speedPercent, 0.1f, Time.deltaTime); // ���ٶȴ��붯��������
            //�����Ҿ������2�� �Ϳ�ʼ����
            if (distance < 2)
            {
                ChangeAttack();
            }
            //�������12��û�б��������Ҿ������׷����Χ���ҳ��ֵ��Ϊ�� �򷵻�Ѳ��״̬
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
            //������жϲ����ĸ���������
            int j = UnityEngine.Random.Range(0, 2);
            //�����˾�����true
            AttackLock = true;
            //��ÿ�����������ٶ� ��Ȼ�ᱣ��ǰһ��״̬�µ��ٶ�
            agent.speed = 0f;
            //������ҹ���
            gameObject.transform.LookAt(player.transform.position);
            if (j == 0) 
                ani.SetTrigger("Attack1");
            else 
                ani.SetTrigger("Attack2");
            //�����ǰasync��Ϊ�� ��ֱ�Ӵ�Я�� ������ͣ����һ��Я���ٿ�����Э��
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
        //����������� �ı�״̬������״̬
        if (Health.isDead)
        {
            ChangeDead();
        }
    }

    public override void StateDead()
    {
        ani.SetBool("Walk", false);
        //ֹͣ�ƶ�
        agent.speed = 0;
        //����NavMeshAgent
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

        // �������һ�α�������ʱ��
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
        //��Э��Ϊ�� ֻ�������һ�ε��õ�Э�� ��ֹ�ظ�����
        async = null;
        //�������1�ͼ���׷��
        if (distance > 1) 
        {
            ChangeWalk();
        }
        //����С��1 ��ô������һ�ι���
        else
        {
            ChangeAttack();
        }
    }

    //����ڵ���׷����ʧȥĿ�����������������ɵĵ�
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
            // �������
            newTarget = new GameObject[0];
        }
    }   
}