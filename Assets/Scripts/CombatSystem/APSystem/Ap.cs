using System;
using APSystem;
using APSystem.ApUI;
using Tools;
using UnityEngine;

namespace APSystem
{
    [UseAttributes]
    public class Ap : MonoBehaviour, IAp
    {
        private int AP;
        private int MaxAP;
        private float partialAP;
        private float timeForApRegen;
        private float currentTimerRegen;

        #region GetterAndSetter

        public int GetAP()
        {
            return AP;
        }
        
        int IAp.GetAP()
        {
            return GetAP();
        }

        private IApUI MyApUI;
        IApUI IAp.ApUI => MyApUI;

        #endregion

        #region CallBacks

        void Awake()
        {
            MyApUI = GetComponent<ApUI.ApUI>();
            timeForApRegen = GlobalConfig.Instance.timeForApRegen;
            partialAP = MaxAP = AP = GlobalConfig.Instance.maxAP;
        }

        private void Update()
        {
            if (currentTimerRegen <= timeForApRegen && AP < MaxAP)
            {
                currentTimerRegen += Time.deltaTime;
                var _value = Time.deltaTime / timeForApRegen;
                GainAPRegen(_value);
            }
            else
            {
                currentTimerRegen = 0.0f;
            }
        }

        void OnEnable()
        {
            OnGainAP += GainAP;
            OnUseAP += UseAP;
            OnChangeAP += ChangeAP;
        }

        void OnDisable()
        {
            OnGainAP -= GainAP;
            OnUseAP -= UseAP;
            OnChangeAP -= ChangeAP;
        }

        #endregion

        void GainAPRegen(float _value)
        {
            partialAP += _value;
            MyApUI?.OnUpdateAPUI?.Invoke(partialAP);

            if (Mathf.FloorToInt(partialAP) > AP)
                AP++;
              
        }

        void ChangeAP(int _value)
        {
            if(_value > 0)
                GainAP(_value);
            else
                UseAP(_value);
        }
        
        void GainAP(int _value)
        {
            AP += _value;
            partialAP = AP;
            MyApUI?.OnUpdateAPUI?.Invoke(AP);
        }

        void UseAP(int _value)
        {
            AP -= _value;
            partialAP = AP;
            MyApUI?.OnUpdateAPUI?.Invoke(AP);
        }

        public int testHPValue = 1;

        [Button]
        public void TestDamage()
        {
            UseAP(testHPValue);
        }

        [Button]
        public void TestHealing()
        {
            GainAP(testHPValue);
        }

        public Action<int> OnGainAP { get; set; }
        public Action<int> OnChangeAP { get; set; }
        public Action<int> OnUseAP { get; set; }
    }
}