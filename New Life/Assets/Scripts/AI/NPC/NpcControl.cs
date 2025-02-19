using Fungus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class NpcControl : MonoBehaviour
{
    public string ChatName;
    //当前是否可以对话
    private bool isChat;
    public Flowchart flowchart;
    //是否主动聊天
    public bool isActive;

    void Start()
    {
        //获取场景上的对话脚本
        flowchart = flowchart.GetComponent<Flowchart>();
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && UIDataMgr.Instance.GetPanel<GamePanel>().tipChat.gameObject.activeSelf == true)
        {
            UIDataMgr.Instance.GetPanel<GamePanel>().tipChat.gameObject.SetActive(false);
            Say();
            // 使NPC面向玩家
            AlignWithPlayer();
        }
    
    }

    private void OnTriggerEnter(Collider other)
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        isChat = true;
        if (other.gameObject.CompareTag("Player") && this.gameObject.CompareTag("Npc"))
        {
            UIDataMgr.Instance.GetPanel<GamePanel>().tiptxtChat.text = "对话";
            UIDataMgr.Instance.GetPanel<GamePanel>().tipChat.gameObject.SetActive(true);
            UIDataMgr.Instance.GetPanel<GamePanel>().tipChat.rectTransform.localPosition = new Vector3(other.transform.position.x + 3, other.transform.position.y, 0);
        }
        else if (other.CompareTag("Player") && this.gameObject.CompareTag("TriggerPlace"))
        {
            Say();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        isChat = false;
        UIDataMgr.Instance.GetPanel<GamePanel>().tipChat.gameObject.SetActive(false);
        Cursor.visible = false;
        //Cursor.lockState = CursorLockMode.Locked;
    }

    public void Say()
    {
        if (isChat)
        {
            //对话是否存在
            if (flowchart.HasBlock(ChatName))
            {
                //执行对话
                flowchart.ExecuteBlock(ChatName);               
            }
        }
    }

    //使NPC面朝向玩家
    private void AlignWithPlayer()
    {
        Transform player = GameObject.FindGameObjectWithTag("Player").transform;
        if (player != null)
        {
            //计算NPC和玩家之间的相对位置
            Vector3 relativePos = player.position - transform.position;
            //计算Y轴旋转角度
            float angle = Mathf.Atan2(relativePos.x, relativePos.z) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.x, angle, transform.rotation.z));
        }
    }

    public void NextScene()
    {
        UIDataMgr.Instance.showPanel<LoadPanel>();
    }

    public void AddObj()
    {
        GameDataMgr.Instance.AddItemToBag(1,6);
    }
}