using System;
using TMPro;
using Tools;
using UnityEngine;
using UnityEngine.UI;

namespace CombatSystem.APSystem
{
    public class ApPresenter : MonoBehaviour, IApPresenter 
    {
        private TextMeshProUGUI _textValue;
        
        public SpriteRenderer MyRenderer { get; set;}
        public IMouseInput MyInput { get; set;}
        public Transform MyTransform { get; set; }
        public Slider APSlider;

        #region CallBacks

        private void Awake()
        {
            MyTransform = transform;
            MyRenderer = GetComponent<SpriteRenderer>();
            MyInput = GetComponent<IMouseInput>();
            _textValue = GetComponentInChildren<TextMeshProUGUI>();
        }

        private void OnEnable()
        {
            OnUpdateAPUI += UpdateAPUI;
            SetMaxAP(GlobalConfig.Instance.maxAP);
        }

        private void OnDisable()
        {
            OnUpdateAPUI -= UpdateAPUI;
        }
        
        #endregion

        void UpdateAPUI(float currentAP) => SetSliderValue(currentAP);
        
        private void SetSliderValue(float value)
        {
            APSlider.value = value;
            _textValue.text = Mathf.Floor(value).ToString();
        }
        
        private void SetMaxAP(float maxAp)
        {
            APSlider.maxValue = maxAp;
            APSlider.value = maxAp;
        }

        public Camera MainCamera => Camera.main;
        public MonoBehaviour MonoBehaviour => this;

        SpriteRenderer IApPresenter.Renderer => MyRenderer;
        IMouseInput IApPresenter.Input => MyInput;

        #region Events

        public Action<float> OnUpdateAPUI { get; set; }

        #endregion
    }
}