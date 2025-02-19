using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class looktest : MonoBehaviour
{
    private EnemyVision enemyVision; // 用于存储敌人的EnemyVision脚本的引用

    void Start()
    {
        // 查找场景中第一个EnemyVision组件
        enemyVision = FindObjectOfType<EnemyVision>();

        if (enemyVision == null)
        {
            Debug.LogError("未能找到敌人的EnemyVision脚本！");
        }
    }

    void Update()
    {
        // 检查敌人是否看到玩家
        if (enemyVision != null)
        {
            if (enemyVision.PlayerInSight)
            {
                Debug.Log("玩家被敌人看到！");
                // 这里可以触发玩家被发现的逻辑，如玩家逃跑、警告音效等
            }
            else
            {
                Debug.Log("玩家没有被敌人看到。");
                // 玩家未被发现，可以执行正常行为
            }
        }
    }
}
