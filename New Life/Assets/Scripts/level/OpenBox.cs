using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenBox : MonoBehaviour
{
    private Animator ani;
    private bool isconform = false;
    private bool isOpen = false;
    public NpcControl over;
    void Start()
    {
        ani = GetComponent<Animator>();
       
    }


    void Update()
    {
        if (isconform)
        {
            if (Input.GetKeyDown(KeyCode.M) && UIDataMgr.Instance.GetPanel<GamePanel>().tipChat.gameObject.activeSelf == true && this.gameObject.name == this.gameObject.name)
            {
                if (GameDataMgr.Instance.BagDataList[3].itemcount >= 1)
                {
                    ani.SetBool("Open", true);
                    GameDataMgr.Instance.ReduceItemFromBag(3, 1);
                    GameDataMgr.Instance.AddItemToBag(1, 4);
                    over = over.GetComponent<NpcControl>();
                    isOpen = true;
                    over.ChatName = "over";
                    over.Say();
                }
                else if(GameDataMgr.Instance.BagDataList[3].itemcount < 1 && !isOpen)
                {
                    GameDataMgr.Instance.ReduceItemFromBag(3, 1);
                }
            }
        }
       
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            UIDataMgr.Instance.GetPanel<GamePanel>().tiptxtChat.text = "开启";
            UIDataMgr.Instance.GetPanel<GamePanel>().tipChat.gameObject.SetActive(true);
            UIDataMgr.Instance.GetPanel<GamePanel>().tipChat.rectTransform.localPosition = new Vector3(other.transform.position.x, other.transform.position.y, 0);
            isconform = true;
            if (isOpen)
            {
                UIDataMgr.Instance.GetPanel<GamePanel>().tiptxt.text = "宝箱已经打开 无法再次开启";
            }
        }


    }

    private void OnTriggerExit(Collider other)
    {
        UIDataMgr.Instance.GetPanel<GamePanel>().tipChat.gameObject.SetActive(false);
    }

}
