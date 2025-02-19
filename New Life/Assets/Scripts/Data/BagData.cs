using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public enum ItemSortEnum
{
    Equip,          //װ��
    Consumables     //����Ʒ
}
public class BagData
{
    public string itemname;//����
    public int itemid;      //����
    public int itemsortIndex;//��Ʒ����
    public string itemdesc; //��ʾ��Ϣ
    public string itemspname;//ͼ��·��
    public int itemcount;    //����
    public int itemfunction; //����

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

    //��ȡ��Ʒ�ķ���
    public ItemSortEnum itemsort
    {
        get
        {
            return (ItemSortEnum)itemsortIndex;
        }
    }
}
