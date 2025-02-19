using MoonSharp.VsCodeDebugger.SDK;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CompanionNpc : MonoBehaviour
{
    private Animator ani;
    private NavMeshAgent agent;
    public Transform firstPos;
    public Transform secondPos;
    public Transform thirdPos;
    public NpcControl dialogue;
    public Animator door;
    private Transform player;
    private bool isNext = false;
    private bool isover = false;
    public GameObject over;
    void Start()
    {
        ani = this.GetComponent<Animator>();
        agent = this.GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        SetFirstPos();
        //�����ٶ���������������ʲô
        ani.SetBool("walk", agent.velocity != Vector3.zero);
        //�ж��Ƿ񵽴���Ŀ�ĵ�
        if ((agent.transform.position - firstPos.position).magnitude < 1 && isNext)
        {
            agent.speed = 0;            
        }

        if ((agent.transform.position - secondPos.position).magnitude < 1 && !isNext)
        {
            door.SetBool("Open", false);
            agent.speed = 0;           
            ani.SetBool("sit",true);
            transform.rotation = secondPos.rotation;
        }

        if ((agent.transform.position - thirdPos.position).magnitude < 1 && isNext)
        {
            agent.speed = 0;
        }

        if (Chapter2Mgr.Instance.checkWin() && isover)
        {
            //��ȡNpcControl���
            dialogue = this.GetComponent<NpcControl>();
            //ʵ��ָ���Ի�����
            //dialogue.ChatName = "over";
            agent.isStopped = false;
            agent.speed = 1;
            agent.SetDestination(player.position);
            if ((agent.transform.position - player.position).magnitude < 3)
            {
                over.SetActive(true);
                agent.speed = 0;
                isover = false;
            }
        }
    }

    public void SetFirstPos()
    {
        // ��ҵ�ǰ�ķ���
        Vector3 direction = player.forward;
        // �����������
        Vector3 offsetPosition = player.position + direction.normalized * 5f;
        // ��ֵ��firstPos
        firstPos.position = offsetPosition;
        firstPos.rotation = player.rotation;
        
    }

    public void FirstPos()
    {
        isNext = true;
        door.SetBool("Open", true);
        agent.SetDestination(firstPos.position);

    }

    public void SecondPos()
    {
        isNext = false;
        agent.speed = 1;
        agent.isStopped = false;
        agent.SetDestination(secondPos.position);
    }

    public void ThirdPos()
    {
        isNext = true;
        agent.isStopped = false;
        ani.SetBool("sit", false);
        agent.speed = 1;
        agent.SetDestination(thirdPos.position);
        isover = true;
    }

    public void BeginCome()
    {
        if (!Chapter2Mgr.Instance.isBegin)
        {
            GameDataMgr.Instance.PlaySound("monster", 3);
        }
        Chapter2Mgr.Instance.isBegin = true;
    }

    public void giveMedicine()
    {
        GameDataMgr.Instance.AddItemToBag(5,5);
    }

}
