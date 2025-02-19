using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndingPanel : BasePanel
{
    public Text content;
    public Text tip;
    private string fullText;
    private string currentText = "";
    public float delay = 0.1f;

    public override void Init()
    {
        fullText = content.text;
        //清空Text组件的文本，以便逐字显示
        content.text = "";
        StartCoroutine(ShowText());
        GameObject.Find("Player").SetActive(false);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    private IEnumerator ShowText()
    {
        for (int i = 0; i < fullText.Length; i++)
        {
            currentText = fullText.Substring(0, i + 1);
            content.text = currentText;
            yield return new WaitForSeconds(delay);
        }

        // 文本显示完成后，提示用户点击任意键
        tip.text = "请点击任意键回到主菜单";

        // 等待用户点击任意键
        yield return new WaitUntil(() => Input.anyKeyDown);

        // 用户点击任意键后，异步加载主菜单场景
        yield return StartCoroutine(LoadMainMenuAsync());
        this.gameObject.SetActive(false);
    }

    private IEnumerator LoadMainMenuAsync()
    {
        // 异步加载场景
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("BeginScene");

        // 等待场景加载完成
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

    }

    
}
