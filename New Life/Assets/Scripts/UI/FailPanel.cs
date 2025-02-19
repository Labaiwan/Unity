using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FailPanel : BasePanel
{
    public Button btnBack;
    public Button btnResume;

    public override void Init()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        btnBack.onClick.AddListener(()=>
        {
            UIDataMgr.Instance.HidePanel<FailPanel>();
            SceneManager.LoadSceneAsync("BeginScene");
        });

        btnResume.onClick.AddListener(() =>
        {
            UIDataMgr.Instance.HidePanel<FailPanel>();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        });
    }

}
