using Scriptable_Objects;
using UnityEngine;

namespace CombatSystem.DummyMode
{
    [RequireComponent(typeof(DummyModePresenter))]
    public class DummyModeModel : MonoBehaviour
    {
        public static bool IsDummyMode { get; private set; }
        [SerializeField]
        private DummyConfigSO defaultConfig;
        
        private bool _hpRegen;
        private bool _ignoreApCosts;
        private bool _enemyHpRegen;
        private float _hpRegenSpeed;
        private float _apRegenSpeed;
        private float _enemyHpRegenSpeed;

        private DummyModePresenter _myDummyModePresenter;

        #region CallBacks

        private void Awake()
        {
            _myDummyModePresenter = GetComponent<DummyModePresenter>();
        }
        
        private void OnEnable()
        {
            if (HpRegen)
                _myDummyModePresenter.OnChangePlayerHealth?.Invoke(true);
            if (EnemyHpRegen)
                _myDummyModePresenter.OnChangeEnemyHealth?.Invoke(true);
        }

        private void OnDisable()
        {
            if (HpRegen)
                _myDummyModePresenter.OnChangePlayerHealth?.Invoke(false);
            if (EnemyHpRegen)
                _myDummyModePresenter.OnChangeEnemyHealth?.Invoke(false);
        }
        
        #endregion

        private void StartDummy()
        {
            SetDefaultValues();
            _myDummyModePresenter.OnStartDummy?.Invoke(defaultConfig);
        }
        
        public void SetDefaultValues()
        {
            HpRegen = defaultConfig.hpRegen;
            HpRegenSpeed = defaultConfig.hpRegenSpeed;
            IgnoreApCosts = defaultConfig.ignoreApCosts;
            ApRegenSpeed = defaultConfig.apRegenSpeed;
            EnemyHpRegen = defaultConfig.enemyHpRegen;
            EnemyHpRegenSpeed = defaultConfig.enemyHpRegenSpeed;
        }

        private void DisableAllValues()
        {
            HpRegen = false;
            HpRegenSpeed = 0;
            IgnoreApCosts = false;
            ApRegenSpeed = 1; //TODO: Add default AP regen
            EnemyHpRegen = false;
            EnemyHpRegenSpeed = 0;
        }

        public void CallOpenOptions()
        {
            _myDummyModePresenter.OnOpenOptions?.Invoke();
        }

        public void CallCloseOptions()
        {
            _myDummyModePresenter.OnOpenOptions?.Invoke();

        }

        public void SetDummyMode(bool value)
        {
            IsDummyMode = value; 
            if (value)
                StartDummy();
            else
            {
                DisableAllValues();
                _myDummyModePresenter.OnDisableOptionsBtn?.Invoke();
            }
        }

        #region Getters/Setters

        public bool HpRegen
        {
            get => _hpRegen;
            set => _hpRegen = value;
        }

        public float HpRegenSpeed
        {
            get => _hpRegenSpeed;
            set => _hpRegenSpeed = value;
        }

        public bool EnemyHpRegen
        {
            get => _enemyHpRegen;
            set => _enemyHpRegen = value;
        }

        public float EnemyHpRegenSpeed
        {
            get => _enemyHpRegenSpeed;
            set => _enemyHpRegenSpeed = value;
        }
        
        public bool IgnoreApCosts
        {
            get => _ignoreApCosts;
            set => _ignoreApCosts = value;
        }

        public float ApRegenSpeed
        {
            get => _apRegenSpeed;
            set => _apRegenSpeed = value;
        }

        #endregion
    }
}
