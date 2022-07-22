﻿using System;
using System.Collections;
using Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Scriptable_Objects;
using Zenject;

namespace HealthSystem
{
    public class HealthPresenter : MonoBehaviour 
    {
        public Slider healthSlider;
        
        [Inject]
        private CombatConfigSO _combatConfig;
        private float _uISpeed;
        private IEnumerator _smoothHealthCoroutine;
        private TextMeshProUGUI _textValue;

        #region CallBacks

        private void Awake()
        {
            _textValue = GetComponentInChildren<TextMeshProUGUI>();
            _uISpeed = _combatConfig.healthUIFillSpeed;
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
            while (_lerpTime < _uISpeed)
            {
                var _newHealth = Mathf.Lerp(_sliderValue, currentHealth, _lerpTime/_uISpeed);
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
            healthSlider.maxValue = PlayerNetworkEntityModel.MaxHealth;
            healthSlider.value = PlayerNetworkEntityModel.MaxHealth;
        }

        #region Getters/Setters

        
        #endregion

        #region Events

        public Action<float> OnUpdateHealthUI { get; set; }
        
        #endregion
    }
}