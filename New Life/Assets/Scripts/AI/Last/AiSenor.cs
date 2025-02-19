using UnityEngine;

public class AiSenor : MonoBehaviour
{
    public float viewRadius = 5f; // 视距
    public float viewAngle = 120f; // 视角范围（以度为单位）
    public LayerMask playerLayer;  // 玩家所在的层
    public LayerMask obstacleLayer; // 障碍物所在的层

    private void Update()
    {
        if (IsPlayerInSight())
        {
            Debug.Log("玩家在视野内");
        }
        else
        {
            Debug.Log("玩家不在视野内");
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