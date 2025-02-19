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
        healthControl.onReceiveDamage.AddListener(Damage); //添加受到伤害事件的监听器
        healthSlider.maxValue = healthControl.maxHealth; //设置健康滑块的最大值为最大生命值
        healthSlider.value = healthSlider.maxValue; //设置健康滑块的初始值为最大生命值
        hpTxt.text = string.Empty; //清空伤害计数器文本
        healthBar.SetActive(false); //一开始设置为不可见
    }

    void Update()
    {
        //如果没有健康控制器或生命值为0
        if (healthControl.currentHealth <= 0)
            Destroy(gameObject);

        healthSlider.value = healthControl.currentHealth; //更新健康滑块的值为当前生命值
    }

    public void Damage(vDamage damage)
    {
        hpTxt.text = healthSlider.value + "/" + healthSlider.maxValue; //更新伤害计数器文本
        healthSlider.value -= damage.damageValue; //减少健康滑块的值

        if (healthBar && !healthBar.activeSelf)
            healthBar.SetActive(true);
    }
}

