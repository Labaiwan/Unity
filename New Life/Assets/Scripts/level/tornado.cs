using Invector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tornado : MonoBehaviour
{
    public Transform tornadoCenter;    // 龙卷风中心
    public float tornadoRadius = 500f; // 龙卷风的半径
    public float damageRate = 1f;      // 每秒伤害
    public float pullForce = 10f;      // 吸引力

    private GameObject player;         // 玩家对象
    private Rigidbody playerRigidbody; // 玩家刚体
    private bool isPlayerInRange = false;

    void Start()
    {
        // 获取玩家对象和玩家的 Rigidbody 组件
        player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerRigidbody = player.GetComponent<Rigidbody>();
        }
    }

    void Update()
    {
        // 检查玩家是否在龙卷风范围内
        CheckPlayerInRange();

        if (isPlayerInRange && player != null)
        {
            // 施加伤害
            ApplyDamageToPlayer();
            // 吸引玩家
            PullPlayerTowardsTornado();
        }
    }

    // 检查玩家是否在龙卷风的范围内
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

    // 对玩家施加伤害
    private void ApplyDamageToPlayer()
    {
        // 假设玩家有一个 PlayerHealth 脚本来管理健康
        vHealthController playerHealth = player.GetComponent<vHealthController>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(new vDamage((int)Time.deltaTime)); // 每秒伤害
        }
    }

    // 将玩家吸引到龙卷风的中心
    private void PullPlayerTowardsTornado()
    {
        if (playerRigidbody != null)
        {
            Vector3 direction = (tornadoCenter.position - player.transform.position).normalized;
            playerRigidbody.AddForce(direction * pullForce * Time.deltaTime, ForceMode.Acceleration); // 向中心施加力
        }
    }
}
