using System;
using Player;
using UnityEngine;
using Sirenix.OdinInspector;

namespace HealthSystem
{
    public class HealthModel : MonoBehaviour, IHealthModel
    {
        public PlayerNetworkObject Parent { get; set; }

        private IHealthPresenter _myHealthPresenter;
        IHealthPresenter IHealthModel.HealthPresenter => _myHealthPresenter;

        #region CallBacks

        private void Awake()
        {
            _myHealthPresenter = GetComponent<HealthPresenter>();
        }

        private void OnEnable()
        {
            OnGainHP += GainHP;
            OnTakeDamage += TakeDamage;
            OnChangeHP += ChangeHP;
            OnInitialize += Initialize;
        }

        private void OnDisable()
        {
            OnGainHP -= GainHP;
            OnTakeDamage -= TakeDamage;
            OnChangeHP -= ChangeHP;
            OnInitialize -= Initialize;
        }

        #endregion

        private void Initialize(float _value)
        {
            OnGainHP?.Invoke(_value);
        }

        private void ChangeHP(float _value)
        {
            var _temp = GetHealth() - _value;
            if (_temp > 0)
                OnGainHP?.Invoke(_temp);
            else
                OnTakeDamage?.Invoke(_temp);
        }

        private void GainHP(float _value)
        {
            var _health = GetHealth() + _value;
            if (_health > PlayerNetworkObject.MAX_HEALTH)
                _health = PlayerNetworkObject.MAX_HEALTH;
            Parent.Health = _health;
            _myHealthPresenter?.OnUpdateHealthUI?.Invoke(_health);
        }

        private void TakeDamage(float _value)
        {
            var _health = GetHealth() - _value;
            if (_health < 0)
                _health = 0;
            Parent.Health = _health;
            _myHealthPresenter?.OnUpdateHealthUI?.Invoke(_health);
        }

        #region Getters/Setters

        public float GetHealth()
        {
            return Parent.Health;
        }

        #endregion

        #region Events

        public Action<float> OnGainHP { get; set; }
        public Action<float> OnChangeHP { get; set; }
        public Action<float> OnTakeDamage { get; set; }
        public Action<float> OnInitialize { get; set; }

        #endregion
    }
}