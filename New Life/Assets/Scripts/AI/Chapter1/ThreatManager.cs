using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThreatManager : MonoBehaviour
{
    public static ThreatManager instance;
    public int threatLevel = 0;
    public float timeUntilDecrease = 40f; //减少仇恨值的时间
    private bool canDecreaseThreat = true; //是否可以减少仇恨值
    private GameObject randomPos;
    public GameObject tipArrow;
    public GameObject bigHouse;
    private bool isShow = false;
    public GameObject player;
    private void Awake()
    {
        instance = this;
        //显示仇恨值面板
        UIDataMgr.Instance.GetPanel<GamePanel>().thread.gameObject.SetActive(true);
        //显示任务提示面板
        UIDataMgr.Instance.showPanel<TestPanel>();
        UIDataMgr.Instance.GetPanel<TestPanel>().content.text = "<b><size=34>1.</size></b>当击杀怪物时会获得一点仇恨值，每40s内仇恨值为满时将消除一点仇恨值，当仇恨值满的时候所有怪物将全体向你进攻...\r\n\r\n<b><size=34>2.</size></b>当你攻击怪物或者靠近怪物时，怪物将对你发起进攻，当你主动攻击怪物并在15s没有进行下一次进攻，或者当你脱离怪物的追击范围，怪物将不会对你进攻...\r\n\r\n<b><size=34>3.</size></b>你需要在在小区的一楼内寻找到遗失的钥匙...\r\n\r\n<b><size=34>4.</size></b>当寻找到钥匙时，前往郊外的一座欧式建筑风格的房子内，打开二楼卧室的台灯,开启地下室，找到保险箱...";
    }

    private void Start()
    {
        randomPos = GameObject.Find("AgentPosition");
        // 获取所有的子物体
        Transform[] children = randomPos.GetComponentsInChildren<Transform>();
        //在这些点上生成敌人
        foreach (Transform child in children)
        {
            Instantiate(Resources.Load<GameObject>("Chapter1/Beast"), child.position, child.rotation);
        } 
    }

    private void Update()
    {
        PlayerDistance(bigHouse.transform);
        //当玩家找到钥匙后 显示指示器到达目的地
        if (GameDataMgr.Instance.BagDataList[3].itemcount == 1 && !isShow)
        {
            isShow = true;
            UIDataMgr.Instance.GetPanel<GamePanel>().ShowTipText("成功找到钥匙 前往下一个地点");
            tipArrow.SetActive(true);
            //设置目的地为bigHouse
            tipArrow.GetComponent<TargetToScreen>().TargetTransform = bigHouse.transform;
        }
        //满足条件 每隔40s减少一点仇恨值
        if (canDecreaseThreat && threatLevel > 0)
        {
            if (timeUntilDecrease > 0)
            {
                timeUntilDecrease -= Time.deltaTime;
            }
            else
            {
                DecreaseThreat();
                timeUntilDecrease = 40f; // 重置时间
            }
            UpdateThreadUI();
        }
    }
    //增加仇恨值
    public void AddThreat()
    {
        if (threatLevel < 5)
        {
            threatLevel++; 
            Debug.Log("Threat Level: " + threatLevel);
            CheckThreatLevel();
        }
    }

    //仇恨值满
    private void CheckThreatLevel()
    {
        if (threatLevel > 4)
        {
            // 触发所有敌人攻击
            StartCoroutine(AllEnemiesAttack());
            UpdateThreadUI();
            canDecreaseThreat = false; //停止减少仇恨值
        }
    }

    //当仇恨值满后 场景上所有的敌人都追击玩家
    private IEnumerator AllEnemiesAttack()
    {
        foreach (Enemy1_FSM enemy in FindObjectsOfType<Enemy1_FSM>())
        {
            Debug.Log("弟兄们冲啊");
            enemy.ChangeWalk();
        }
        yield return new WaitForSeconds(timeUntilDecrease);
        canDecreaseThreat = true; //恢复减少仇恨值
    }

    //减少仇恨值
    public void DecreaseThreat()
    {
        if (threatLevel > 0 && threatLevel < 5)
        {
            threatLevel--; //减少仇恨值
            Debug.Log("Threat Level decreased: " + threatLevel);
            UpdateThreadUI();
        }
    }

    // 更新UI显示
    public void UpdateThreadUI()
    { 
        // 先隐藏所有UI
        for (int i = 0; i < UIDataMgr.Instance.GetPanel<GamePanel>().threadImages.Length; i++)
        {
            UIDataMgr.Instance.GetPanel<GamePanel>().threadImages[i].gameObject.SetActive(false);
        }

        // 显示当前仇恨值对应的UI
        int maxIndex = Mathf.Min(threatLevel, UIDataMgr.Instance.GetPanel<GamePanel>().threadImages.Length);
        for (int i = 0; i < maxIndex; i++)
        {
            UIDataMgr.Instance.GetPanel<GamePanel>().threadImages[i].gameObject.SetActive(true);
        }
    }

    //设置指示器的目标
    public void TipArrowTar(Transform obj)
    {
        tipArrow.GetComponent<TargetToScreen>().TargetTransform = obj;
        PlayerDistance(obj);
    }

    //当靠近目标后就隐藏指示器
    public void PlayerDistance(Transform target)
    {
        if (Vector3.Distance(player.transform.position, target.position) < 5)
        {
            tipArrow.SetActive(false);
        }
    }
}