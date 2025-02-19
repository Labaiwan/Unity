using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenLight : MonoBehaviour
{
    public GameObject underdoor;
    public Transform underdoorPos;
    private bool isOpen = false;
    void Start()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L) && UIDataMgr.Instance.GetPanel<GamePanel>().tipChat.gameObject.activeSelf == true && !isOpen && this.gameObject.name == this.gameObject.name)
        {
            isOpen = true;
            UIDataMgr.Instance.GetPanel<GamePanel>().tipChat.gameObject.SetActive(false);
            Instantiate(Resources.Load<GameObject>("light"),this.transform.position + this.transform.up * 0.6f ,Quaternion.identity);
            Destroy(underdoor);
            GameDataMgr.Instance.PlaySound("underdoor",5);
            ThreatManager.instance.tipArrow.gameObject.SetActive(true);

        }
        if (isOpen)
        {
            ThreatManager.instance.TipArrowTar(underdoorPos);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && !isOpen && this.gameObject.name == "Lamp")
        {           
            UIDataMgr.Instance.GetPanel<GamePanel>().tiptxtChat.text = "¿ªÆô";
            UIDataMgr.Instance.GetPanel<GamePanel>().tipChat.gameObject.SetActive(true);
            UIDataMgr.Instance.GetPanel<GamePanel>().tipChat.rectTransform.localPosition = new Vector3(this.transform.position.x, this.transform.position.y + 2, this.transform.position.z);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        UIDataMgr.Instance.GetPanel<GamePanel>().tipChat.gameObject.SetActive(false);
    }
}
