using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameDataMgr
{
    private static GameDataMgr instance = new GameDataMgr();
    public static GameDataMgr Instance => instance;

    //��������
    public MusicData musicData;
    //��������
    public List<BagData> BagDataList;

    private GameDataMgr()
    {
        //��ʼ��Ĭ������
        musicData = JsonMgr.Instance.LoadData<MusicData>("MusicData");
        BagDataList = JsonMgr.Instance.LoadData<List<BagData>>("BagData");
        

    }

    //�洢��������
    public void SaveMusicData()
    {
        JsonMgr.Instance.SavaData(musicData, "MusicData");
    }

    //������Ч
    public void PlaySound(string resName, int DestoryTime, float volume = 1f)
    {
        GameObject obj = new GameObject();
        AudioSource source = obj.AddComponent<AudioSource>();
        source.clip = Resources.Load<AudioClip>("Audio"+"/" + resName);
        source.volume = volume;
        source.Play();
        GameObject.Destroy(obj, DestoryTime);
    }

    //�洢��������
    public void SaveBagData()
    {
        JsonMgr.Instance.SavaData(BagDataList, "BagData");
    }

    //���ⲿͨ��idȥ��ȡ������Ʒ��Ϣ
    public BagData GetItem(int id)
    {
        for (int i = 0; i < BagDataList.Count; i++)
        {
            if (BagDataList[i].itemid == id)
            {
                return BagDataList[i];
            }
        }
        return null;
    }

    //����Ҫ�޸�����Ʒ����Ϣ
    public BagData GetNewItem(int id)
    {
        BagData originItem = GetItem(id);
        BagData newItem = new BagData(originItem);
        SaveBagData();
        return newItem;
    }

    //�����������Ʒ
    public void AddItemToBag(int n, int itemId)
    {
        BagData item = GetNewItem(itemId);
        for (int i = 0; i < n; i++)
        {           
            if (item.itemcount > 0)
            {
                foreach (var bagItem in BagDataList)
                {
                    if (bagItem.itemid == item.itemid)
                    {
                        bagItem.itemcount += item.itemcount;
                        break;
                    }
                }
                if (!BagDataList.Exists(b => b.itemid == item.itemid))
                {
                    BagDataList.Add(item);
                }
                SaveBagData();
                UIDataMgr.Instance.GetPanel<GamePanel>().ShowTipText("���"+item.itemname + "*" + n);
                UIDataMgr.Instance.GetPanel<BagPanel>()?.RefreshPanel();
            }
        }
    }

    //���ٱ����е���Ʒ����
    public void ReduceItemFromBag(int itemId, int amount)
    {
        //���ұ����ж�ӦID����Ʒ
        for (int i = 0; i < BagDataList.Count; i++)
        {
            if (BagDataList[i].itemid == itemId)
            {
                //������ٺ������
                int newCount = BagDataList[i].itemcount - amount;
                if (newCount < 0)
                {
                    //������ٺ������С��0 ��ʾ��ʾ���
                    UIDataMgr.Instance.GetPanel<GamePanel>().ShowTipText("���" + BagDataList[i].itemname + "�������㣡����");
                    return;
                }
                else if (newCount == 0)
                {
                    //�����������0 �ӱ����б����Ƴ�����Ʒ
                    UIDataMgr.Instance.GetPanel<BagPanel>()?.RefreshPanel();
                    BagDataList[i].itemcount = 0;
                }
                else
                {
                    //���� ������Ʒ������
                    BagDataList[i].itemcount = newCount;
                    UIDataMgr.Instance.GetPanel<BagPanel>()?.RefreshPanel();
                    UIDataMgr.Instance.GetPanel<GamePanel>().ShowTipText("��ʹ����" + BagDataList[i].itemname + "* 1");
                }
                SaveBagData();
                break;
            }
        }
    }

   
}








