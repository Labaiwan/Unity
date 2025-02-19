using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class openBook : MonoBehaviour
{
    public GameObject bookCamera;
    public GameObject PlayerCamera;
    private bool isOpen = false;
    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M) && UIDataMgr.Instance.GetPanel<GamePanel>().tipChat.gameObject.activeSelf && isOpen)
        {
            PlayerCamera.SetActive(false);
            bookCamera.SetActive(true);
            UIDataMgr.Instance.HidePanel<GamePanel>();
            UIDataMgr.Instance.GetPanel<GamePanel>().tipChat.gameObject.SetActive(false);
            UIDataMgr.Instance.showPanel<OpenBookPanel>();

        }

        if (!bookCamera.activeSelf)
        {
            PlayerCamera.SetActive(true);
            if (!UIDataMgr.Instance.GetPanel<EndingPanel>().gameObject.activeSelf)
            {
                UIDataMgr.Instance.showPanel<GamePanel>();
                UIDataMgr.Instance.GetPanel<GamePanel>().resistance.SetActive(true);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isOpen = true;
            UIDataMgr.Instance.GetPanel<GamePanel>().tiptxtChat.text = "´ò¿ª";
            UIDataMgr.Instance.GetPanel<GamePanel>().tipChat.gameObject.SetActive(true);
            UIDataMgr.Instance.GetPanel<GamePanel>().tipChat.rectTransform.localPosition = new Vector3(other.transform.position.x, other.transform.position.y + 10, 0);
        }

    }

    private void OnTriggerExit(Collider other)
    {
        isOpen = false;
        UIDataMgr.Instance.GetPanel<GamePanel>().tipChat.gameObject.SetActive(false);
    }
}
