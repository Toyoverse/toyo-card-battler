using Scriptable_Objects;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CombatSystem.DummyMode
{
    public class DummyModeModel : MonoBehaviour
    {
        public static bool IsDummyMode { get; private set; }
        
        [Header("Player")]
        private bool _hpRegen;
        private bool _ignoreApCosts;
        private float _hpRegenSpeed;
        private float _apRegenSpeed;
        [Header("Enemy")]
        private bool _enemyHpRegen;
        private float _enemyHpRegenSpeed;

        [SerializeField]
        private DummyConfigSO defaultConfig;
        private DummyModePresenter _myDummyModePresenter;

        #region CallBacks

        private void Start()
        {
            _myDummyModePresenter = GetComponent<DummyModePresenter>();
        }
        
        private void OnEnable()
        {
            if (_hpRegen)
                _myDummyModePresenter.OnChangePlayerHealth?.Invoke(true);
            if (_enemyHpRegen)
                _myDummyModePresenter.OnChangeEnemyHealth?.Invoke(true);
        }

        private void OnDisable()
        {
            if (_hpRegen)
                _myDummyModePresenter.OnChangePlayerHealth?.Invoke(false);
            if (_enemyHpRegen)
                _myDummyModePresenter.OnChangeEnemyHealth?.Invoke(false);
        }
        
        #endregion

        private void StartDummy()
        {
            SetDefaultValues();
            _myDummyModePresenter.OnStartDummy?.Invoke(defaultConfig);
        }
        
        [Button]
        private void SetDefaultValues()
        {
            _hpRegen = defaultConfig.hpRegen;
            _hpRegenSpeed = defaultConfig.hpRegenSpeed;
            _ignoreApCosts = defaultConfig.ignoreApCosts;
            _apRegenSpeed = defaultConfig.apRegenSpeed;
            _enemyHpRegen = defaultConfig.enemyHpRegen;
            _enemyHpRegenSpeed = defaultConfig.enemyHpRegenSpeed;
        }

        private void DisableAllValues()
        {
            _hpRegen = false;
            _hpRegenSpeed = 0;
            _ignoreApCosts = false;
            _apRegenSpeed = 1; //TODO: Add default AP regen
            _enemyHpRegen = false;
            _enemyHpRegenSpeed = 0;
        }

        public void CallOpenOrCloseOptions(bool open)
        {
            _myDummyModePresenter.OnOpenOptions?.Invoke(open);
        }

        public void SetDummyMode(bool on)
        {
            IsDummyMode = on; //CHECK WITH JHONE THE REASON OF THIS
            if (on)
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
