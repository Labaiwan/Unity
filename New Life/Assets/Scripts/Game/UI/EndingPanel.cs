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
        //���Text������ı����Ա�������ʾ
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

        // �ı���ʾ��ɺ���ʾ�û���������
        tip.text = "����������ص����˵�";

        // �ȴ��û���������
        yield return new WaitUntil(() => Input.anyKeyDown);

        // �û������������첽�������˵�����
        yield return StartCoroutine(LoadMainMenuAsync());
        this.gameObject.SetActive(false);
    }

    private IEnumerator LoadMainMenuAsync()
    {
        // �첽���س���
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("BeginScene");

        // �ȴ������������
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

    }

    
}
