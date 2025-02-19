using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickObj : MonoBehaviour
{
    public int BagId;
    private bool isPick = false;
    void Start()
    {
            
    }


    void Update()
    {
        if (isPick)
        {
            if (Input.GetKeyDown(KeyCode.M) && UIDataMgr.Instance.GetPanel<GamePanel>().tipChat.gameObject.activeSelf == true )
            {
                GameDataMgr.Instance.AddItemToBag(1, BagId);
                Destroy(gameObject);
                UIDataMgr.Instance.GetPanel<GamePanel>().tipChat.gameObject.SetActive(false);
            }
        }


    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            UIDataMgr.Instance.GetPanel<GamePanel>().tiptxtChat.text = "拾​​取";
            UIDataMgr.Instance.GetPanel<GamePanel>().tipChat.gameObject.SetActive(true);
            UIDataMgr.Instance.GetPanel<GamePanel>().tipChat.rectTransform.localPosition = new Vector3(other.transform.position.x, other.transform.position.y, 0);
            isPick = true;
        }


    }

    private void OnTriggerExit(Collider other)
    {
        UIDataMgr.Instance.GetPanel<GamePanel>().tipChat.gameObject.SetActive(false);
    }
}
