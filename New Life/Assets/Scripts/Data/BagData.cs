using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public enum ItemSortEnum
{
    Equip,          //装备
    Consumables     //消耗品
}
public class BagData
{
    public string itemname;//名称
    public int itemid;      //序列
    public int itemsortIndex;//物品类型
    public string itemdesc; //提示信息
    public string itemspname;//图标路径
    public int itemcount;    //数量
    public int itemfunction; //功能

    public BagData()
    {

    }

    public BagData(BagData item)
    {
        itemcount = 1;
        this.itemname = item.itemname;
        this.itemid = item.itemid;
        this.itemsortIndex = item.itemsortIndex;
        this.itemdesc = item.itemdesc;
        this.itemspname = item.itemspname;
        this.itemfunction = item.itemfunction;
    }

    //获取物品的分类
    public ItemSortEnum itemsort
    {
        get
        {
            return (ItemSortEnum)itemsortIndex;
        }
    }
}
