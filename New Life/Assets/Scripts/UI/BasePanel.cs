using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class BasePanel : MonoBehaviour
{
    //���ڿ������͸���ȵ����
    private CanvasGroup canvasGroup;
    //���뵭�����ٶ�
    private float alphaSpeed = 8;
    //��ǰ��ʾ��������
    public bool isShow = false;
    //��������������Ҫִ�е��¼�
    private UnityAction hideCallback = null;

    protected virtual void Awake()
    {
        //һ��ʼ��ȡ����������ϵ����
        canvasGroup = this.GetComponent<CanvasGroup>();

        //���������ӽű�
        if (canvasGroup == null)
            canvasGroup = this.gameObject.AddComponent<CanvasGroup>();
    }
    protected virtual void Start()
    {
        Init();
    }

    //���еĿؼ��¼�д������ д�ɳ��󷽷� ���������ȥʵ��
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
        //��������ʾ״̬ʱ  ���͸���Ȳ�Ϊ1�ͻ᲻ͣ�ļӵ�1 �ӵ�1�����ֹͣ�仯��
        //����
        if (isShow && canvasGroup.alpha != 1)
        {
            canvasGroup.alpha += alphaSpeed * Time.deltaTime;
            if (canvasGroup.alpha >= 1)
            {
                canvasGroup.alpha = 1;
            }
        }
        //����
        else if (!isShow && canvasGroup.alpha != 0)
        {
            canvasGroup.alpha -= alphaSpeed * Time.deltaTime;
            if (canvasGroup.alpha <= 0)
            {
                canvasGroup.alpha = 0;
                //����� ������ɺ� ȥִ�е�һЩ�߼�
                hideCallback?.Invoke();
            }
        }
    }
}
