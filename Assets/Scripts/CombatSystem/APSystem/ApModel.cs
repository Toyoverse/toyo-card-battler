using System;
using UnityEngine;

namespace CombatSystem.APSystem
{
    [RequireComponent(typeof(ApPresenter))]
    public class ApModel : MonoBehaviour
    {
        private int _ap;
        private int _maxAP;
        private float _partialAP;
        private float _timeForApRegen;
        private float _currentTimerRegen;
        private ApPresenter _myApPresenter;

        #region CallBacks

        private void Awake()
        {
            _myApPresenter = GetComponent<ApPresenter>();
            _timeForApRegen = GlobalConfig.Instance.timeForApRegen;
            _partialAP = _maxAP = Ap = GlobalConfig.Instance.maxAP;
        }

        private void Update()
        {
            if (_currentTimerRegen <= _timeForApRegen && Ap < _maxAP)
            {
                _currentTimerRegen += Time.deltaTime;
                var _value = Time.deltaTime / _timeForApRegen;
                GainAPRegen(_value);
            }
            else
            {
                _currentTimerRegen = 0.0f;
            }
        }

        private void OnEnable()
        {
            OnGainAp += GainAP;
            OnUseAp += UseAP;
            OnChangeAp += ChangeAP;
        }

        private void OnDisable()
        {
            OnGainAp -= GainAP;
            OnUseAp -= UseAP;
            OnChangeAp -= ChangeAP;
        }

        #endregion

        private void GainAPRegen(float value)
        {
            _partialAP += value;
            _myApPresenter.OnUpdateApUI?.Invoke(_partialAP);

            if (Mathf.FloorToInt(_partialAP) > Ap)
                Ap++;
              
        }

        private void ChangeAP(int value)
        {
            if(value > 0)
                GainAP(value);
            else
                UseAP(value);
        }
        
        private void GainAP(int value)
        {
            Ap += value;
            _partialAP = Ap;
            _myApPresenter.OnUpdateApUI?.Invoke(Ap);
        }

        private void UseAP(int value)
        {
            Ap -= value;
            _partialAP = Ap;
            _myApPresenter.OnUpdateApUI?.Invoke(Ap);
        }

        #region Getters/Setters

        public int Ap
        {
            get => _ap;
            set => _ap = value;
        }

        #endregion

        #region Events

        public Action<int> OnGainAp { get; set; }
        public Action<int> OnChangeAp { get; set; }
        public Action<int> OnUseAp { get; set; }
        
        #endregion
    }
}