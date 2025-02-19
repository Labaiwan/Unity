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
        //根据速度来决定动画播放什么
        ani.SetBool("walk", agent.velocity != Vector3.zero);
        //判断是否到达了目的地
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
            //获取NpcControl组件
            dialogue = this.GetComponent<NpcControl>();
            //实现指定对话内容
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
        // 玩家当前的方向
        Vector3 direction = player.forward;
        // 距离玩家坐标
        Vector3 offsetPosition = player.position + direction.normalized * 5f;
        // 赋值给firstPos
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
