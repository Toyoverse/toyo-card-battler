using System;
using System.Collections;
using TMPro;
using Tools;
using UnityEngine;
using UnityEngine.UI;

namespace HealthSystem
{
    public class HealthPresenter : MonoBehaviour, IHealthPresenter 
    {
        
        private float tempMaxHealth = 100.0f;
        private float UISpeed = 1.0f;
        
        private IEnumerator smoothHealthCoroutine;
        private TextMeshProUGUI textValue;
        
        public SpriteRenderer MyRenderer { get; set;}
        public IMouseInput MyInput { get; set;}
        public Transform MyTransform { get; set; }
        public Slider HealthSlider;

        #region CallBacks

        private void Awake()
        {
            MyTransform = transform;
            MyRenderer = GetComponent<SpriteRenderer>();
            MyInput = GetComponent<IMouseInput>();
            textValue = GetComponentInChildren<TextMeshProUGUI>();
        }
        
            
        private void OnEnable()
        {
            OnUpdateHealthUI += UpdateHealthUI;
            SetMaxHealth();
        }

        private void OnDisable()
        {
            OnUpdateHealthUI -= UpdateHealthUI;
        }
        
        #endregion

        private void UpdateHealthUI(float _currentHealth)
        {
            if (smoothHealthCoroutine != null) StopCoroutine(smoothHealthCoroutine);
            smoothHealthCoroutine = SmoothHealthCoroutine(_currentHealth);
            StartCoroutine(smoothHealthCoroutine);
        }

        private IEnumerator SmoothHealthCoroutine(float _currentHealth)
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

        private void SetSliderValue(float _value)
        {
            HealthSlider.value = _value;
            textValue.text = Mathf.Round(_value).ToString();
        }

        private void SetMaxHealth()
        {
            HealthSlider.maxValue = tempMaxHealth;
            HealthSlider.value = tempMaxHealth;
        }

        public Transform Transform { get; }
        public Camera MainCamera => Camera.main;
        public MonoBehaviour MonoBehaviour => this;
        public GameObject GameObject { get; }

        SpriteRenderer IHealthPresenter.Renderer => MyRenderer;
        IMouseInput IHealthPresenter.Input => MyInput;

        #region Events

        public Action<float> OnUpdateHealthUI { get; set; }
        
        #endregion
    }
}