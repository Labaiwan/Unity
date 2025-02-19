using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIDataMgr
{
    private static UIDataMgr instance = new UIDataMgr();
    public static UIDataMgr Instance => instance;

    //用于存储显示着的面板的 每显示一个面板 就会存入这个字典
    //隐藏面板时 直接获取字典中的对应面板 进行隐藏
    private Dictionary<string, BasePanel> panelDic = new Dictionary<string, BasePanel>();

    //场景中的 canvas对象 用于设置为面板的父对象
    private Transform canvasPos;

    private UIDataMgr()
    {
        //得到Canvas的对象
        GameObject canvas = GameObject.Instantiate(Resources.Load<GameObject>("BeginUI/Canvas"));
        canvasPos = canvas.transform;
        //通过过场最不移除该对象
        GameObject.DontDestroyOnLoad(canvas);
    }

    //显示面板
    public T showPanel<T>() where T : BasePanel
    {
        //保证泛型T的类型和面板预设体名一样
        string panelName = typeof(T).Name;

        //判断字典中是否已经显示了这个面板
        if (panelDic.ContainsKey(panelName))
            return panelDic[panelName] as T;

        //显示面板 根据面板名字 动态的创建预设体 设置父对象
        GameObject panelObj = GameObject.Instantiate(Resources.Load<GameObject>("BeginUI/" + panelName));
        //把这个对象 放到场景中的 canvas下面
        panelObj.transform.SetParent(canvasPos, false);

        T panel = panelObj.GetComponent<T>();
        //把这个面板脚本存储到字典中 方便之后的获取和隐藏
        panelDic.Add(panelName, panel);
        //调用自己的显示逻辑
        panel.ShowMe();
        return panel;
    }

    /// <summary>
    /// 隐藏面板
    /// </summary>
    /// <typeparam name="T">面板类名</typeparam>
    /// <param name="isFade">是否淡出完毕过后才删除面板 默认是ture</param>
    public void HidePanel<T>(bool isFade = true) where T : BasePanel
    {
        //根据泛型获得名字
        string panelName = typeof(T).Name;
        //判断当前显示的面板 有没有你想要隐藏的
        if (panelDic.ContainsKey(panelName))
        {
            if (isFade)
            {
                //让面板 淡出完毕过后 再删除
                panelDic[panelName].HideMe(() =>
                {
                    //删除对象
                    GameObject.Destroy(panelDic[panelName].gameObject);
                    //删除字典里存在的面板脚本
                    panelDic.Remove(panelName);

                });
            }
            else
            {
                //删除对象
                GameObject.Destroy(panelDic[panelName].gameObject);
                //删除字典里存在的面板脚本
                panelDic.Remove(panelName);

            }
        }
    }

    //得到面板
    public T GetPanel<T>() where T : BasePanel
    {
        string panelName = typeof(T).Name;
        if (panelDic.ContainsKey(panelName))
            return panelDic[panelName] as T;
        //如果没有对应面板显示 就返回空
        return null;
    }
}
