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

    // 血量相关字段
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

        // 获取场景中的所有龙卷风对象
        allTornadoes = FindObjectsOfType<TornadoMovement>();

        // 初始化UI显示
        UIDataMgr.Instance.showPanel<TestPanel>();
        UIDataMgr.Instance.GetPanel<TestPanel>().content.text = "<b><size=34>1.</size></b> 尽可能在健康值消耗前穿越沙漠\r\n\r\n<b><size=34>2.</size></b> 已为你自动装备防辐射防护服，大幅提升健康值\r\n\r\n<b><size=34>3.</size></b> 辐射防护药能为你恢复一定的健康值\r\n\r\n<b><size=34>4.</size></b> 龙卷风将直接影响你的血量，躲在安全区域即可躲避龙卷风";
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

    //更新最近安全区提示
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

    //更新最近的龙卷风提示
    private void UpdateTornadoTip()
    {
        TornadoMovement nearestTornado = GetNearestTornado();

        if (nearestTornado != null)
        {
            float distanceToTornado = Vector3.Distance(player.transform.position, nearestTornado.transform.position);

            if (distanceToTornado <= distance)
            {
                UIDataMgr.Instance.GetPanel<GamePanel>().tornadosTip.text = $"龙卷风即将来袭！距离你 {distanceToTornado.ToString("00")}m";
                UIDataMgr.Instance.GetPanel<GamePanel>().tornados.SetActive(true);
            }
            else
            {
                UIDataMgr.Instance.GetPanel<GamePanel>().tornados.SetActive(false);
            }
        }
    }

    //获取最近的龙卷风
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

    //初始化血量值
    public void InitializeHealth(float baseHealth, float itemHealthBonus)
    {
        maxHealth = baseHealth + itemHealthBonus;
        if (currentHealth == 0f)
        {
            currentHealth = maxHealth;
        }
    }

    //获取当前血量
    public static float GetCurrentHealth()
    {
        return currentHealth;
    }

    //设置当前血量
    public static void SetCurrentHealth(float value)
    {
        currentHealth = Mathf.Clamp(value, 0f, maxHealth);
    }

    //获取最大血量
    public static float GetMaxHealth()
    {
        return maxHealth;
    }
}