using Invector;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Player : MonoBehaviour
{
    private bool isBag;
    private bool isBack;
    private vHealthController health;

    void Start()
    {
        health = GetComponent<vHealthController>();
    }

    void Update()
    {
        OpenMyBag();
        OpenBack();
        Usepotion();
        Useresistance();

    }

    //使用药水
    private void Usepotion()
    {

        if (Input.GetKeyDown(KeyCode.Alpha1) && health.currentHealth < health.maxHealth && GameDataMgr.Instance.BagDataList[2].itemcount > 0)
        {
            GameDataMgr.Instance.ReduceItemFromBag(2, 1);
            health.AddHealth(GameDataMgr.Instance.BagDataList[2].itemfunction);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha1) && health.currentHealth == health.maxHealth && GameDataMgr.Instance.BagDataList[2].itemcount > 0)
        {
            UIDataMgr.Instance.GetPanel<GamePanel>().ShowTipText("你当前血量已满 无需使用药品");
        }
        else if(Input.GetKeyDown(KeyCode.Alpha1) && GameDataMgr.Instance.BagDataList[2].itemcount <= 0)
        {
            GameDataMgr.Instance.ReduceItemFromBag(2, 1);
        }

    }

    //使用抗辐射药
    private void Useresistance()
    {
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            if (Input.GetKeyDown(KeyCode.Alpha2) && GameDataMgr.Instance.BagDataList[5].itemcount > 0)
            {
                GameDataMgr.Instance.ReduceItemFromBag(5, 1);
                //UIDataMgr.Instance.GetPanel<GamePanel>().resistance.GetComponent<ResistanceValue>().healthSlider.value += GameDataMgr.Instance.BagDataList[5].itemfunction;
                UIDataMgr.Instance.GetPanel<GamePanel>().resistance.GetComponent<ResistanceValue>().IncreaseHealth(GameDataMgr.Instance.BagDataList[5].itemfunction);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2) && GameDataMgr.Instance.BagDataList[5].itemcount <= 0)
            {
                GameDataMgr.Instance.ReduceItemFromBag(5, 1);
            }
        }

    }

    //打开背包
    private void OpenMyBag()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            isBag = !isBag;
            if (isBag)
            {
                UIDataMgr.Instance.showPanel<BagPanel>();
            }
            else
            {
                UIDataMgr.Instance.HidePanel<BagPanel>();
                Cursor.visible = false;
            }
        }
    }

    //打开游戏菜单
    private void OpenBack()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isBack = !isBack; // 切换状态
            if (isBack)
            {
                UIDataMgr.Instance.showPanel<BackPanel>();
            }
            else
            {
                UIDataMgr.Instance.HidePanel<BackPanel>();
                Cursor.visible = false;
            }
        }
    }

    public void Dead()
    {
        UIDataMgr.Instance.showPanel<FailPanel>();
    }


    void OnDestroy()
    {
        // 在游戏对象被销毁时保存数据
        GameDataMgr.Instance.SaveBagData();
    }
}
