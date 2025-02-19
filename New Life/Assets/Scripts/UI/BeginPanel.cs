using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BeginPanel : BasePanel
{
    public Button btnStart;
    public Button btnSetting;
    public Button btnResume;
    public Button btnQuit;

    public override void Init()
    {        
        btnStart.onClick.AddListener(()=>
        {
            UIDataMgr.Instance.showPanel<LoadPanel>();           
            
        });

        btnSetting.onClick.AddListener(() =>
        {
            UIDataMgr.Instance.showPanel<SettingPanel>();
        });

        btnResume.onClick.AddListener(() =>
        {
            UIDataMgr.Instance.showPanel<AboutPanel>();
            UIDataMgr.Instance.HidePanel<BeginPanel>();
        });

        btnQuit.onClick.AddListener(() =>
        {
            Application.Quit();
        });
    }

    
    

    
}
