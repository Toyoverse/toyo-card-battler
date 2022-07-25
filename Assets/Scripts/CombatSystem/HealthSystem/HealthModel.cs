using System;
using Player;
using UnityEngine;

namespace HealthSystem
{
    [RequireComponent(typeof(HealthPresenter))]
    public class HealthModel : MonoBehaviour
    {
        private PlayerNetworkEntityModel _parent;
        private HealthPresenter _myHealthPresenter;

        #region CallBacks

        private void Awake()
        {
            _myHealthPresenter = GetComponent<HealthPresenter>();
        }

        public void InjectNetwork(PlayerNetworkEntityModel network)
        {
            _parent = network;
        }

        private void OnEnable()
        {
            OnGainHp += GainHp;
            OnTakeDamage += TakeDamage;
            OnChangeHp += ChangeHp;
        }

        private void OnDisable()
        {
            OnGainHp -= GainHp;
            OnTakeDamage -= TakeDamage;
            OnChangeHp -= ChangeHp;
        }

        #endregion

        private void ChangeHp(float value)
        {
            var _temp = Parent.Health - value;
            if (_temp > 0)
                OnGainHp?.Invoke(_temp);
            else
                OnTakeDamage?.Invoke(_temp);
        }

        private void GainHp(float value)
        {
            var _health = Parent.Health + value;
            if (_health > PlayerNetworkEntityModel.MaxHealth)
                _health = PlayerNetworkEntityModel.MaxHealth;
            Parent.Health = _health;
            _myHealthPresenter.OnUpdateHealthUI?.Invoke(_health);
        }

        private void TakeDamage(float value)
        {
            var _health = Parent.Health - value;
            if (_health < 0)
                _health = 0;
            Parent.Health = _health;
            _myHealthPresenter.OnUpdateHealthUI?.Invoke(_health);
        }

        #region Getters/Setters
        
        public HealthPresenter HealthPresenter => _myHealthPresenter;
        
        public PlayerNetworkEntityModel Parent
        {
            get => _parent;
            set => _parent = value;
        }

        #endregion

        #region Events

        public Action<float> OnGainHp { get; set; }
        public Action<float> OnChangeHp { get; set; }
        public Action<float> OnTakeDamage { get; set; }

        #endregion
    }
}