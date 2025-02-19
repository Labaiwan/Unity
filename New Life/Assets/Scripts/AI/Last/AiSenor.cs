using UnityEngine;

public class AiSenor : MonoBehaviour
{
    public float viewRadius = 5f; // �Ӿ�
    public float viewAngle = 120f; // �ӽǷ�Χ���Զ�Ϊ��λ��
    public LayerMask playerLayer;  // ������ڵĲ�
    public LayerMask obstacleLayer; // �ϰ������ڵĲ�

    private void Update()
    {
        if (IsPlayerInSight())
        {
            Debug.Log("�������Ұ��");
        }
        else
        {
            Debug.Log("��Ҳ�����Ұ��");
        }
    }

    private bool IsPlayerInSight()
    {
        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, playerLayer);
        foreach (var target in targetsInViewRadius)
        {
            Vector3 directionToPlayer = (target.transform.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, directionToPlayer) < viewAngle / 2)
            {
                if (!Physics.Raycast(transform.position, directionToPlayer, Vector3.Distance(transform.position, target.transform.position), obstacleLayer))
                {
                    return true;
                }
            }
        }
        return false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, viewRadius);

        Vector3 leftBoundary = Quaternion.Euler(0, -viewAngle / 2, 0) * transform.forward;
        Vector3 rightBoundary = Quaternion.Euler(0, viewAngle / 2, 0) * transform.forward;

        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + leftBoundary * viewRadius);
        Gizmos.DrawLine(transform.position, transform.position + rightBoundary * viewRadius);
    }
}