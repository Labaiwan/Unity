using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction
{
    North,
    East,
    South,
    West
}
public class EnemyPoint : MonoBehaviour
{
    //怪物有多少波
    public int maxWave;
    //每波怪物有多少只
    public int monsterNumOneWave;
    //用于记录 当前波的怪物还有多少只没有创建
    private int nowNum;

    //用于记录 当前波 要创建什么ID的怪物
    private int nowID;

    //单只怪物创建间隔时间
    public float createOffsetTime;
    //波与波之间的间隔时间
    public float delayTime;
    //第一波怪物创建的间隔时间
    public float firstDelayTime;
    private GameObject[] enemys;


    public Direction currentdirect = Direction.North;
    void Start()
    {
        enemys = Resources.LoadAll<GameObject>("Enemy");
        Invoke("CreateWave", firstDelayTime);
        //记录出怪点
        Chapter2Mgr.Instance.AddEnemyPoint(this);
        //更新最大波数
        Chapter2Mgr.Instance.UpdategetMaxNum(maxWave);
    }

    private void CreateWave()
    {
        //得到当前怪物的ID
        //nowID = Random.Range(0, enemys.Length);
        nowID = (Random.value < 0.8f) ? 0 : Random.Range(1, enemys.Length);
        //当前波怪物有多少只
        nowNum = monsterNumOneWave;
        //创建怪物
        CreateEnemy();
        --maxWave;
        //通知管理器 出了一波怪
        Chapter2Mgr.Instance.ChangeNowWaveNum(1);
    }

    private void CreateEnemy()
    {
        GameObject obj = Instantiate(enemys[nowID], this.transform.position, Quaternion.identity);
        Monster monster = obj.GetComponent<Monster>();
        //记录怪物到列表中
        Chapter2Mgr.Instance.AddEnemy(monster);
        //怪物攻击的城墙方向
        switch (currentdirect)
        {
            case Direction.North:
                monster.dire = "Northtower";
                break;
            case Direction.South:
                monster.dire = "Southtower";
                break;
            case Direction.East:
                monster.dire = "Easttower";
                break;

        }
        //创建完一只怪物后 减去要创建的怪物数量1
        --nowNum;
        if (nowNum == 0)
        {
            if (maxWave > 0)
            {
                Invoke("CreateWave", delayTime);
            }
        }
        else
        {
            Invoke("CreateEnemy", createOffsetTime);
        }
    }

    //检查出怪点的怪物是否出完
    public bool Check0ver()
    { 
        return nowNum <= 0 && maxWave <= 0;
    }

    public void stopOver()
    { 
        nowNum = maxWave = 0;
    }

    //游戏失败
    public void StopSpawning()
    {
        // 取消所有待执行的Invoke调用
        CancelInvoke("CreateWave");
        CancelInvoke("CreateEnemy");

        // 重置相关状态变量
        nowNum = 0;
        maxWave = 0;
    }
}
