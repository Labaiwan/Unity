using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chapter4 : MonoBehaviour
{
    
    void Start()
    {

    }

    
    void Update()
    {
        UIDataMgr.Instance.GetPanel<GamePanel>().minMap.gameObject.SetActive(false);
        UIDataMgr.Instance.GetPanel<GamePanel>().resistance.gameObject.SetActive(false);
    }

    public void Over()
    {
        Cursor.visible = true;
        UIDataMgr.Instance.HidePanel<GamePanel>();
        UIDataMgr.Instance.showPanel<EndingPanel>();
    }
}
