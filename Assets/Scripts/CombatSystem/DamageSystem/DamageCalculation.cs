using System;
using Card;
using ToyoSystem;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

namespace CombatSystem.DamageSystem
{
    public class DamageCalculation
    {
        public static void CalculateDamage(DamageInformation dmgInfo)
        {
            var _attackType = dmgInfo.AttackType;
            var _damage = _attackType == ATTACK_TYPE.PHYSICAL
                ? CalculateStrenght(dmgInfo)
                : CalculateCyberForce(dmgInfo);

            float _damageFromType = dmgInfo.CardType switch
            {
                CARD_TYPE.HEAVY => ApplyDamageCalculation(dmgInfo, _damage),
                CARD_TYPE.FAST => ApplyDamageCalculation(dmgInfo, _damage),
                CARD_TYPE.SUPER => ApplyDamageCalculation(dmgInfo, _damage),
                CARD_TYPE.BOND => CalculateBondCard(dmgInfo),
                _ => 0
            };

            if (dmgInfo.CardType == CARD_TYPE.DEFENSE)
            {
                var success = DefenseSuccess(dmgInfo);
                var counter = success && CounterSuccess(dmgInfo);
            }
        }

        static float CalculateBondCard(DamageInformation dmgInfo)
        {
            return GlobalConfig.Instance.globalCardDataSO.bondCardDamage;
        }
        
        static float CalculateStrenght(DamageInformation dmgInfo)
        {
            var _strenght = dmgInfo.ToyoStats[TOYO_STAT.PHYSICAL_STRENGHT];
            var _hitVariation = dmgInfo.HitVariation;
            return _strenght * _hitVariation;
        }
        
        static float CalculateCyberForce(DamageInformation dmgInfo)
        {
            var _cyberForce = dmgInfo.ToyoStats[TOYO_STAT.CYBER_FORCE];
            var _hitVariation = dmgInfo.HitVariation;
            return _cyberForce * _hitVariation;
        }

        /// <summary>
        /// Returns damage dealt after applying modifiers.
        /// </summary>
        static float ApplyDamageCalculation(DamageInformation dmgInfo, float damage)
        {
            float _sum = 0;
            float _multiplier = 1;
            GetSumAndMultiplier(dmgInfo.CardType, ref _sum, ref _multiplier);
            
            var _comboFactor = GetComboFactor(dmgInfo);
            
            var _criticalHit = CheckCriticalHit(dmgInfo);
            
            var _enemyDef = GetEnemyDef(dmgInfo, _criticalHit);

            var _hitVar = GetHitVariation(dmgInfo, _criticalHit);
            
            var _damageResult = _multiplier * (_sum + _comboFactor * _hitVar * damage);
            _damageResult -= _enemyDef;
            
            return _damageResult;
        }
        
