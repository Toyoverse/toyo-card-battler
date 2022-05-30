using System.Collections.Generic;
using Player;
using Scriptable_Objects;
using Tools;
using UnityEngine;
using UnityEngine.UIElements;
using Button = UnityEngine.UIElements.Button;

namespace CombatSystem
{
    [UseAttributes]
    public class DummyMode : MonoBehaviour
    {
        public static bool IsDummyMode { get; private set; }

        public DummyConfigSO defaultConfig;
        public UIDocument UiDoc;
        private List<Toggle> toggles;
        private List<Slider> sliders;
        private Button openOptionsButton;
        private VisualElement optionsPanel;

        [Header("Player")] private bool hpRegen;
        private bool ignoreApCosts;
        private float hpRegenSpeed;
        private float apRegenSpeed;

        [Header("Enemy")]
        private bool enemyHpRegen;
        private float enemyHpRegenSpeed;

        public bool IsHpRegen => hpRegen;
        public float GetHpRegenSpeed => hpRegenSpeed;
        public bool IsIgnoreApCosts => ignoreApCosts;
        public float GetApRegenSpeed => apRegenSpeed;
        public bool IsEnemyHpRegen => enemyHpRegen;
        public float GetEnemyHpRegenSpeed => enemyHpRegenSpeed;

        private void HpRegenToggleCallback(ChangeEvent<bool> evt)
        {
            hpRegen = evt.newValue;
            ChangePlayerHealthEvent(hpRegen);
            if (hpRegen)
                PlayerNetworkManager.GetLocalPlayer().MyPlayerHealth.OnChangeHP.Invoke(PlayerNetworkObject.MAX_HEALTH);
        }

        private void HpRegenSpeedSliderCallback(ChangeEvent<float> evt) => hpRegenSpeed = evt.newValue;
        private void IgnoreApCostsToggleCallback(ChangeEvent<bool> evt) => ignoreApCosts = evt.newValue;
        private void ApRegenSpeedSliderCallback(ChangeEvent<float> evt) => apRegenSpeed = evt.newValue;

        private void EnemyHpRegenToggleCallback(ChangeEvent<bool> evt)
        {
            enemyHpRegen = evt.newValue;
            ChangeEnemyHealthEvent(enemyHpRegen);
            if (enemyHpRegen)
                PlayerNetworkManager.GetEnemy().MyPlayerHealth.OnChangeHP.Invoke(PlayerNetworkObject.MAX_HEALTH);
        }

        private void EnemyHpRegenSpeedSliderCallback(ChangeEvent<float> evt) => enemyHpRegenSpeed = evt.newValue;

        private void DefaultFloatCallback(ChangeEvent<float> evt) { }
        private void DefaultBoolCallback(ChangeEvent<bool> evt) { }

        private void StartDummy()
        {
            SetDefaultValues();
            var root = UiDoc.rootVisualElement;
            toggles = root?.Query<Toggle>().ToList();
            sliders = root?.Query<Slider>().ToList();
            SetToggles();
            SetSliders();
            openOptionsButton = root?.Q<Button>("optionsButton");
            optionsPanel = root?.Q<VisualElement>("OptionsPanel");
            if (openOptionsButton != null) openOptionsButton.visible = true;
        }

        private void SetToggles()
        {
            foreach (var t in toggles)
                t.RegisterValueChangedCallback(t.name switch
                {
                    "hpRegen" => HpRegenToggleCallback,
                    "ignoreApCosts" => IgnoreApCostsToggleCallback,
                    "enemyHpRegen" => EnemyHpRegenToggleCallback,
                    _ => DefaultBoolCallback
                });
        }

        private void SetSliders()
        {
            foreach (var s in sliders)
                s.RegisterValueChangedCallback(s.name switch
                {
                    "hpRegenSpeed" => HpRegenSpeedSliderCallback,
                    "apRegenSpeed" => ApRegenSpeedSliderCallback,
                    "enemyHpRegenSpeed" => EnemyHpRegenSpeedSliderCallback,
                    _ => DefaultFloatCallback
                });
        }

        [Tools.Button]
        public void SetDefaultValues()
        {
            this.hpRegen = defaultConfig.hpRegen;
            this.hpRegenSpeed = defaultConfig.hpRegenSpeed;
            this.ignoreApCosts = defaultConfig.ignoreApCosts;
            this.apRegenSpeed = defaultConfig.apRegenSpeed;
            this.enemyHpRegen = defaultConfig.enemyHpRegen;
            this.enemyHpRegenSpeed = defaultConfig.enemyHpRegenSpeed;
        }

        private void OnEnable()
        {
            if (hpRegen)
                ChangePlayerHealthEvent(true);
            if (enemyHpRegen)
                ChangeEnemyHealthEvent(true);
        }

        private void OnDisable()
        {
            if (hpRegen)
                ChangePlayerHealthEvent(false);
            if (enemyHpRegen)
                ChangeEnemyHealthEvent(false);
        }

        private void DisableAllValues()
        {
            this.hpRegen = false;
            this.hpRegenSpeed = 0;
            this.ignoreApCosts = false;
            this.apRegenSpeed = 1; //TODO: Add default AP regen
            this.enemyHpRegen = false;
            this.enemyHpRegenSpeed = 0;
        }

        public void OpenOptions(bool open)
        {
            optionsPanel.visible = open;
            openOptionsButton.visible = !open;
        }

        public void SetDummyMode(bool on)
        {
            IsDummyMode = on;
            if (on)
                StartDummy();
            else
            {
                DisableAllValues();
                openOptionsButton.visible = false;
            }
        }

        private static void ChangePlayerHealthEvent(bool addEvent)
        {
            if (addEvent)
                PlayerNetworkManager.GetLocalPlayer().MyPlayerHealth.HealthUI.OnUpdateHealthUI
                    += PlayerRecoverHPEvent;
            else
                PlayerNetworkManager.GetLocalPlayer().MyPlayerHealth.HealthUI.OnUpdateHealthUI
                    -= PlayerRecoverHPEvent;
        }

        private static void ChangeEnemyHealthEvent(bool addEvent)
        {
            if (addEvent)
                PlayerNetworkManager.GetEnemy().MyPlayerHealth.HealthUI.OnUpdateHealthUI
                    += EnemyRecoverHPEvent;
            else
                PlayerNetworkManager.GetEnemy().MyPlayerHealth.HealthUI.OnUpdateHealthUI
                    -= EnemyRecoverHPEvent;
        }

        private static void EnemyRecoverHPEvent(float hp)
        {
            if (hp < PlayerNetworkObject.MAX_HEALTH)
                PlayerNetworkManager.GetEnemy().MyPlayerHealth.OnChangeHP.Invoke(PlayerNetworkObject.MAX_HEALTH);
        }

        private static void PlayerRecoverHPEvent(float hp)
        {
            if (hp < PlayerNetworkObject.MAX_HEALTH)
                PlayerNetworkManager.GetLocalPlayer().MyPlayerHealth.OnChangeHP.Invoke(PlayerNetworkObject.MAX_HEALTH);
        }
    }
}
