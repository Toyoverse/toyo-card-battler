using System;
using System.Collections;
using TMPro;
using Tools;
using UnityEngine;
using UnityEngine.UI;

namespace HealthSystem.HealthUI
{
    public class HealthUI : MonoBehaviour, IHealthUI 
    {
        
        private float tempMaxHealth = 100.0f;
        private float UISpeed = 1.0f;

        private TextMeshProUGUI textValue;
        
        public SpriteRenderer MyRenderer { get; set;}
        public IMouseInput MyInput { get; set;}
        public Transform MyTransform { get; set; }

        public Slider HealthSlider;

        void Awake()
        {
            MyTransform = transform;
            MyRenderer = GetComponent<SpriteRenderer>();
            MyInput = GetComponent<IMouseInput>();
            textValue = GetComponentInChildren<TextMeshProUGUI>();
        }
        
            
        void OnEnable()
        {
            OnUpdateHealthUI += UpdateHealthUI;
            SetMaxHealth();
        }

        void OnDisable()
        {
            OnUpdateHealthUI -= UpdateHealthUI;
        }

        void UpdateHealthUI(float _currentHealth)
        {
            if (smoothHealthCoroutine != null) StopCoroutine(smoothHealthCoroutine);
            smoothHealthCoroutine = SmoothHealthCoroutine(_currentHealth);
            StartCoroutine(smoothHealthCoroutine);
        }
        
        private IEnumerator smoothHealthCoroutine;

        IEnumerator SmoothHealthCoroutine(float _currentHealth)
        {
            var lerpTime = 0.0f;
            var sliderValue = HealthSlider.value;
            while (lerpTime < UISpeed)
            {
                var _newhealth = Mathf.Lerp(sliderValue, _currentHealth, lerpTime/UISpeed);
                SetSliderValue(_newhealth);
                lerpTime += Time.deltaTime;
                yield return null;
            }
            SetSliderValue(_currentHealth);
        }

        void SetSliderValue(float _value)
        {
            HealthSlider.value = _value;
            textValue.text = Mathf.Round(_value).ToString();
        }

        void SetMaxHealth()
        {
            HealthSlider.maxValue = tempMaxHealth;
            HealthSlider.value = tempMaxHealth;
        }

        public Camera MainCamera => Camera.main;
        public MonoBehaviour MonoBehaviour => this;
        public Action<float> OnUpdateHealthUI { get; set; }

        SpriteRenderer IHealthUI.Renderer => MyRenderer;
        IMouseInput IHealthUI.Input => MyInput;
        
    }
}