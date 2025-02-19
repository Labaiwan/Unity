using Invector;
using UnityEngine;
using UnityEngine.AI;

public class TornadoMovement : MonoBehaviour
{
    [HideInInspector]public NavMeshAgent agent;
    //地形中心点
    public Transform center;
    //地形半径
    public float radius = 500f;
    //寻路点
    private Vector3 randomPoint;

    //寻路随机间隔
    private float timeInterval;
    //追逐玩家时间
    public float playerChaseTime = 4f;
    //随机点时间
    public float randomPointTime = 50f;
    private float timer;

    //玩家对象
    public GameObject player;
    private bool isChasingPlayer;
    private float chaseTimeRemaining = 0f;

    void Start()
    {
        agent = this.GetComponent<NavMeshAgent>();
        player = GameObject.Find("Player");
        //初始化第一个寻路点
        SetRandomDestination();
    }

    void Update()
    {
        timer -= Time.deltaTime;

        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance || timer <= 0)
        {
            SetNextDestination();
        }

        //如果正在追逐玩家 减少追逐时间
        if (isChasingPlayer && chaseTimeRemaining > 0f)
        {
            chaseTimeRemaining -= Time.deltaTime;
            Debug.Log("正在追击玩家，还剩"+ chaseTimeRemaining+"秒，原本是"+ playerChaseTime+"秒的");
        }

        //如果追逐时间结束 开始随机寻路
        if (chaseTimeRemaining <= 0 && isChasingPlayer)
        {
            isChasingPlayer = false;
            SetRandomDestination();
            timeInterval = randomPointTime;
            timer = timeInterval;
        }
    }

    //设置下一个目标点
    private void SetNextDestination()
    {
        if (isChasingPlayer)
        {
            return;
        }
        else
        {
            //概率追逐玩家
            if (Random.Range(0f, 1f) <= 0.1f && player != null)
            {
                isChasingPlayer = true;
                chaseTimeRemaining = playerChaseTime;
                agent.SetDestination(player.transform.position);
                timeInterval = playerChaseTime;
            }
            else
            {
                SetRandomDestination();
                timeInterval = randomPointTime;
            }
        }

        //重置计时器
        timer = timeInterval;
    }

    //设置随机寻路目标
    private void SetRandomDestination()
    {
        //循环直到找到一个在地形范围内的随机点
        do
        {
            randomPoint = GetRandomPointWithinRadius();
        }
        while (!IsRange(randomPoint));

        //设置寻路目标
        agent.SetDestination(randomPoint);
        Debug.Log("随机生成的点：" + randomPoint);
    }

    //设置随机生成点
    private Vector3 GetRandomPointWithinRadius()
    {
        //以地形中心点为基础生成一个随机点
        Vector2 randomCircle = Random.insideUnitCircle * radius;
        return new Vector3(center.position.x + randomCircle.x, center.position.y, center.position.z + randomCircle.y);
    }

    //判断是否在地形中心点的范围内
    private bool IsRange(Vector3 point)
    {
        // 检查点是否在地形范围内
        float distance = Vector3.Distance(point, center.position);
        return distance <= radius;
    }

    private void OnTriggerStay(Collider other)
    {

        if (other.gameObject.CompareTag("Player") && !Chapter3.Instance.isSafe)
        {
            vHealthController damage = player.GetComponent<vHealthController>();
            damage.TakeDamage(new vDamage((int)(Time.deltaTime * 100)));
        }
    }


}
