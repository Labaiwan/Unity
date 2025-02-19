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

    //ʹ��ҩˮ
    private void Usepotion()
    {

        if (Input.GetKeyDown(KeyCode.Alpha1) && health.currentHealth < health.maxHealth && GameDataMgr.Instance.BagDataList[2].itemcount > 0)
        {
            GameDataMgr.Instance.ReduceItemFromBag(2, 1);
            health.AddHealth(GameDataMgr.Instance.BagDataList[2].itemfunction);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha1) && health.currentHealth == health.maxHealth && GameDataMgr.Instance.BagDataList[2].itemcount > 0)
        {
            UIDataMgr.Instance.GetPanel<GamePanel>().ShowTipText("�㵱ǰѪ������ ����ʹ��ҩƷ");
        }
        else if(Input.GetKeyDown(KeyCode.Alpha1) && GameDataMgr.Instance.BagDataList[2].itemcount <= 0)
        {
            GameDataMgr.Instance.ReduceItemFromBag(2, 1);
        }

    }

    //ʹ�ÿ�����ҩ
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

    //�򿪱���
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

    //����Ϸ�˵�
    private void OpenBack()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isBack = !isBack; // �л�״̬
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
        // ����Ϸ��������ʱ��������
        GameDataMgr.Instance.SaveBagData();
    }
}
