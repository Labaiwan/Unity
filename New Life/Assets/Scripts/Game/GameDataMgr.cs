using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameDataMgr
{
    private static GameDataMgr instance = new GameDataMgr();
    public static GameDataMgr Instance => instance;

    //音乐数据
    public MusicData musicData;
    //背包数据
    public List<BagData> BagDataList;

    private GameDataMgr()
    {
        //初始化默认数据
        musicData = JsonMgr.Instance.LoadData<MusicData>("MusicData");
        BagDataList = JsonMgr.Instance.LoadData<List<BagData>>("BagData");
        

    }

    //存储音乐数据
    public void SaveMusicData()
    {
        JsonMgr.Instance.SavaData(musicData, "MusicData");
    }

    //播放音效
    public void PlaySound(string resName, int DestoryTime, float volume = 1f)
    {
        GameObject obj = new GameObject();
        AudioSource source = obj.AddComponent<AudioSource>();
        source.clip = Resources.Load<AudioClip>("Audio"+"/" + resName);
        source.volume = volume;
        source.Play();
        GameObject.Destroy(obj, DestoryTime);
    }

    //存储背包数据
    public void SaveBagData()
    {
        JsonMgr.Instance.SavaData(BagDataList, "BagData");
    }

    //给外部通过id去获取背包物品信息
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

    //增加要修改了物品的信息
    public BagData GetNewItem(int id)
    {
        BagData originItem = GetItem(id);
        BagData newItem = new BagData(originItem);
        SaveBagData();
        return newItem;
    }

    //往背包添加物品
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
                UIDataMgr.Instance.GetPanel<GamePanel>().ShowTipText("获得"+item.itemname + "*" + n);
                UIDataMgr.Instance.GetPanel<BagPanel>()?.RefreshPanel();
            }
        }
    }

    //减少背包中的物品数量
    public void ReduceItemFromBag(int itemId, int amount)
    {
        //查找背包中对应ID的物品
        for (int i = 0; i < BagDataList.Count; i++)
        {
            if (BagDataList[i].itemid == itemId)
            {
                //计算减少后的数量
                int newCount = BagDataList[i].itemcount - amount;
                if (newCount < 0)
                {
                    //如果减少后的数量小于0 显示提示面板
                    UIDataMgr.Instance.GetPanel<GamePanel>().ShowTipText("你的" + BagDataList[i].itemname + "数量不足！！！");
                    return;
                }
                else if (newCount == 0)
                {
                    //如果数量减至0 从背包列表中移除该物品
                    UIDataMgr.Instance.GetPanel<BagPanel>()?.RefreshPanel();
                    BagDataList[i].itemcount = 0;
                }
                else
                {
                    //否则 更新物品的数量
                    BagDataList[i].itemcount = newCount;
                    UIDataMgr.Instance.GetPanel<BagPanel>()?.RefreshPanel();
                    UIDataMgr.Instance.GetPanel<GamePanel>().ShowTipText("你使用了" + BagDataList[i].itemname + "* 1");
                }
                SaveBagData();
                break;
            }
        }
    }

   
}








