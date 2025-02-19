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
    // 怪物是否死亡
    public bool isDead = false;
    // 上一次攻击的时间
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
        towerChildren = tower.GetChildren(); //获取塔的所有子物体
        target = towerChildren[Random.Range(0, towerChildren.Count)]; //随机选择一个子物体作为目标
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
        //检测和目标点的距离
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

        //检测什么时候停下来攻击
        if (isDead)
            return;

        // 根据速度来决定动画播放什么
        animator.SetBool("run", agent.velocity != Vector3.zero);
        //检测和目标点达到移动条件时就攻击
        if (Vector3.Distance(this.transform.position, targetObject.transform.position) < 5 && Time.time - frontTime >= 1.5 && !tower.isDead)
        {
            //记录这次攻击时的时间
            frontTime = Time.time;
            //播放攻击动画
            animator.SetTrigger("attack");
        }   
    }

    public void Wound()
    {
        //播放受伤动画
        animator.SetTrigger("hurt");
    }

    public void Dead()
    {
        isDead = true;
        animator.SetBool("Dead", true);
        agent.isStopped = true;
        Destroy(this.gameObject, 3);
        //从列表中移除怪物
        Chapter2Mgr.Instance.RemoveEnemy(this);
        //游戏胜利
        if (Chapter2Mgr.Instance.checkWin())
        {
            UIDataMgr.Instance.GetPanel<GamePanel>().ShowTipText("恭喜你 成功守住");
        }
    }

    public void stopAgent()
    {
        agent.isStopped = true;
    }
}