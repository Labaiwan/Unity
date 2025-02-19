using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BackPanel : BasePanel
{
    public Button btnSet;
    public Button btnBack; 
    public Button btnQuit;

    public override void Init()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        btnSet.onClick.AddListener(() => 
        {
            UIDataMgr.Instance.showPanel<SettingPanel>();
            UIDataMgr.Instance.HidePanel<BackPanel>();
        });

        btnBack.onClick.AddListener(() => 
        {
            AsyncOperation operation = SceneManager.LoadSceneAsync(0);
            operation.completed += ((value) =>
            {
                UIDataMgr.Instance.HidePanel<GamePanel>();
                UIDataMgr.Instance.HidePanel<BackPanel>();
                UIDataMgr.Instance.showPanel<BeginPanel>();
            });
          
        });

        btnQuit.onClick.AddListener(() =>
        {
            UIDataMgr.Instance.HidePanel<BackPanel>();
            Cursor.visible = false;
        });
        
    }
}
