using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BagPanel : BasePanel
{
    private List<BagSlot> allslots;//所有格子
    private Transform info;//信息提示面板父类
    private Text infoTitle;//面板的标题
    private Text infoTips; //面板的提示
    public Button btnQuit;//关闭按钮
    private BagSlot currentSlot;
    public override void Init()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        info = this.transform.Find("Info");
        infoTitle = info.Find("title").GetComponent<Text>();
        infoTips = info.Find("tips").GetComponent<Text>();

        btnQuit.onClick.AddListener(() =>
        {
            UIDataMgr.Instance.HidePanel<BagPanel>();
            Cursor.visible = false;
        });

        allslots = new List<BagSlot>();
        //遍历所有格子
        Transform slotRoot = this.transform.Find("Slots");
        for (int i = 0; i < slotRoot.childCount; i++)
        {
            //获取当前格子的游戏对象
            GameObject tmpSlot = slotRoot.GetChild(i).gameObject;
            //给每个格子添加BagSlot组件
            BagSlot bagSlot = tmpSlot.AddComponent<BagSlot>();
            bagSlot.Init(i);
            allslots.Add(bagSlot);
        }
        RefreshPanel();
    }

    //放入背包格子中
    public void PickUp(BagData itemInfo)
    {
        for (int i = 0; i < allslots.Count; i++)
        {
            //如果有物品并且是消耗品
            if (allslots[i].HasItem() && allslots[i].ItemCanAdd() && allslots[i].selfItem.itemid == itemInfo.itemid)
            {
                allslots[i].AddItem(itemInfo.itemcount);
                break;
            }
            else if (!allslots[i].HasItem())
            {
                //如果没有物品并且不是消耗品
                allslots[i].SetItem(itemInfo);
                break;
            }
        }
    }

    //设置当前选中的格子
    public void SetCurrentSlot(BagSlot slot)
    {
        currentSlot = slot;
    }

    //显示物品的提示信息
    public void ShowItemInfo(BagData itemInfo, Vector3 pos)
    {
        info.position = pos;
        infoTitle.text = itemInfo.itemname;
        infoTips.text = itemInfo.itemdesc;
        info.gameObject.SetActive(true);
    }

    //隐藏物品信息
    public void HideItemInfo()
    {
        info.gameObject.SetActive(false);
    }

    // 刷新背包面板
    public void RefreshPanel()
    {
        foreach (var slot in allslots)
        {
            slot.Clear();
        }

        foreach (var item in GameDataMgr.Instance.BagDataList)
        {
            if (item.itemcount >= 1)
            {
                PickUp(item);
            }
        }
    }
}
