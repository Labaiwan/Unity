using Invector;
using UnityEngine;
using UnityEngine.UI;

public class ResistanceValue : MonoBehaviour
{
    public Slider healthSlider;
    public Text hpTxt;
    private float timer = 0f;
    private GameObject player;
    private int baseHealth = 200;

    void Start()
    {
        if (GameDataMgr.Instance.BagDataList[4].itemcount == 1)
        {
            GameDataMgr.Instance.ReduceItemFromBag(4,1);
        }
        player = GameObject.Find("Player");
        //��ʼ��Ѫ��ֵ
        float itemHealthBonus = GameDataMgr.Instance.BagDataList[4].itemfunction;
        Chapter3.Instance.InitializeHealth(baseHealth, itemHealthBonus);

        //���� Slider �����ֵ�͵�ǰֵ
        healthSlider.maxValue = Chapter3.GetMaxHealth();
        healthSlider.value = Chapter3.GetCurrentHealth();
        UpdateHealthText();
    }

    void Update()
    {
        //����ÿ���Ѫ
        timer += Time.deltaTime;
        if (timer >= 0.35f && !player.GetComponent<vHealthController>().isDead && !Chapter3.Instance.isLast)
        {
            if (Chapter3.GetCurrentHealth() > 0)
            {
                Chapter3.SetCurrentHealth(Chapter3.GetCurrentHealth() - 1);
                healthSlider.value = Chapter3.GetCurrentHealth();
                UpdateHealthText();
            }
            else
            {
                vHealthController damage = player.GetComponent<vHealthController>();
                damage.TakeDamage(new vDamage(1));
            }
            timer = 0f;
        }
    }

    //������ʾѪ�����ı�
    void UpdateHealthText()
    {
        if (hpTxt != null)
        {
            hpTxt.text = Chapter3.GetCurrentHealth() + "/" + healthSlider.maxValue;
        }
    }

    public void IncreaseHealth(float amount)
    {
        float newHealth = Mathf.Min(Chapter3.GetCurrentHealth() + amount, Chapter3.GetMaxHealth());
        Chapter3.SetCurrentHealth(newHealth);
        healthSlider.value = newHealth;
        UpdateHealthText();
    }
}