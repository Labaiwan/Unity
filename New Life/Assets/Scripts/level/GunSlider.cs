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

    //开始显示 记录开始时间 重置进度条
    public void StartCharge()
    {
        timer = Time.time;
        isEnter = true;
        slider.value = 0;
        sliderText.text = "0%";
    }

    //停止显示 重置进度条
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

    //是否填满
    public bool IsFull()
    {
        return slider.value >= 1;
    }

}
