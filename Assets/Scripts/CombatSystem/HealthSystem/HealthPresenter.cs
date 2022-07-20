using System;
using System.Collections;
using Player;
using TMPro;
using Tools;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace HealthSystem
{
    public class HealthPresenter : MonoBehaviour 
    {
        public Slider healthSlider;
        public float uISpeed = 1.0f;
        
        private IEnumerator _smoothHealthCoroutine;
        private TextMeshProUGUI _textValue;

        #region CallBacks

        private void Awake()
        {
            _textValue = GetComponentInChildren<TextMeshProUGUI>();
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

        private void UpdateHealthUI(float currentHealth)
        {
            if (_smoothHealthCoroutine != null) StopCoroutine(_smoothHealthCoroutine);
            _smoothHealthCoroutine = SmoothHealthCoroutine(currentHealth);
            StartCoroutine(_smoothHealthCoroutine);
        }

        private IEnumerator SmoothHealthCoroutine(float currentHealth)
        {
            var _lerpTime = 0.0f;
            var _sliderValue = healthSlider.value;
            while (_lerpTime < uISpeed)
            {
                var _newHealth = Mathf.Lerp(_sliderValue, currentHealth, _lerpTime/uISpeed);
                SetSliderValue(_newHealth);
                _lerpTime += Time.deltaTime;
                yield return null;
            }
            SetSliderValue(currentHealth);
        }

        private void SetSliderValue(float value)
        {
            healthSlider.value = value;
            _textValue.text = Mathf.Round(value).ToString();
        }

        private void SetMaxHealth()
        {
            healthSlider.maxValue = PlayerNetworkObject.MAX_HEALTH;
            healthSlider.value = PlayerNetworkObject.MAX_HEALTH;
        }

        #region Getters/Setters

        
        #endregion

        #region Events

        public Action<float> OnUpdateHealthUI { get; set; }
        
        #endregion
    }
}