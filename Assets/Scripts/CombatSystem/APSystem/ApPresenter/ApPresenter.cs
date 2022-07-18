using System;
using TMPro;
using Tools;
using UnityEngine;
using UnityEngine.UI;

namespace CombatSystem.APSystem
{
    public class ApPresenter : MonoBehaviour, IApPresenter 
    {
        public SpriteRenderer MyRenderer { get; set;}
        public IMouseInput MyInput { get; set;}
        public Transform MyTransform { get; set; }
        private TextMeshProUGUI textValue;

        public Slider APSlider;

        void Awake()
        {
            MyTransform = transform;
            MyRenderer = GetComponent<SpriteRenderer>();
            MyInput = GetComponent<IMouseInput>();
            textValue = GetComponentInChildren<TextMeshProUGUI>();
        }

        void OnEnable()
        {
            OnUpdateAPUI += UpdateAPUI;
            SetMaxAP(GlobalConfig.Instance.maxAP);
        }

        void OnDisable()
        {
            OnUpdateAPUI -= UpdateAPUI;
        }

        void UpdateAPUI(float _currentAP) => SetSliderValue(_currentAP);
        
        void SetSliderValue(float _value)
        {
            APSlider.value = _value;
            textValue.text = Mathf.Floor(_value).ToString();
        }
        
        void SetMaxAP(float _maxAp)
        {
            APSlider.maxValue = _maxAp;
            APSlider.value = _maxAp;
        }

        public Camera MainCamera => Camera.main;
        public MonoBehaviour MonoBehaviour => this;
        public Action<float> OnUpdateAPUI { get; set; }

        SpriteRenderer IApPresenter.Renderer => MyRenderer;
        IMouseInput IApPresenter.Input => MyInput;
        
    }
}