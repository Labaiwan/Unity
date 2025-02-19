using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpenBookPanel : BasePanel
{
    public Button btn;
    public GameObject book;
    public override void Init()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        book = GameObject.Find("noteCamera");
        btn.onClick.AddListener(()=>
        { 
            book.gameObject.SetActive(false);
            UIDataMgr.Instance.HidePanel<OpenBookPanel>();
            UIDataMgr.Instance.showPanel<GamePanel>();
            UIDataMgr.Instance.GetPanel<GamePanel>().tipChat.gameObject.SetActive(false);
            
        });
    }
}
