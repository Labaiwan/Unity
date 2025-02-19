using UnityEngine;

public class EnemyVision : MonoBehaviour
{
    //��Ұ�뾶
    public float visionRadius = 10f;
    //��Ұ�Ƕ�
    public float visionAngle = 120f;
    //�߶�����
    public float heightLimit = 2f;
    //��Ҳ�
    public LayerMask playerLayer;
    //�ϰ����
    public LayerMask obstacleLayer;

    //����Ƿ�����Ұ��
    private bool playerInSight = false;

    //�����ⲿ����playerInSight
    public bool PlayerInSight { get { return playerInSight; } }

    //�ⲿ���� ���ڼ���Ƿ����������Ұ��
    public bool CheckPlayerInSight()
    {
        //��ʼ����Ҳ�����Ұ��
        playerInSight = false;

        //�����Ұ��Χ�ڵ����
        Collider[] playersInRadius = Physics.OverlapSphere(this.transform.position, visionRadius, playerLayer);

        foreach (Collider playerCollider in playersInRadius)
        {
            //������������֮��ķ���
            Vector3 directionToPlayer = (playerCollider.transform.position - this.transform.position).normalized;
            //����Ƕ�
            float angleToPlayer = Vector3.Angle(this.transform.forward, directionToPlayer);
            //����߶Ȳ�
            float heightDifference = Mathf.Abs(playerCollider.transform.position.y - this.transform.position.y);

            //����������Ұ�Ƕ����Ҹ߶Ȳ������Ʒ�Χ��
            if (angleToPlayer <= visionAngle / 2 && heightDifference <= heightLimit)
            {
                //�������߼�⣬���������ܶ�
                bool isObstructed = false;
                float raycastDistance = Vector3.Distance(this.transform.position, playerCollider.transform.position);
                int raycastCount = 5;
                for (int i = 0; i < raycastCount; i++)
                {
                    //΢Сƫ��������� �����Ӽ���ܶ�
                    Vector3 randomOffset = Random.insideUnitSphere * 0.1f;
                    Vector3 raycastOrigin = this.transform.position + randomOffset;
                    RaycastHit hit;
                    if (Physics.Raycast(raycastOrigin, directionToPlayer, out hit, raycastDistance, obstacleLayer))
                    {
                        isObstructed = true;
                        break;
                    }
                }
                //���û���ϰ����赲
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
        //����CheckPlayerInSight��������Ƿ����������Ұ��
        playerInSight = CheckPlayerInSight();
    }

    void OnDrawGizmos()
    {
        //������Ұ��Χ
        Gizmos.color = Color.red;
        //������ҰԲ׶
        DrawVisionCone(this.transform.position, visionRadius, visionAngle);
        //���Ƹ߶����Ƶ���
        Gizmos.DrawLine(this.transform.position, this.transform.position + Vector3.up * heightLimit);
        //���Ƹ߶����Ƶ���ҰԲ׶
        DrawVisionCone(this.transform.position + Vector3.up * heightLimit, visionRadius, visionAngle);
    }

    void DrawVisionCone(Vector3 center, float radius, float angle)
    {
        Vector3 forward = this.transform.forward;
        Vector3 right = this.transform.right;
        Vector3 up = this.transform.up;

        //������Ұ�Ƕȵİ��
        float halfAngle = angle / 2.0f;

        //������ҰԲ׶�ı�
        Gizmos.DrawLine(center, center + Quaternion.Euler(0, -halfAngle, 0) * forward * radius);
        Gizmos.DrawLine(center, center + Quaternion.Euler(0, halfAngle, 0) * forward * radius);

        //������ҰԲ׶�Ļ�
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
