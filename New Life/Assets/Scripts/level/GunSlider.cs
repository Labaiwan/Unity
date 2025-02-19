using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GunSlider : MonoBehaviour
{
    public Slider slider;
    public float timer;
    public bool isEnter = false;
    public float timefill = 10f;

    public Text sliderText;

    //��ʼ��ʾ ��¼��ʼʱ�� ���ý�����
    public void StartCharge()
    {
        timer = Time.time;
        isEnter = true;
        slider.value = 0;
        sliderText.text = "0%";
    }

    //ֹͣ��ʾ ���ý�����
    public void StopCharge()
    {
        isEnter = false;
        slider.value = 0;
        sliderText.text = "0%";
    }

    public void Update()
    {
        if (isEnter && slider.value < 1)
        {
            float time = Time.time - timer;
            slider.value = time / timefill;
            sliderText.text = (slider.value * 100).ToString("0") + "%";
        }
    }

    //�Ƿ�����
    public bool IsFull()
    {
        return slider.value >= 1;
    }

}
