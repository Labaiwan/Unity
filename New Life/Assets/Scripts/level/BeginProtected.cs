using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeginProtected : MonoBehaviour
{
    public GameObject enemyPoint;
    void Start()
    {
        UIDataMgr.Instance.GetPanel<GamePanel>().thread.SetActive(false);
        UIDataMgr.Instance.GetPanel<GamePanel>().tipChat.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Chapter2Mgr.Instance.isBegin) 
        {
            UIDataMgr.Instance.GetPanel<GamePanel>().tower.SetActive(true);
            enemyPoint.gameObject.SetActive(true);
        }

        if (Chapter2Mgr.Instance.checkWin())
        {
            UIDataMgr.Instance.GetPanel<GamePanel>().tower.SetActive(false);
        }
    }
}
