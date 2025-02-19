using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chapter3 : MonoBehaviour
{
    private static Chapter3 instance;
    public static Chapter3 Instance => instance;

    private List<GameObject> safehouseList = new List<GameObject>();
    public bool isSafe = false;
    private GameObject player;
    private static TornadoMovement[] allTornadoes;
    public int distance = 550;
    public bool isLast = false;

    // Ѫ������ֶ�
    private static float currentHealth;
    private static float maxHealth;

    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        player = GameObject.Find("Player");
        SafeHouse[] safehouses = FindObjectsOfType<SafeHouse>();
        foreach (SafeHouse safe in safehouses)
        {
            safehouseList.Add(safe.gameObject);
        }

        // ��ȡ�����е�������������
        allTornadoes = FindObjectsOfType<TornadoMovement>();

        // ��ʼ��UI��ʾ
        UIDataMgr.Instance.showPanel<TestPanel>();
        UIDataMgr.Instance.GetPanel<TestPanel>().content.text = "<b><size=34>1.</size></b> �������ڽ���ֵ����ǰ��ԽɳĮ\r\n\r\n<b><size=34>2.</size></b> ��Ϊ���Զ�װ��������������������������ֵ\r\n\r\n<b><size=34>3.</size></b> �������ҩ��Ϊ��ָ�һ���Ľ���ֵ\r\n\r\n<b><size=34>4.</size></b> ����罫ֱ��Ӱ�����Ѫ�������ڰ�ȫ���򼴿ɶ�������";
        UIDataMgr.Instance.GetPanel<TestPanel>().DoThing(() =>
        {
            UIDataMgr.Instance.GetPanel<GamePanel>().resistance.SetActive(true);
        });
    }

    void Update()
    {
        UpdateSafehouseTip();
        UpdateTornadoTip();
        UIDataMgr.Instance.GetPanel<GamePanel>().resistance.gameObject.SetActive(true);
    }

    //���������ȫ����ʾ
    private void UpdateSafehouseTip()
    {
        GameObject nearestSafehouse = null;
        float minDistance = float.MaxValue;

        foreach (GameObject safehouse in safehouseList)
        {
            float distance = Vector3.Distance(player.transform.position, safehouse.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                nearestSafehouse = safehouse;
            }
        }

        if (nearestSafehouse != null && UIDataMgr.Instance.GetPanel<GamePanel>().tornados.activeSelf)
        {
            UIDataMgr.Instance.GetPanel<GamePanel>().safehouseTip.GetComponent<TargetToScreen>().TargetTransform = nearestSafehouse.transform;
            UIDataMgr.Instance.GetPanel<GamePanel>().safehouseTip.SetActive(true);
        }

        if (isSafe == true)
        {
            UIDataMgr.Instance.GetPanel<GamePanel>().safehouseTip.SetActive(false);
        }
    }

    //����������������ʾ
    private void UpdateTornadoTip()
    {
        TornadoMovement nearestTornado = GetNearestTornado();

        if (nearestTornado != null)
        {
            float distanceToTornado = Vector3.Distance(player.transform.position, nearestTornado.transform.position);

            if (distanceToTornado <= distance)
            {
                UIDataMgr.Instance.GetPanel<GamePanel>().tornadosTip.text = $"����缴����Ϯ�������� {distanceToTornado.ToString("00")}m";
                UIDataMgr.Instance.GetPanel<GamePanel>().tornados.SetActive(true);
            }
            else
            {
                UIDataMgr.Instance.GetPanel<GamePanel>().tornados.SetActive(false);
            }
        }
    }

    //��ȡ����������
    private TornadoMovement GetNearestTornado()
    {
        TornadoMovement nearest = null;
        float shortestDistance = float.MaxValue;

        foreach (var tornado in allTornadoes)
        {
            float distance = Vector3.Distance(player.transform.position, tornado.transform.position);
            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                nearest = tornado;
            }
        }

        return nearest;
    }

    public void StopAllTornados()
    {
        isLast = true;
        if (allTornadoes != null)
        {
            foreach (TornadoMovement tornado in allTornadoes)
            {
                if (tornado != null && tornado.agent != null)
                {
                    tornado.agent.enabled = false;
                }
            }
        }
    }

    //��ʼ��Ѫ��ֵ
    public void InitializeHealth(float baseHealth, float itemHealthBonus)
    {
        maxHealth = baseHealth + itemHealthBonus;
        if (currentHealth == 0f)
        {
            currentHealth = maxHealth;
        }
    }

    //��ȡ��ǰѪ��
    public static float GetCurrentHealth()
    {
        return currentHealth;
    }

    //���õ�ǰѪ��
    public static void SetCurrentHealth(float value)
    {
        currentHealth = Mathf.Clamp(value, 0f, maxHealth);
    }

    //��ȡ���Ѫ��
    public static float GetMaxHealth()
    {
        return maxHealth;
    }
}