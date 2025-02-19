using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;



public class Chapter2Mgr
{
    private static Chapter2Mgr instance = new Chapter2Mgr();
    public static Chapter2Mgr Instance => instance;

    private Chapter2Mgr() { }

    //所有的出怪点
    public List<EnemyPoint> points = new List<EnemyPoint>();


    //记录当前场景上的怪物
    public List<Monster> monsterList = new List<Monster>();

    public EnemyPoint point;

    //是否开启游戏
    public bool isBegin = false;
    ////记录当前 还有多少波怪物
    public int nowWaveNum = 0;
    ////记录 一共有多少波怪物
    public int maxWaveNum = 0;


    //更新一共有多少波敌人
    public void UpdategetMaxNum(int num)
    {
        maxWaveNum += num;
        nowWaveNum = maxWaveNum;
    }

    public void ChangeNowWaveNum(int num)
    {
        nowWaveNum -= num;
    }


    //用于记录出怪点的方法
    public void AddEnemyPoint(EnemyPoint point)
    { 
        points.Add(point);
    }

    //检测是否胜利
    public bool checkWin()
    { 
        for (int i = 0; i < points.Count; i++)
        {
            //只要有一个出怪点 还没有出完怪 那么就证明还没有胜利
            if (!points[i].Check0ver())
                return false;
        }

        if (monsterList.Count > 0)
            return false;
        return true;
    }

    //记录敌人到列表之中
    public void AddEnemy(Monster obj)
    { 
        monsterList.Add(obj);
    }

    //当敌人死亡时，将敌人从列表中移除
    public void RemoveEnemy(Monster obj)
    {
        monsterList.Remove(obj);
    }

    public void ClearInfo()
    {
        for (int i = 0; i < points.Count; i++)
        {
            points[i].StopSpawning();
        }

        for (int i = 0; i < monsterList.Count; i++)
        {
            monsterList[i].stopAgent();
        }

        points.Clear();

        nowWaveNum = maxWaveNum = 0;
    }
}
