using Invector;
using Invector.vCharacterController;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;


public class GamePanel : BasePanel
{
    public Slider healthSlider;
    public float healthSliderMaxValueSmooth = 10f;
    public float healthSliderValueSmooth = 10;

    public Text tiptxt;
    public Image tipChat;
    public Text tiptxtChat;
    public GameObject thread;
    public Image[] threadImages;
    public GameObject tower;
    public GameObject resistance;
    public GameObject tornados;
    public Text tornadosTip;
    public GameObject safehouseTip;
    public GameObject minMap;
    public override void Init()
    {
        tipChat.gameObject.SetActive(false);
        tiptxt.gameObject.SetActive(false);
        tipChat.gameObject.SetActive(false);
        thread.gameObject.SetActive(false);
        tower.gameObject.SetActive(false);
        resistance.gameObject.SetActive(false);
        tornados.gameObject.SetActive(false);
        safehouseTip.gameObject.SetActive(false);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
    public void Initd(vThirdPersonController cc)
    {
        cc.onDead.AddListener(OnDead);
        if (healthSlider)
        {
            if (cc.maxHealth != healthSlider.maxValue)
            {
                healthSlider.maxValue = cc.maxHealth;
                healthSlider.onValueChanged.Invoke(healthSlider.value);
            }
            healthSlider.value = cc.currentHealth;
        }

    }
    private void OnDead(GameObject arg0)
    {
        Debug.Log("You are Dead!");
    }

    // 显示tiptxt文本三秒后消失
    public void ShowTipText(string message)
    {
        tiptxt.text = message;
        tiptxt.gameObject.SetActive(true);
        StartCoroutine(HideTipText());
    }

    private IEnumerator HideTipText()
    {
        yield return new WaitForSeconds(3);
        tiptxt.gameObject.SetActive(false);
    }


}
