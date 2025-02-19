using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BagSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    // 物品槽位的索引
    private int selfIndex;
    private Image selfImage;
    public BagData selfItem { get; private set; }
    private Text selfNum;

    // 初始化槽位
    public void Init(int index)
    {
        selfIndex = index;
        selfImage = transform.Find("icon").GetComponent<Image>();
        selfNum = transform.Find("num").GetComponent<Text>();
        Clear();
    }

    //判断槽位是否有物品
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

    //判断背包的某个物品为消耗品
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

    //设置槽位的物品
    public void SetItem(BagData itemInfo)
    {
        if (itemInfo.itemcount > 0)
        {
            selfItem = itemInfo;
            selfNum.gameObject.SetActive(true);
            Sprite sp = Resources.Load<Sprite>("Icon/" + itemInfo.itemspname);
            selfImage.sprite = sp;
            //动态添加后默认是透明的
            selfImage.color = Color.white;
            //如果是消耗品就显示数量
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
            Clear(); // 如果数量为0，则清除显示
        }
    }

    //向槽位中添加物品
    public void AddItem(int count)
    {
        if (selfItem != null && selfItem.itemcount + count > 0)
        {
            selfItem.itemcount += count;
            SetItem(selfItem); // 更新显示
        }
        else if (selfItem != null && selfItem.itemcount + count <= 0)
        {
            selfItem.itemcount = 0;
            //清除显示
            Clear();
        }
    }

    // 清除槽位的显示
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

    //当鼠标指针进入槽位时
    public void OnPointerEnter(PointerEventData eventData)
    {
        UIDataMgr.Instance.GetPanel<BagPanel>().SetCurrentSlot(this);
        if (HasItem())
        {
            UIDataMgr.Instance.GetPanel<BagPanel>().ShowItemInfo(selfItem, this.transform.position + new Vector3(30, -110, 0));
        }
    }

    //当鼠标指针离开槽位时调用
    public void OnPointerExit(PointerEventData eventData)
    {
        UIDataMgr.Instance.GetPanel<BagPanel>().HideItemInfo();
    }
}