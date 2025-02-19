using UnityEngine;

public class EnemyVision : MonoBehaviour
{
    //视野半径
    public float visionRadius = 10f;
    //视野角度
    public float visionAngle = 120f;
    //高度限制
    public float heightLimit = 2f;
    //玩家层
    public LayerMask playerLayer;
    //障碍物层
    public LayerMask obstacleLayer;

    //玩家是否在视野内
    private bool playerInSight = false;

    //用于外部访问playerInSight
    public bool PlayerInSight { get { return playerInSight; } }

    //外部调用 用于检查是否有玩家在视野内
    public bool CheckPlayerInSight()
    {
        //初始化玩家不在视野内
        playerInSight = false;

        //检测视野范围内的玩家
        Collider[] playersInRadius = Physics.OverlapSphere(this.transform.position, visionRadius, playerLayer);

        foreach (Collider playerCollider in playersInRadius)
        {
            //计算玩家与敌人之间的方向
            Vector3 directionToPlayer = (playerCollider.transform.position - this.transform.position).normalized;
            //计算角度
            float angleToPlayer = Vector3.Angle(this.transform.forward, directionToPlayer);
            //计算高度差
            float heightDifference = Mathf.Abs(playerCollider.transform.position.y - this.transform.position.y);

            //如果玩家在视野角度内且高度差在限制范围内
            if (angleToPlayer <= visionAngle / 2 && heightDifference <= heightLimit)
            {
                //进行射线检测，增加射线密度
                bool isObstructed = false;
                float raycastDistance = Vector3.Distance(this.transform.position, playerCollider.transform.position);
                int raycastCount = 5;
                for (int i = 0; i < raycastCount; i++)
                {
                    //微小偏移射线起点 以增加检测密度
                    Vector3 randomOffset = Random.insideUnitSphere * 0.1f;
                    Vector3 raycastOrigin = this.transform.position + randomOffset;
                    RaycastHit hit;
                    if (Physics.Raycast(raycastOrigin, directionToPlayer, out hit, raycastDistance, obstacleLayer))
                    {
                        isObstructed = true;
                        break;
                    }
                }
                //如果没有障碍物阻挡
                if (!isObstructed)
                {
                    playerInSight = true;
                    break; 
                }
            }
        }
        return playerInSight;
    }

    void Update()
    {
        //调用CheckPlayerInSight方法检查是否有玩家在视野内
        playerInSight = CheckPlayerInSight();
    }

    void OnDrawGizmos()
    {
        //绘制视野范围
        Gizmos.color = Color.red;
        //绘制视野圆锥
        DrawVisionCone(this.transform.position, visionRadius, visionAngle);
        //绘制高度限制的线
        Gizmos.DrawLine(this.transform.position, this.transform.position + Vector3.up * heightLimit);
        //绘制高度限制的视野圆锥
        DrawVisionCone(this.transform.position + Vector3.up * heightLimit, visionRadius, visionAngle);
    }

    void DrawVisionCone(Vector3 center, float radius, float angle)
    {
        Vector3 forward = this.transform.forward;
        Vector3 right = this.transform.right;
        Vector3 up = this.transform.up;

        //计算视野角度的半角
        float halfAngle = angle / 2.0f;

        //绘制视野圆锥的边
        Gizmos.DrawLine(center, center + Quaternion.Euler(0, -halfAngle, 0) * forward * radius);
        Gizmos.DrawLine(center, center + Quaternion.Euler(0, halfAngle, 0) * forward * radius);

        //绘制视野圆锥的弧
        int segments = 24;
        Vector3 lastPoint = center + Quaternion.Euler(0, -halfAngle, 0) * forward * radius;
        for (int i = 1; i <= segments; i++)
        {
            float angleOffset = -halfAngle + (i / (float)segments) * angle;
            Vector3 point = center + Quaternion.Euler(0, angleOffset, 0) * forward * radius;
            Gizmos.DrawLine(lastPoint, point);
            lastPoint = point;
        }
    }
}
