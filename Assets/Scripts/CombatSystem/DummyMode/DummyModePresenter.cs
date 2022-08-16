using System;
using System.Collections.Generic;
using Card.QueueSystem;
using CardSystem.PlayerHand;
using Player;
using Scriptable_Objects;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UIElements;
using Zenject;

namespace CombatSystem.DummyMode
{
    public class DummyModePresenter : MonoBehaviour
    {
        private static SignalBus _signalBus;
        private static PlayerNetworkManager _playerNetworkManager;
        [Inject]
        public static void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
            _signalBus.Subscribe<PlayerNetworkInitializedSignal>(x => _playerNetworkManager = x.PlayerNetworkManager);
        }
        
        [SerializeField]
        private UIDocument uiDoc;
        
        private List<Toggle> _toggles;
        private List<Slider> _sliders;
        private Button _openOptionsButton;
        private VisualElement _optionsPanel;

        private DummyModeModel _myDummyModeModel;

        #region CallBacks

        private void Start()
        {
            _myDummyModeModel = GetComponent<DummyModeModel>();
        }

        private void OnEnable()
        {
            OnStartDummy += StartDummy;
            OnChangePlayerHealth += ChangePlayerHealthEvent;
            OnChangeEnemyHealth += ChangeEnemyHealthEvent;
            OnOpenOptions += OpenOptions;
            OnCloseOptions += CloseOptions;
            OnDisableOptionsBtn += DisableOptionsBtn;
        }

        private void OnDisable()
        {
            OnStartDummy -= StartDummy;
            OnChangePlayerHealth -= ChangePlayerHealthEvent;
            OnChangeEnemyHealth -= ChangeEnemyHealthEvent;
            OnOpenOptions -= OpenOptions;
            OnCloseOptions -= CloseOptions;
            OnDisableOptionsBtn -= DisableOptionsBtn;
        }
        
        #endregion

        private void StartDummy(DummyConfigSO dummyConfig)
        {
            var _root = uiDoc.rootVisualElement;
            _toggles = _root?.Query<Toggle>().ToList();
            _sliders = _root?.Query<Slider>().ToList();
            SetToggles();
            SetSliders();
            _openOptionsButton = _root?.Q<Button>("optionsButton");
            _optionsPanel = _root?.Q<VisualElement>("OptionsPanel");
            if (_openOptionsButton != null) _openOptionsButton.visible = true;
        }

        [Button]
        public void CallSetDefaultValues()
        {
            _myDummyModeModel.SetDefaultValues();
        }
        
        private void OpenOptions()
        {
            _optionsPanel.visible = true;
            _openOptionsButton.visible = false;
        }

        private void CloseOptions()
        {
            _optionsPanel.visible = false;
            _openOptionsButton.visible = true;
        }

        private void DisableOptionsBtn()
        {
            _openOptionsButton.visible = false;
        }
        
        private void SetToggles()
        {
            foreach (var _t in _toggles)
                _t.RegisterValueChangedCallback(_t.name switch
                {
                    "hpRegen" => HpRegenToggleCallback,
                    "ignoreApCosts" => IgnoreApCostsToggleCallback,
                    "enemyHpRegen" => EnemyHpRegenToggleCallback,
                    _ => DefaultBoolCallback
                });
        }

        private void SetSliders()
        {
            foreach (var _s in _sliders)
                _s.RegisterValueChangedCallback(_s.name switch
                {
                    "hpRegenSpeed" => HpRegenSpeedSliderCallback,
                    "apRegenSpeed" => ApRegenSpeedSliderCallback,
                    "enemyHpRegenSpeed" => EnemyHpRegenSpeedSliderCallback,
                    _ => DefaultFloatCallback
                });
        }
        
        private void HpRegenToggleCallback(ChangeEvent<bool> evt)
        {
            _myDummyModeModel.HpRegen = evt.newValue;
            ChangePlayerHealthEvent(evt.newValue);
        }
        
        private void EnemyHpRegenToggleCallback(ChangeEvent<bool> evt)
        {
            _myDummyModeModel.EnemyHpRegen = evt.newValue;
            ChangeEnemyHealthEvent(evt.newValue);
        }

        private void EnemyHpRegenSpeedSliderCallback(ChangeEvent<float> evt) =>
            _myDummyModeModel.EnemyHpRegenSpeed = evt.newValue;
        private void DefaultFloatCallback(ChangeEvent<float> evt) { }
        private void DefaultBoolCallback(ChangeEvent<bool> evt) { }
        private void HpRegenSpeedSliderCallback(ChangeEvent<float> evt) => _myDummyModeModel.HpRegenSpeed = evt.newValue;
        private void IgnoreApCostsToggleCallback(ChangeEvent<bool> evt) => _myDummyModeModel.IgnoreApCosts = evt.newValue;
        private void ApRegenSpeedSliderCallback(ChangeEvent<float> evt) => _myDummyModeModel.ApRegenSpeed = evt.newValue;
        
        private static void ChangePlayerHealthEvent(bool addEvent)
        {
            if (addEvent)
                _playerNetworkManager.GetLocalPlayer().MyPlayerHealthModel.HealthPresenter.OnUpdateHealthUI
                    += PlayerRecoverHpEvent;
            else
                _playerNetworkManager.GetLocalPlayer().MyPlayerHealthModel.HealthPresenter.OnUpdateHealthUI
                    -= PlayerRecoverHpEvent;
        }
        
        private static void ChangeEnemyHealthEvent(bool addEvent)
        {
            if (addEvent)
                _playerNetworkManager.GetEnemy().MyPlayerHealthModel.HealthPresenter.OnUpdateHealthUI
                    += EnemyRecoverHpEvent;
            else
                _playerNetworkManager.GetEnemy().MyPlayerHealthModel.HealthPresenter.OnUpdateHealthUI
                    -= EnemyRecoverHpEvent;
        }
        
        private static void PlayerRecoverHpEvent(float hp)
        {
            if (hp < PlayerNetworkEntityModel.MaxHealth)
                _playerNetworkManager.GetLocalPlayer().MyPlayerHealthModel.OnChangeHp.Invoke(PlayerNetworkEntityModel.MaxHealth);
        }
        
        private static void EnemyRecoverHpEvent(float hp)
        {
            if (hp < PlayerNetworkEntityModel.MaxHealth)
                _playerNetworkManager.GetEnemy().MyPlayerHealthModel.OnChangeHp.Invoke(PlayerNetworkEntityModel.MaxHealth);
        }

        #region Events

        public Action<DummyConfigSO> OnStartDummy { get; private set; }
        public Action<bool> OnChangePlayerHealth { get; private set; }
        public Action<bool> OnChangeEnemyHealth { get; private set; }
        public Action OnOpenOptions { get; private set; }
        public Action OnCloseOptions { get; private set; }
        public Action OnDisableOptionsBtn { get; set;}

        #endregion
    }
}