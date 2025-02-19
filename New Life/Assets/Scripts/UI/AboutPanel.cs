using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AboutPanel : BasePanel
{
    public Button btn;
    public override void Init()
    {
        btn.onClick.AddListener(() =>
        {
            UIDataMgr.Instance.HidePanel<AboutPanel>();
            UIDataMgr.Instance.showPanel<BeginPanel>();
        });
    }
}
