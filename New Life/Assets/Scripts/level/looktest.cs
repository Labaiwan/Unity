using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class looktest : MonoBehaviour
{
    private EnemyVision enemyVision; // ���ڴ洢���˵�EnemyVision�ű�������

    void Start()
    {
        // ���ҳ����е�һ��EnemyVision���
        enemyVision = FindObjectOfType<EnemyVision>();

        if (enemyVision == null)
        {
            Debug.LogError("δ���ҵ����˵�EnemyVision�ű���");
        }
    }

    void Update()
    {
        // �������Ƿ񿴵����
        if (enemyVision != null)
        {
            if (enemyVision.PlayerInSight)
            {
                Debug.Log("��ұ����˿�����");
                // ������Դ�����ұ����ֵ��߼�����������ܡ�������Ч��
            }
            else
            {
                Debug.Log("���û�б����˿�����");
                // ���δ�����֣�����ִ��������Ϊ
            }
        }
    }
}
