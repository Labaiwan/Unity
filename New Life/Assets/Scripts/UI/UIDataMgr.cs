using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIDataMgr
{
    private static UIDataMgr instance = new UIDataMgr();
    public static UIDataMgr Instance => instance;

    //���ڴ洢��ʾ�ŵ����� ÿ��ʾһ����� �ͻ��������ֵ�
    //�������ʱ ֱ�ӻ�ȡ�ֵ��еĶ�Ӧ��� ��������
    private Dictionary<string, BasePanel> panelDic = new Dictionary<string, BasePanel>();

    //�����е� canvas���� ��������Ϊ���ĸ�����
    private Transform canvasPos;

    private UIDataMgr()
    {
        //�õ�Canvas�Ķ���
        GameObject canvas = GameObject.Instantiate(Resources.Load<GameObject>("BeginUI/Canvas"));
        canvasPos = canvas.transform;
        //ͨ��������Ƴ��ö���
        GameObject.DontDestroyOnLoad(canvas);
    }

    //��ʾ���
    public T showPanel<T>() where T : BasePanel
    {
        //��֤����T�����ͺ����Ԥ������һ��
        string panelName = typeof(T).Name;

        //�ж��ֵ����Ƿ��Ѿ���ʾ��������
        if (panelDic.ContainsKey(panelName))
            return panelDic[panelName] as T;

        //��ʾ��� ����������� ��̬�Ĵ���Ԥ���� ���ø�����
        GameObject panelObj = GameObject.Instantiate(Resources.Load<GameObject>("BeginUI/" + panelName));
        //��������� �ŵ������е� canvas����
        panelObj.transform.SetParent(canvasPos, false);

        T panel = panelObj.GetComponent<T>();
        //��������ű��洢���ֵ��� ����֮��Ļ�ȡ������
        panelDic.Add(panelName, panel);
        //�����Լ�����ʾ�߼�
        panel.ShowMe();
        return panel;
    }

    /// <summary>
    /// �������
    /// </summary>
    /// <typeparam name="T">�������</typeparam>
    /// <param name="isFade">�Ƿ񵭳���Ϲ����ɾ����� Ĭ����ture</param>
    public void HidePanel<T>(bool isFade = true) where T : BasePanel
    {
        //���ݷ��ͻ������
        string panelName = typeof(T).Name;
        //�жϵ�ǰ��ʾ����� ��û������Ҫ���ص�
        if (panelDic.ContainsKey(panelName))
        {
            if (isFade)
            {
                //����� ������Ϲ��� ��ɾ��
                panelDic[panelName].HideMe(() =>
                {
                    //ɾ������
                    GameObject.Destroy(panelDic[panelName].gameObject);
                    //ɾ���ֵ�����ڵ����ű�
                    panelDic.Remove(panelName);

                });
            }
            else
            {
                //ɾ������
                GameObject.Destroy(panelDic[panelName].gameObject);
                //ɾ���ֵ�����ڵ����ű�
                panelDic.Remove(panelName);

            }
        }
    }

    //�õ����
    public T GetPanel<T>() where T : BasePanel
    {
        string panelName = typeof(T).Name;
        if (panelDic.ContainsKey(panelName))
            return panelDic[panelName] as T;
        //���û�ж�Ӧ�����ʾ �ͷ��ؿ�
        return null;
    }
}
