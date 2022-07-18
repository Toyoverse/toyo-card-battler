using System;
using UnityEngine;
using Sirenix.OdinInspector;

namespace CombatSystem.APSystem
{
    public class ApModel : MonoBehaviour, IApModel
    {
        private int AP;
        private int MaxAP;
        private float partialAP;
        private float timeForApRegen;
        private float currentTimerRegen;

        #region CallBacks

        private void Awake()
        {
            _myApPresenter = GetComponent<ApPresenter>();
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

        private void OnEnable()
        {
            OnGainAP += GainAP;
            OnUseAP += UseAP;
            OnChangeAP += ChangeAP;
        }

        private void OnDisable()
        {
            OnGainAP -= GainAP;
            OnUseAP -= UseAP;
            OnChangeAP -= ChangeAP;
        }

        #endregion

        private void GainAPRegen(float _value)
        {
            partialAP += _value;
            _myApPresenter?.OnUpdateAPUI?.Invoke(partialAP);

            if (Mathf.FloorToInt(partialAP) > AP)
                AP++;
              
        }

        private void ChangeAP(int _value)
        {
            if(_value > 0)
                GainAP(_value);
            else
                UseAP(_value);
        }
        
        private void GainAP(int _value)
        {
            AP += _value;
            partialAP = AP;
            _myApPresenter?.OnUpdateAPUI?.Invoke(AP);
        }

        private void UseAP(int _value)
        {
            AP -= _value;
            partialAP = AP;
            _myApPresenter?.OnUpdateAPUI?.Invoke(AP);
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

        #region Getters/Setters

        public int GetAP()
        {
            return AP;
        }
        
        int IApModel.GetAP()
        {
            return GetAP();
        }

        private IApPresenter _myApPresenter;
        IApPresenter IApModel.ApPresenter => _myApPresenter;

        #endregion

        #region Events

        public Action<int> OnGainAP { get; set; }
        public Action<int> OnChangeAP { get; set; }
        public Action<int> OnUseAP { get; set; }
        
        #endregion
    }
}