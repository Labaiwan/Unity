using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BagPanel : BasePanel
{
    private List<BagSlot> allslots;//���и���
    private Transform info;//��Ϣ��ʾ��常��
    private Text infoTitle;//���ı���
    private Text infoTips; //������ʾ
    public Button btnQuit;//�رհ�ť
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
        //�������и���
        Transform slotRoot = this.transform.Find("Slots");
        for (int i = 0; i < slotRoot.childCount; i++)
        {
            //��ȡ��ǰ���ӵ���Ϸ����
            GameObject tmpSlot = slotRoot.GetChild(i).gameObject;
            //��ÿ���������BagSlot���
            BagSlot bagSlot = tmpSlot.AddComponent<BagSlot>();
            bagSlot.Init(i);
            allslots.Add(bagSlot);
        }
        RefreshPanel();
    }

    //���뱳��������
    public void PickUp(BagData itemInfo)
    {
        for (int i = 0; i < allslots.Count; i++)
        {
            //�������Ʒ����������Ʒ
            if (allslots[i].HasItem() && allslots[i].ItemCanAdd() && allslots[i].selfItem.itemid == itemInfo.itemid)
            {
                allslots[i].AddItem(itemInfo.itemcount);
                break;
            }
            else if (!allslots[i].HasItem())
            {
                //���û����Ʒ���Ҳ�������Ʒ
                allslots[i].SetItem(itemInfo);
                break;
            }
        }
    }

    //���õ�ǰѡ�еĸ���
    public void SetCurrentSlot(BagSlot slot)
    {
        currentSlot = slot;
    }

    //��ʾ��Ʒ����ʾ��Ϣ
    public void ShowItemInfo(BagData itemInfo, Vector3 pos)
    {
        info.position = pos;
        infoTitle.text = itemInfo.itemname;
        infoTips.text = itemInfo.itemdesc;
        info.gameObject.SetActive(true);
    }

    //������Ʒ��Ϣ
    public void HideItemInfo()
    {
        info.gameObject.SetActive(false);
    }

    // ˢ�±������
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
