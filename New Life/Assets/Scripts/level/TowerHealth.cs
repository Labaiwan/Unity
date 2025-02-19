using Invector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerHealth : MonoBehaviour
{
    public Slider healthSlider; // 用于显示健康值的滑块
    public Text hpTxt; // 用于显示伤害计数的文本
    private vHealthController healthControl; // 引用vHealthController组件
    void Start()
    {

        healthControl = GameObject.Find("DefendTower").GetComponent<vHealthController>();
        healthControl.onReceiveDamage.AddListener(Damage); // 添加受到伤害事件的监听器
        healthSlider.maxValue = healthControl.maxHealth; // 设置健康滑块的最大值为最大生命值
        healthSlider.value = healthSlider.maxValue; // 设置健康滑块的初始值为最大生命值
        hpTxt.text = string.Empty; // 清空伤害计数器文本
    }

    public void Damage(vDamage damage)
    {
        hpTxt.text = healthSlider.value + "/" + healthSlider.maxValue; // 更新伤害计数器文本
        healthSlider.value -= damage.damageValue; // 减少健康滑块的值
    }
}
