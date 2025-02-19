using Invector;
using UnityEngine;
using UnityEngine.AI;

public class TornadoMovement : MonoBehaviour
{
    [HideInInspector]public NavMeshAgent agent;
    //�������ĵ�
    public Transform center;
    //���ΰ뾶
    public float radius = 500f;
    //Ѱ·��
    private Vector3 randomPoint;

    //Ѱ·������
    private float timeInterval;
    //׷�����ʱ��
    public float playerChaseTime = 4f;
    //�����ʱ��
    public float randomPointTime = 50f;
    private float timer;

    //��Ҷ���
    public GameObject player;
    private bool isChasingPlayer;
    private float chaseTimeRemaining = 0f;

    void Start()
    {
        agent = this.GetComponent<NavMeshAgent>();
        player = GameObject.Find("Player");
        //��ʼ����һ��Ѱ·��
        SetRandomDestination();
    }

    void Update()
    {
        timer -= Time.deltaTime;

        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance || timer <= 0)
        {
            SetNextDestination();
        }

        //�������׷����� ����׷��ʱ��
        if (isChasingPlayer && chaseTimeRemaining > 0f)
        {
            chaseTimeRemaining -= Time.deltaTime;
            Debug.Log("����׷����ң���ʣ"+ chaseTimeRemaining+"�룬ԭ����"+ playerChaseTime+"���");
        }

        //���׷��ʱ����� ��ʼ���Ѱ·
        if (chaseTimeRemaining <= 0 && isChasingPlayer)
        {
            isChasingPlayer = false;
            SetRandomDestination();
            timeInterval = randomPointTime;
            timer = timeInterval;
        }
    }

    //������һ��Ŀ���
    private void SetNextDestination()
    {
        if (isChasingPlayer)
        {
            return;
        }
        else
        {
            //����׷�����
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

        //���ü�ʱ��
        timer = timeInterval;
    }

    //�������Ѱ·Ŀ��
    private void SetRandomDestination()
    {
        //ѭ��ֱ���ҵ�һ���ڵ��η�Χ�ڵ������
        do
        {
            randomPoint = GetRandomPointWithinRadius();
        }
        while (!IsRange(randomPoint));

        //����Ѱ·Ŀ��
        agent.SetDestination(randomPoint);
        Debug.Log("������ɵĵ㣺" + randomPoint);
    }

    //����������ɵ�
    private Vector3 GetRandomPointWithinRadius()
    {
        //�Ե������ĵ�Ϊ��������һ�������
        Vector2 randomCircle = Random.insideUnitCircle * radius;
        return new Vector3(center.position.x + randomCircle.x, center.position.y, center.position.z + randomCircle.y);
    }

    //�ж��Ƿ��ڵ������ĵ�ķ�Χ��
    private bool IsRange(Vector3 point)
    {
        // �����Ƿ��ڵ��η�Χ��
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
