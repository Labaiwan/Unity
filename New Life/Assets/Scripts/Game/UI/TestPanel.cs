using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TestPanel : BasePanel
{
    public Text content;
    public Button btnSure;
    private UnityAction hideCallback = null;
    public override void Init()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        btnSure.onClick.AddListener(() =>
        {
            UIDataMgr.Instance.HidePanel<TestPanel>();
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            hideCallback?.Invoke();
        });
    }

    public virtual void DoThing(UnityAction callBack)
    {
        hideCallback = callBack;
    }

}
