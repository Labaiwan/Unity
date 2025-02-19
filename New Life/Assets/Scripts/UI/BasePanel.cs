using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class BasePanel : MonoBehaviour
{
    //用于控制面板透明度的组件
    private CanvasGroup canvasGroup;
    //淡入淡出的速度
    private float alphaSpeed = 8;
    //当前显示还是隐藏
    public bool isShow = false;
    //当隐藏完面板后所要执行的事件
    private UnityAction hideCallback = null;

    protected virtual void Awake()
    {
        //一开始获取挂载在面板上的组件
        canvasGroup = this.GetComponent<CanvasGroup>();

        //如果忘记添加脚本
        if (canvasGroup == null)
            canvasGroup = this.gameObject.AddComponent<CanvasGroup>();
    }
    protected virtual void Start()
    {
        Init();
    }

    //所有的控件事件写在这里 写成抽象方法 让子类必须去实现
    public abstract void Init();

    public virtual void ShowMe()
    {
        canvasGroup.alpha = 0;
        isShow = true;
    }

    public virtual void HideMe(UnityAction callBack)
    {
        canvasGroup.alpha = 1;
        isShow = false;
        hideCallback = callBack;
    }

    void Update()
    {
        //当处于显示状态时  如果透明度不为1就会不停的加到1 加到1过后就停止变化了
        //淡入
        if (isShow && canvasGroup.alpha != 1)
        {
            canvasGroup.alpha += alphaSpeed * Time.deltaTime;
            if (canvasGroup.alpha >= 1)
            {
                canvasGroup.alpha = 1;
            }
        }
        //淡出
        else if (!isShow && canvasGroup.alpha != 0)
        {
            canvasGroup.alpha -= alphaSpeed * Time.deltaTime;
            if (canvasGroup.alpha <= 0)
            {
                canvasGroup.alpha = 0;
                //让面板 淡出完成后 去执行的一些逻辑
                hideCallback?.Invoke();
            }
        }
    }
}