        static void GetSumAndMultiplier(CARD_TYPE type, ref float _sum, ref float _multiplier)
        {
            switch (type)
            {
                case CARD_TYPE.FAST:
                    _sum = GlobalConfig.Instance.globalCardDataSO.fastCardSumFactor;
                    _multiplier = GlobalConfig.Instance.globalCardDataSO.fastCardMultiplierFactor;
                    break;
                case CARD_TYPE.HEAVY:
                    _sum = GlobalConfig.Instance.globalCardDataSO.heavyCardSumFactor;
                    _multiplier = GlobalConfig.Instance.globalCardDataSO.heavyCardMultiplierFactor;
                    break;
                case CARD_TYPE.SUPER:
                    _sum = GlobalConfig.Instance.globalCardDataSO.superCardSumFactor;
                    _multiplier = GlobalConfig.Instance.globalCardDataSO.superCardMultiplierFactor;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        static int GetComboFactor(DamageInformation dmgInfo)
        {
            var _comboFactor = 1;
            if (dmgInfo.CardType == CARD_TYPE.HEAVY)
            {
                _comboFactor = GlobalConfig.Instance.globalCardDataSO.comboSystemFactor;
            }
            var comboFactor = dmgInfo.CurrentCombo / _comboFactor;
            return comboFactor;
        }
        
        static bool CheckCriticalHit(DamageInformation dmgInfo)
        {   
            var _precision = dmgInfo.ToyoStats[TOYO_STAT.PRECISION];
            var _luck = dmgInfo.ToyoStats[TOYO_STAT.LUCK];

            var _criticalChance =
                (_precision * GlobalConfig.Instance.globalCardDataSO.criticalPrecisionFactor)
                + (_luck * GlobalConfig.Instance.globalCardDataSO.criticalLuckFactor);

            var _maxCritical = GlobalConfig.Instance.globalCardDataSO.maxCriticalChance;
            _criticalChance = _criticalChance > _maxCritical ? _maxCritical : _criticalChance;
            return Random.Range(0, 100) >= _criticalChance;
        }

        static float GetEnemyDef(DamageInformation dmgInfo, bool criticalHit)
        {
            var _enemyDef = dmgInfo.AttackType == ATTACK_TYPE.PHYSICAL ?
                (dmgInfo.EnemyToyoStats[TOYO_STAT.RESISTANCE] * 
                 GlobalConfig.Instance.globalCardDataSO.enemyResistanceMultiplier) : 
                (dmgInfo.EnemyToyoStats[TOYO_STAT.RESILIENCE] *
                 GlobalConfig.Instance.globalCardDataSO.enemyResilienceMultiplier);
            if (criticalHit)
            {
                _enemyDef *= GlobalConfig.Instance.globalCardDataSO.defenseInCriticalMultiplier;
            }

            return _enemyDef;
        }

        static float GetHitVariation(DamageInformation dmgInfo, bool criticalHit)
        {
            return criticalHit ? 
                dmgInfo.HitVariation * GlobalConfig.Instance.globalCardDataSO.criticalDamageModifier
                : dmgInfo.HitVariation;
        }

        static float GetDefenseChance(DamageInformation dmgInfo)
        {
            float _defChance = 100;
            switch (dmgInfo.DefenseType)
            {
                case DEFENSE_TYPE.BLOCK:
                    _defChance += ((dmgInfo.ToyoStats[TOYO_STAT.TECHNIQUE]
                                  * GlobalConfig.Instance.globalCardDataSO.techDefMultiplier)
                                  - (dmgInfo.EnemyToyoStats[TOYO_STAT.ANALYSIS] 
                                  * GlobalConfig.Instance.globalCardDataSO.analysisMultiplier));
                    break;
                case  DEFENSE_TYPE.DODGE:
                    _defChance += ((dmgInfo.ToyoStats[TOYO_STAT.AGILITY]
                                  * GlobalConfig.Instance.globalCardDataSO.agilityDefMultiplier)
                                  - (dmgInfo.EnemyToyoStats[TOYO_STAT.SPEED] 
                                  * GlobalConfig.Instance.globalCardDataSO.speedMultiplier));
                    break;
            }

            return _defChance;
        }

        static bool DefenseSuccess(DamageInformation dmgInfo)
        {
            float defenseChance = GetDefenseChance(dmgInfo);
            return Random.Range(0, 100) <= defenseChance;
        }
        
        static float GetCounterChance(DamageInformation dmgInfo)
        {
            float _counterChance = GlobalConfig.Instance.globalCardDataSO.baseCounterChance;
            switch (dmgInfo.DefenseType)
            {
                case DEFENSE_TYPE.BLOCK:
                    _counterChance += ((dmgInfo.ToyoStats[TOYO_STAT.TECHNIQUE] * 
                                      GlobalConfig.Instance.globalCardDataSO.counterTechMultiplier) + 
                                      (dmgInfo.ToyoStats[TOYO_STAT.LUCK] * 
                                      GlobalConfig.Instance.globalCardDataSO.counterLuckFactor));
                    break;
                case  DEFENSE_TYPE.DODGE:
                    _counterChance += ((dmgInfo.ToyoStats[TOYO_STAT.AGILITY] 
                                      * GlobalConfig.Instance.globalCardDataSO.counterAgilityMultiplier) 
                                      + (dmgInfo.ToyoStats[TOYO_STAT.LUCK] 
                                      * GlobalConfig.Instance.globalCardDataSO.counterLuckFactor));
                    break;
            }

            return _counterChance;
        }
        
        static bool CounterSuccess(DamageInformation dmgInfo)
        {
            float counterChance = GetCounterChance(dmgInfo);
            return Random.Range(0, 100) <= counterChance;
        }
    }
}