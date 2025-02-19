using Invector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    public GameObject healthBar;
    public Slider healthSlider; 
    public Text hpTxt; 
    private vHealthController healthControl;

    void Start() 
    {
        healthControl = transform.GetComponentInParent<vHealthController>();
        healthControl.onReceiveDamage.AddListener(Damage); //����ܵ��˺��¼��ļ�����
        healthSlider.maxValue = healthControl.maxHealth; //���ý�����������ֵΪ�������ֵ
        healthSlider.value = healthSlider.maxValue; //���ý�������ĳ�ʼֵΪ�������ֵ
        hpTxt.text = string.Empty; //����˺��������ı�
        healthBar.SetActive(false); //һ��ʼ����Ϊ���ɼ�
    }

    void Update()
    {
        //���û�н���������������ֵΪ0
        if (healthControl.currentHealth <= 0)
            Destroy(gameObject);

        healthSlider.value = healthControl.currentHealth; //���½��������ֵΪ��ǰ����ֵ
    }

    public void Damage(vDamage damage)
    {
        hpTxt.text = healthSlider.value + "/" + healthSlider.maxValue; //�����˺��������ı�
        healthSlider.value -= damage.damageValue; //���ٽ��������ֵ

        if (healthBar && !healthBar.activeSelf)
            healthBar.SetActive(true);
    }
}

