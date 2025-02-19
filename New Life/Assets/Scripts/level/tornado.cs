using Invector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tornado : MonoBehaviour
{
    public Transform tornadoCenter;    // ���������
    public float tornadoRadius = 500f; // �����İ뾶
    public float damageRate = 1f;      // ÿ���˺�
    public float pullForce = 10f;      // ������

    private GameObject player;         // ��Ҷ���
    private Rigidbody playerRigidbody; // ��Ҹ���
    private bool isPlayerInRange = false;

    void Start()
    {
        // ��ȡ��Ҷ������ҵ� Rigidbody ���
        player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerRigidbody = player.GetComponent<Rigidbody>();
        }
    }

    void Update()
    {
        // �������Ƿ�������緶Χ��
        CheckPlayerInRange();

        if (isPlayerInRange && player != null)
        {
            // ʩ���˺�
            ApplyDamageToPlayer();
            // �������
            PullPlayerTowardsTornado();
        }
    }

    // �������Ƿ��������ķ�Χ��
    private void CheckPlayerInRange()
    {
        if (player != null && Vector3.Distance(player.transform.position, tornadoCenter.position) <= tornadoRadius)
        {
            isPlayerInRange = true;
        }
        else
        {
            isPlayerInRange = false;
        }
    }

    // �����ʩ���˺�
    private void ApplyDamageToPlayer()
    {
        // ���������һ�� PlayerHealth �ű���������
        vHealthController playerHealth = player.GetComponent<vHealthController>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(new vDamage((int)Time.deltaTime)); // ÿ���˺�
        }
    }

    // ���������������������
    private void PullPlayerTowardsTornado()
    {
        if (playerRigidbody != null)
        {
            Vector3 direction = (tornadoCenter.position - player.transform.position).normalized;
            playerRigidbody.AddForce(direction * pullForce * Time.deltaTime, ForceMode.Acceleration); // ������ʩ����
        }
    }
}
