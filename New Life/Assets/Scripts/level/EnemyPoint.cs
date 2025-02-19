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
    //�����ж��ٲ�
    public int maxWave;
    //ÿ�������ж���ֻ
    public int monsterNumOneWave;
    //���ڼ�¼ ��ǰ���Ĺ��ﻹ�ж���ֻû�д���
    private int nowNum;

    //���ڼ�¼ ��ǰ�� Ҫ����ʲôID�Ĺ���
    private int nowID;

    //��ֻ���ﴴ�����ʱ��
    public float createOffsetTime;
    //���벨֮��ļ��ʱ��
    public float delayTime;
    //��һ�����ﴴ���ļ��ʱ��
    public float firstDelayTime;
    private GameObject[] enemys;


    public Direction currentdirect = Direction.North;
    void Start()
    {
        enemys = Resources.LoadAll<GameObject>("Enemy");
        Invoke("CreateWave", firstDelayTime);
        //��¼���ֵ�
        Chapter2Mgr.Instance.AddEnemyPoint(this);
        //���������
        Chapter2Mgr.Instance.UpdategetMaxNum(maxWave);
    }

    private void CreateWave()
    {
        //�õ���ǰ�����ID
        //nowID = Random.Range(0, enemys.Length);
        nowID = (Random.value < 0.8f) ? 0 : Random.Range(1, enemys.Length);
        //��ǰ�������ж���ֻ
        nowNum = monsterNumOneWave;
        //��������
        CreateEnemy();
        --maxWave;
        //֪ͨ������ ����һ����
        Chapter2Mgr.Instance.ChangeNowWaveNum(1);
    }

    private void CreateEnemy()
    {
        GameObject obj = Instantiate(enemys[nowID], this.transform.position, Quaternion.identity);
        Monster monster = obj.GetComponent<Monster>();
        //��¼���ﵽ�б���
        Chapter2Mgr.Instance.AddEnemy(monster);
        //���﹥���ĳ�ǽ����
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
        //������һֻ����� ��ȥҪ�����Ĺ�������1
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

    //�����ֵ�Ĺ����Ƿ����
    public bool Check0ver()
    { 
        return nowNum <= 0 && maxWave <= 0;
    }

    public void stopOver()
    { 
        nowNum = maxWave = 0;
    }

    //��Ϸʧ��
    public void StopSpawning()
    {
        // ȡ�����д�ִ�е�Invoke����
        CancelInvoke("CreateWave");
        CancelInvoke("CreateEnemy");

        // �������״̬����
        nowNum = 0;
        maxWave = 0;
    }
}
