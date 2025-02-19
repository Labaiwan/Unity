using Invector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerHealth : MonoBehaviour
{
    public Slider healthSlider; // ������ʾ����ֵ�Ļ���
    public Text hpTxt; // ������ʾ�˺��������ı�
    private vHealthController healthControl; // ����vHealthController���
    void Start()
    {

        healthControl = GameObject.Find("DefendTower").GetComponent<vHealthController>();
        healthControl.onReceiveDamage.AddListener(Damage); // ����ܵ��˺��¼��ļ�����
        healthSlider.maxValue = healthControl.maxHealth; // ���ý�����������ֵΪ�������ֵ
        healthSlider.value = healthSlider.maxValue; // ���ý�������ĳ�ʼֵΪ�������ֵ
        hpTxt.text = string.Empty; // ����˺��������ı�
    }

    public void Damage(vDamage damage)
    {
        hpTxt.text = healthSlider.value + "/" + healthSlider.maxValue; // �����˺��������ı�
        healthSlider.value -= damage.damageValue; // ���ٽ��������ֵ
    }
}
