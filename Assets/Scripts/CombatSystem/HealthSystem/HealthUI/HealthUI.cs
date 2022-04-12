using System;
using System.Collections;
using Tools;
using UnityEngine;
using UnityEngine.UI;

namespace HealthSystem.HealthUI
{
    public class HealthUI : MonoBehaviour, IHealthUI 
    {
        
        public float tempMaxHealth = 100.0f;
        public float UISpeed = 4.0f;
        
        public SpriteRenderer MyRenderer { get; set;}
        public IMouseInput MyInput { get; set;}
        public Transform MyTransform { get; set; }

        public Slider HealthSlider;

        void Awake()
        {
            MyTransform = transform;
            MyRenderer = GetComponent<SpriteRenderer>();
            MyInput = GetComponent<IMouseInput>();
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
            while (lerpTime < UISpeed)
            {
                var sliderValue = HealthSlider.value;
                var _newhealth = Mathf.Lerp(sliderValue, _currentHealth, lerpTime/UISpeed);
                HealthSlider.value = _newhealth;
                lerpTime += Time.deltaTime;
                yield return null;
            }
            HealthSlider.value = _currentHealth;
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