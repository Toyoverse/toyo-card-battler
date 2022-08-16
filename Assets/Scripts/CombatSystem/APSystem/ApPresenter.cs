using System;
using ServiceLocator;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CombatSystem.APSystem
{
    public class ApPresenter : MonoBehaviour 
    {
        public Slider apSlider;
        
        private TextMeshProUGUI _textValue;

        #region CallBacks

        private void Awake()
        {
            _textValue = GetComponentInChildren<TextMeshProUGUI>();
        }

        private void OnEnable()
        {
            OnUpdateApUI += UpdateApUI;
            SetMaxAP(Locator.GetGlobalConfig().maxAP);
        }

        private void OnDisable()
        {
            OnUpdateApUI -= UpdateApUI;
        }
        
        #endregion

        private void UpdateApUI(float currentAP) => SetSliderValue(currentAP);
        
        private void SetSliderValue(float value)
        {
            apSlider.value = value;
            _textValue.text = Mathf.Floor(value).ToString();
        }
        
        private void SetMaxAP(float maxAp)
        {
            apSlider.maxValue = maxAp;
            apSlider.value = maxAp;
        }

        #region Getters/Setters

        
        #endregion

        #region Events

        public Action<float> OnUpdateApUI { get; set; }

        #endregion
    }
}