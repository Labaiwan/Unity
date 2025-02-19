using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;



public class Chapter2Mgr
{
    private static Chapter2Mgr instance = new Chapter2Mgr();
    public static Chapter2Mgr Instance => instance;

    private Chapter2Mgr() { }

    //���еĳ��ֵ�
    public List<EnemyPoint> points = new List<EnemyPoint>();


    //��¼��ǰ�����ϵĹ���
    public List<Monster> monsterList = new List<Monster>();

    public EnemyPoint point;

    //�Ƿ�����Ϸ
    public bool isBegin = false;
    ////��¼��ǰ ���ж��ٲ�����
    public int nowWaveNum = 0;
    ////��¼ һ���ж��ٲ�����
    public int maxWaveNum = 0;


    //����һ���ж��ٲ�����
    public void UpdategetMaxNum(int num)
    {
        maxWaveNum += num;
        nowWaveNum = maxWaveNum;
    }

    public void ChangeNowWaveNum(int num)
    {
        nowWaveNum -= num;
    }


    //���ڼ�¼���ֵ�ķ���
    public void AddEnemyPoint(EnemyPoint point)
    { 
        points.Add(point);
    }

    //����Ƿ�ʤ��
    public bool checkWin()
    { 
        for (int i = 0; i < points.Count; i++)
        {
            //ֻҪ��һ�����ֵ� ��û�г���� ��ô��֤����û��ʤ��
            if (!points[i].Check0ver())
                return false;
        }

        if (monsterList.Count > 0)
            return false;
        return true;
    }

    //��¼���˵��б�֮��
    public void AddEnemy(Monster obj)
    { 
        monsterList.Add(obj);
    }

    //����������ʱ�������˴��б����Ƴ�
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
