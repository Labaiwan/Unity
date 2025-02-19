using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BagSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    // ��Ʒ��λ������
    private int selfIndex;
    private Image selfImage;
    public BagData selfItem { get; private set; }
    private Text selfNum;

    // ��ʼ����λ
    public void Init(int index)
    {
        selfIndex = index;
        selfImage = transform.Find("icon").GetComponent<Image>();
        selfNum = transform.Find("num").GetComponent<Text>();
        Clear();
    }

    //�жϲ�λ�Ƿ�����Ʒ
    public bool HasItem()
    {
        if (selfItem != null)
        {
            return true;
        }
        else if (selfItem == null)
        {
            return false;
        }
        return false;
    }

    //�жϱ�����ĳ����ƷΪ����Ʒ
    public bool ItemCanAdd()
    {
        if (selfItem.itemsort == ItemSortEnum.Consumables)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    //���ò�λ����Ʒ
    public void SetItem(BagData itemInfo)
    {
        if (itemInfo.itemcount > 0)
        {
            selfItem = itemInfo;
            selfNum.gameObject.SetActive(true);
            Sprite sp = Resources.Load<Sprite>("Icon/" + itemInfo.itemspname);
            selfImage.sprite = sp;
            //��̬��Ӻ�Ĭ����͸����
            selfImage.color = Color.white;
            //���������Ʒ����ʾ����
            if (itemInfo.itemsortIndex == (int)ItemSortEnum.Consumables)
            {
                selfNum.text = itemInfo.itemcount.ToString();
            }
            else
            {
                selfNum.text = "";
            }
        }
        else
        {
            Clear(); // �������Ϊ0���������ʾ
        }
    }

    //���λ�������Ʒ
    public void AddItem(int count)
    {
        if (selfItem != null && selfItem.itemcount + count > 0)
        {
            selfItem.itemcount += count;
            SetItem(selfItem); // ������ʾ
        }
        else if (selfItem != null && selfItem.itemcount + count <= 0)
        {
            selfItem.itemcount = 0;
            //�����ʾ
            Clear();
        }
    }

    // �����λ����ʾ
    public void Clear()
    {
        if (selfImage != null)
        {
            selfImage.color = Color.clear;
        }
        if (selfNum != null)
        {
            selfNum.text = "";
        }
        selfItem = null;
    }

    //�����ָ������λʱ
    public void OnPointerEnter(PointerEventData eventData)
    {
        UIDataMgr.Instance.GetPanel<BagPanel>().SetCurrentSlot(this);
        if (HasItem())
        {
            UIDataMgr.Instance.GetPanel<BagPanel>().ShowItemInfo(selfItem, this.transform.position + new Vector3(30, -110, 0));
        }
    }

    //�����ָ���뿪��λʱ����
    public void OnPointerExit(PointerEventData eventData)
    {
        UIDataMgr.Instance.GetPanel<BagPanel>().HideItemInfo();
    }
}