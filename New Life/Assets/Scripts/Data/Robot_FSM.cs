using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Robot_FSM : FSM
{
    public GameObject[] Target;//������ŵ���Ѳ��ʱ��λ�����꣬һ����������
    private Animator ani;//���Է���FSM���棬���������һ�ûȷ�����������Ķ����ǲ��Ƕ���5�֡�
    private NavMeshAgent agent;//��ͨ��NavMesh����
    private Coroutine async;//Э�̣��ܴ�����
    private bool AttackLock;//����������ֹ�ظ�����
    private int i = 0;
    private float time = 5.0f;
    private float CD = 2f;

    private void Awake()
    {
        ani = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        AttackLock = false;//��ʼʱ����ס��
    }

    public override void StateIdle()
    {
        agent.speed = 2.0f;//AI�����ٶ�Ϊ2.0F��һ��ÿ��״̬�¶���Ҫ��д��ͬ���ٶȡ�
        time -= Time.deltaTime;
        if (time <= 0)
        {
            time = 5.0f;
            i = Random.Range(0, Target.Length);//��ʱ�����ٲ�ͬ��Ѳ�ߵ�֮��������ѡ����Ѳ��
            agent.SetDestination(Target[i].transform.position);//����Ŀ���ƶ�
        }


        //�����ٶ��������Ƿ���·
        ani.SetBool("Walk", agent.velocity != Vector3.zero);
        //0���� 0.5��· 1�ܲ�
        ani.SetFloat("VSpeed", 0.5f);

        if (distance < 5)
        {
            ChangeWalk();
        }


    }

    public override void StateWalk()
    {
        float speedPercent = agent.velocity.magnitude / agent.speed;//����ȡ��ǰ�ٶȵı�׼����ֵ
        agent.speed = 5.0f;//׷��״̬���ٶ�����5.0f
        agent.SetDestination(player.transform.position);//���˿�ʼ������ҵ�λ��
        ani.SetFloat("Speed", speedPercent, 0.1f, Time.deltaTime);//���ٶȴ��붯����������
        if (distance < 2)//�жϾ���С��2����ʼ����
        {
            ChangeAttack();
        }
    }

    public override void StateAttack()
    {
        if (AttackLock == false)//û����������²��ܽ��й�����
        {
            int j = Random.Range(0, 2);//������жϲ����ĸ���������
            AttackLock = true;//������ʼ�˾���ס
            agent.speed = 0f;//������ÿ�����������ٶȣ���Ȼ�ᱣ��ǰһ��״̬�µ��ٶ�
            gameObject.transform.LookAt(player.transform.position);//������ҹ���
            if (j == 0) ani.SetTrigger("Attack1");
            else ani.SetTrigger("Attack2");
            //ע�⣺Э���жϣ������ǰasync��Ϊ�գ���ֱ�Ӵ�Я�̣�������ͣ����һ��Я���ٿ�����Э��
            //�������߼���˵����������Ŀ��
            if (async != null)
            {
                StopCoroutine(StateChange());
            }
            async = StartCoroutine(StateChange());
        }
    }
    //��ʱ��ûд�����ˣ���ΪĿǰ���˻�ûд�õ��˵�����ϵͳ������ϵͳ����������ʱ����
    public override void StateDamage()
    {
        agent.speed = 0;
    }
    //ͬ��
    public override void StateDead()
    {
        agent.speed = 0;
    }
    //���Ѳ��״ֻ̬����һ�Σ�����settriggerֻ��change�������һ�Ρ�
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
    //ע���˺�����˵��һ��Э�̵�ԭ����Ҫ�ǵ����п��ܻ�һֱ�ܵ���ҵĹ�����������ϣ������
    //ֻ��¼���һ�ι�����Ȼ�������Ҳ��������ٿ�ʼ�ж���׷�����ǻ�ȥѲ�ߡ�
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
    //Э��˵�����ȴ�CDʱ��󣬵����ٸ�������ҵľ��������жϡ�
    private IEnumerator StateChange()
    {
        yield return new WaitForSeconds(CD);
        AttackLock = false;//�����ǹ�������Э�̻������˿����������϶�����ס����
        async = null;//��Э��Ϊ�գ�ֻ�������һ�ε��õ�Э�̡�
        if (distance > 1)//�������1�ͽ��빥��
        {
            ChangeWalk();
        }
        else//����С��1��Ҳ���ǵ����������������ô������һ�ι�����
        {
            ChangeAttack();
        }
    }
}
