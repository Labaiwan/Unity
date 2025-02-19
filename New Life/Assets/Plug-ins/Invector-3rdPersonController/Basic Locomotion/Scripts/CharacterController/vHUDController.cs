using UnityEngine;
using UnityEngine.UI;

namespace Invector.vCharacterController
{
    public class vHUDController : MonoBehaviour
    {
        #region General Variables

        #region Health/Stamina Variables
        [Header("Health/Stamina")]
        public Slider healthSlider;
        public float healthSliderMaxValueSmooth = 10f;
        public float healthSliderValueSmooth = 10;

        [Header("DamageHUD")]
        public Image damageImage;
        public float flashSpeed = 5f;
        public Color flashColour = new Color(1f, 0f, 0f, 0.1f);
        [HideInInspector] public bool damaged;
        #endregion
        #endregion

        private static vHUDController _instance;
        public static vHUDController instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = GameObject.FindObjectOfType<vHUDController>();
                }
                return _instance;
            }
        }

        void Start()
        {
            
        }

        public void Init(vThirdPersonController cc)
        {
            cc.onDead.AddListener(OnDead);
            cc.onReceiveDamage.AddListener(EnableDamageSprite);
            damageImage.color = new Color(0f, 0f, 0f, 0f);
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

        public virtual void UpdateHUD(vThirdPersonController cc)
        {
            UpdateSliders(cc);
            ShowDamageSprite();
        }

       
        void UpdateSliders(vThirdPersonController cc)
        {
            if (healthSlider != null)
            {
                if (cc.maxHealth != healthSlider.maxValue)
                {
                    healthSlider.maxValue = Mathf.Lerp(healthSlider.maxValue, cc.maxHealth, healthSliderMaxValueSmooth * Time.fixedDeltaTime);
                    healthSlider.onValueChanged.Invoke(healthSlider.value);
                }
                healthSlider.value = Mathf.Lerp(healthSlider.value, cc.currentHealth, healthSliderValueSmooth * Time.fixedDeltaTime);
            }
        }

        public void ShowDamageSprite()
        {
            if (damaged)
            {
                damaged = false;
                if (damageImage != null)
                    damageImage.color = flashColour;
            }
            else if (damageImage != null)
                damageImage.color = Color.Lerp(damageImage.color, Color.clear, flashSpeed * Time.deltaTime);
        }

        public void EnableDamageSprite(vDamage damage)
        {
            if (damageImage != null)
                damageImage.enabled = true;
            damaged = true;
        }

    }
}