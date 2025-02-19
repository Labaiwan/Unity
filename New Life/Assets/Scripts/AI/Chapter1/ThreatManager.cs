using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThreatManager : MonoBehaviour
{
    public static ThreatManager instance;
    public int threatLevel = 0;
    public float timeUntilDecrease = 40f; //���ٳ��ֵ��ʱ��
    private bool canDecreaseThreat = true; //�Ƿ���Լ��ٳ��ֵ
    private GameObject randomPos;
    public GameObject tipArrow;
    public GameObject bigHouse;
    private bool isShow = false;
    public GameObject player;
    private void Awake()
    {
        instance = this;
        //��ʾ���ֵ���
        UIDataMgr.Instance.GetPanel<GamePanel>().thread.gameObject.SetActive(true);
        //��ʾ������ʾ���
        UIDataMgr.Instance.showPanel<TestPanel>();
        UIDataMgr.Instance.GetPanel<TestPanel>().content.text = "<b><size=34>1.</size></b>����ɱ����ʱ����һ����ֵ��ÿ40s�ڳ��ֵΪ��ʱ������һ����ֵ�������ֵ����ʱ�����й��ｫȫ���������...\r\n\r\n<b><size=34>2.</size></b>���㹥��������߿�������ʱ�����ｫ���㷢����������������������ﲢ��15sû�н�����һ�ν��������ߵ�����������׷����Χ�����ｫ����������...\r\n\r\n<b><size=34>3.</size></b>����Ҫ����С����һ¥��Ѱ�ҵ���ʧ��Կ��...\r\n\r\n<b><size=34>4.</size></b>��Ѱ�ҵ�Կ��ʱ��ǰ�������һ��ŷʽ�������ķ����ڣ��򿪶�¥���ҵ�̨��,���������ң��ҵ�������...";
    }

    private void Start()
    {
        randomPos = GameObject.Find("AgentPosition");
        // ��ȡ���е�������
        Transform[] children = randomPos.GetComponentsInChildren<Transform>();
        //����Щ�������ɵ���
        foreach (Transform child in children)
        {
            Instantiate(Resources.Load<GameObject>("Chapter1/Beast"), child.position, child.rotation);
        } 
    }

    private void Update()
    {
        PlayerDistance(bigHouse.transform);
        //������ҵ�Կ�׺� ��ʾָʾ������Ŀ�ĵ�
        if (GameDataMgr.Instance.BagDataList[3].itemcount == 1 && !isShow)
        {
            isShow = true;
            UIDataMgr.Instance.GetPanel<GamePanel>().ShowTipText("�ɹ��ҵ�Կ�� ǰ����һ���ص�");
            tipArrow.SetActive(true);
            //����Ŀ�ĵ�ΪbigHouse
            tipArrow.GetComponent<TargetToScreen>().TargetTransform = bigHouse.transform;
        }
        //�������� ÿ��40s����һ����ֵ
        if (canDecreaseThreat && threatLevel > 0)
        {
            if (timeUntilDecrease > 0)
            {
                timeUntilDecrease -= Time.deltaTime;
            }
            else
            {
                DecreaseThreat();
                timeUntilDecrease = 40f; // ����ʱ��
            }
            UpdateThreadUI();
        }
    }
    //���ӳ��ֵ
    public void AddThreat()
    {
        if (threatLevel < 5)
        {
            threatLevel++; 
            Debug.Log("Threat Level: " + threatLevel);
            CheckThreatLevel();
        }
    }

    //���ֵ��
    private void CheckThreatLevel()
    {
        if (threatLevel > 4)
        {
            // �������е��˹���
            StartCoroutine(AllEnemiesAttack());
            UpdateThreadUI();
            canDecreaseThreat = false; //ֹͣ���ٳ��ֵ
        }
    }

    //�����ֵ���� ���������еĵ��˶�׷�����
    private IEnumerator AllEnemiesAttack()
    {
        foreach (Enemy1_FSM enemy in FindObjectsOfType<Enemy1_FSM>())
        {
            Debug.Log("�����ǳ尡");
            enemy.ChangeWalk();
        }
        yield return new WaitForSeconds(timeUntilDecrease);
        canDecreaseThreat = true; //�ָ����ٳ��ֵ
    }

    //���ٳ��ֵ
    public void DecreaseThreat()
    {
        if (threatLevel > 0 && threatLevel < 5)
        {
            threatLevel--; //���ٳ��ֵ
            Debug.Log("Threat Level decreased: " + threatLevel);
            UpdateThreadUI();
        }
    }

    // ����UI��ʾ
    public void UpdateThreadUI()
    { 
        // ����������UI
        for (int i = 0; i < UIDataMgr.Instance.GetPanel<GamePanel>().threadImages.Length; i++)
        {
            UIDataMgr.Instance.GetPanel<GamePanel>().threadImages[i].gameObject.SetActive(false);
        }

        // ��ʾ��ǰ���ֵ��Ӧ��UI
        int maxIndex = Mathf.Min(threatLevel, UIDataMgr.Instance.GetPanel<GamePanel>().threadImages.Length);
        for (int i = 0; i < maxIndex; i++)
        {
            UIDataMgr.Instance.GetPanel<GamePanel>().threadImages[i].gameObject.SetActive(true);
        }
    }

    //����ָʾ����Ŀ��
    public void TipArrowTar(Transform obj)
    {
        tipArrow.GetComponent<TargetToScreen>().TargetTransform = obj;
        PlayerDistance(obj);
    }

    //������Ŀ��������ָʾ��
    public void PlayerDistance(Transform target)
    {
        if (Vector3.Distance(player.transform.position, target.position) < 5)
        {
            tipArrow.SetActive(false);
        }
    }
}