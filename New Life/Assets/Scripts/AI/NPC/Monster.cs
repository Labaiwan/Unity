using Invector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Monster : MonoBehaviour
{
    private Animator animator;
    private GameObject player;
    private NavMeshAgent agent;
    private Tower tower;
    private List<Transform> towerChildren;
    private Transform target;
    public Direction Dire;
    public string dire;
    // �����Ƿ�����
    public bool isDead = false;
    // ��һ�ι�����ʱ��
    private float frontTime;

    private GameObject targetObject;

    void Start()
    {

        player = GameObject.Find("Player");
        if (tower == null)
        { 
            tower = GameObject.Find(dire).GetComponent<Tower>();
        }
        animator = this.GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        towerChildren = tower.GetChildren(); //��ȡ��������������
        target = towerChildren[Random.Range(0, towerChildren.Count)]; //���ѡ��һ����������ΪĿ��
        agent.SetDestination(target.position);
    }

    void Update()
    {
        if (target == null || target.gameObject == null || tower.isDead)
        {
            agent.isStopped = true;
            animator.SetBool("run",false);
            return;
        }
        //����Ŀ���ľ���
        float distanceToPlayer = Vector3.Distance(this.transform.position, player.transform.position);

        targetObject = null;
        if (distanceToPlayer < 8)
        {
            targetObject = player;
        }
        else
        {
            targetObject = target.gameObject;
            
        }

        agent.SetDestination(targetObject.transform.position);

        //���ʲôʱ��ͣ��������
        if (isDead)
            return;

        // �����ٶ���������������ʲô
        animator.SetBool("run", agent.velocity != Vector3.zero);
        //����Ŀ���ﵽ�ƶ�����ʱ�͹���
        if (Vector3.Distance(this.transform.position, targetObject.transform.position) < 5 && Time.time - frontTime >= 1.5 && !tower.isDead)
        {
            //��¼��ι���ʱ��ʱ��
            frontTime = Time.time;
            //���Ź�������
            animator.SetTrigger("attack");
        }   
    }

    public void Wound()
    {
        //�������˶���
        animator.SetTrigger("hurt");
    }

    public void Dead()
    {
        isDead = true;
        animator.SetBool("Dead", true);
        agent.isStopped = true;
        Destroy(this.gameObject, 3);
        //���б����Ƴ�����
        Chapter2Mgr.Instance.RemoveEnemy(this);
        //��Ϸʤ��
        if (Chapter2Mgr.Instance.checkWin())
        {
            UIDataMgr.Instance.GetPanel<GamePanel>().ShowTipText("��ϲ�� �ɹ���ס");
        }
    }

    public void stopAgent()
    {
        agent.isStopped = true;
    }
}