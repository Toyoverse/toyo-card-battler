using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CombatSystem
{
    public class DamageCalculation
    {
        public static float[] CalculateDamage(DamageInformation dmgInfo)
        {
            var _attackType = dmgInfo.AttackType;
            var _damage = _attackType == ATTACK_TYPE.PHYSICAL
                ? CalculateStrength(dmgInfo)
                : CalculateCyberForce(dmgInfo);

            var _result = new float[dmgInfo.HitVariation.Count];
            for (var i = 0; i < _damage.Length; i++)
            {
                _result[i] = ApplyDamageCalculation(dmgInfo, _damage[i], i);
            }

            return _result;
        }

        private static float[] CalculateStrength(DamageInformation dmgInfo)
        {   
            //TODO: Consider balancing the calculation to avoid absurd values.
            float[] _strength = new float[dmgInfo.HitVariation.Count];
            for (var i = 0; i < dmgInfo.HitVariation.Count; i++)
            {
                _strength[i] = dmgInfo.ToyoStats[TOYO_STAT.PHYSICAL_STRENGTH];
                _strength[i] *= BoundSystem.GetFactorInMyBuffs(dmgInfo, TOYO_STAT.PHYSICAL_STRENGTH);
                _strength[i] *= dmgInfo.HitVariation[i].Damage;
            }

            return _strength;

            /*var _strength = dmgInfo.ToyoStats[TOYO_STAT.PHYSICAL_STRENGTH];
            _strength *= BoundSystem.GetFactorInMyBuffs(dmgInfo, TOYO_STAT.PHYSICAL_STRENGTH);
            var _hitVariation = dmgInfo.HitVariation;
            return _strength * _hitVariation;*/
        }

        private static float[] CalculateCyberForce(DamageInformation dmgInfo)
        {
            //TODO: Consider balancing the calculation to avoid absurd values.
            float[] _cyberForce = new float[dmgInfo.HitVariation.Count];
            for (var i = 0; i < dmgInfo.HitVariation.Count; i++)
            {
                _cyberForce[i] = dmgInfo.ToyoStats[TOYO_STAT.CYBER_FORCE];
                _cyberForce[i] *= BoundSystem.GetFactorInMyBuffs(dmgInfo, TOYO_STAT.CYBER_FORCE);
                _cyberForce[i] *= dmgInfo.HitVariation[i].Damage;
            }

            return _cyberForce;
            
            /*var _cyberForce = dmgInfo.ToyoStats[TOYO_STAT.CYBER_FORCE];
            _cyberForce *= BoundSystem.GetFactorInMyBuffs(dmgInfo, TOYO_STAT.CYBER_FORCE);
            var _hitVariation = dmgInfo.HitVariation;
            return _cyberForce * _hitVariation;*/
        }

        /// <summary>
        /// Returns damage dealt after applying modifiers.
        /// </summary>
        private static float ApplyDamageCalculation(DamageInformation dmgInfo, float damage, int hitIndex)
        {
            float _sum = 0;
            float _multiplier = 1;
            GetSumAndMultiplier(dmgInfo.CardType, ref _sum, ref _multiplier);
            Debug.Log("AttackSum: " + _sum + " | AttackMultiplier: " + _multiplier);
            
            var _comboFactor = GetComboFactor(dmgInfo);
            Debug.Log("ComboFactor: " + _comboFactor);
            
            var _criticalHit = CheckCriticalHit(dmgInfo);
            Debug.Log("Critical: " + _criticalHit);
            
            var _enemyDef = GetEnemyDef(dmgInfo, _criticalHit);
            Debug.Log("EnemyDef: " + _enemyDef);

            var _hitVar = GetHitVariation(dmgInfo, _criticalHit, hitIndex);
            Debug.Log("HitVariation: " + _hitVar);
            
            Debug.Log("BaseDamage: " + damage);
            var _damageResult = _multiplier * ((_sum + _comboFactor) * _hitVar * damage);
            var _maxDefFactor = GlobalConfig.Instance.globalCardDataSO.maxDefenseFactor;
            _enemyDef = _enemyDef > (_damageResult / _maxDefFactor) ? 
                (_damageResult / _maxDefFactor) : _enemyDef;
            _damageResult -= _enemyDef;
            Debug.Log("DamageCalcResult: " + _damageResult);

            return _damageResult;
        }

        private static void GetSumAndMultiplier(CARD_TYPE type, ref float _sum, ref float _multiplier)
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
                case CARD_TYPE.DEFENSE:
                case CARD_TYPE.BOND:
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private static int GetComboFactor(DamageInformation dmgInfo)
        {
            var _comboFactor = 1;
            if (dmgInfo.CardType == CARD_TYPE.HEAVY)
            {
                _comboFactor = GlobalConfig.Instance.globalCardDataSO.comboSystemFactor;
            }
            return dmgInfo.CurrentCombo / _comboFactor;
        }

        private static bool CheckCriticalHit(DamageInformation dmgInfo)
        {   
            var _precision = dmgInfo.ToyoStats[TOYO_STAT.PRECISION];
            _precision *= BoundSystem.GetFactorInMyBuffs(dmgInfo, TOYO_STAT.PRECISION);
            Debug.Log("MyTOYO_PRECISION: " + _precision);

            var _luck = dmgInfo.ToyoStats[TOYO_STAT.LUCK];
            _luck *= BoundSystem.GetFactorInMyBuffs(dmgInfo, TOYO_STAT.LUCK);

            var _criticalChance =
            (_precision * GlobalConfig.Instance.globalCardDataSO.criticalPrecisionFactor)
            + (_luck * GlobalConfig.Instance.globalCardDataSO.criticalLuckFactor);

            var _maxCritical = GlobalConfig.Instance.globalCardDataSO.maxCriticalChance;
            _criticalChance = _criticalChance > _maxCritical ? _maxCritical : _criticalChance;

            return Random.Range(0, 100) <= _criticalChance;
        }

        private static float GetEnemyDef(DamageInformation dmgInfo, bool criticalHit)
        {
            if (BoundSystem.GetTrueDamage(dmgInfo)) { return 0; }
            
            var _enemyResistance = dmgInfo.EnemyToyoStats[TOYO_STAT.RESISTANCE];
            Debug.Log("EnemyResistance: " + _enemyResistance);
            _enemyResistance *= BoundSystem.GetFactorInEnemyBuffs(dmgInfo, TOYO_STAT.RESISTANCE);

            var _enemyResilience = dmgInfo.EnemyToyoStats[TOYO_STAT.RESILIENCE];
            Debug.Log("EnemyResilience: " + _enemyResilience);
            _enemyResilience *= BoundSystem.GetFactorInEnemyBuffs(dmgInfo, TOYO_STAT.RESILIENCE);

            var _enemyDef = dmgInfo.AttackType == ATTACK_TYPE.PHYSICAL ?
                (_enemyResistance * 
                 GlobalConfig.Instance.globalCardDataSO.enemyResistanceMultiplier) : 
                (_enemyResilience *
                 GlobalConfig.Instance.globalCardDataSO.enemyResilienceMultiplier);
            if (criticalHit)
            {
                _enemyDef *= GlobalConfig.Instance.globalCardDataSO.defenseInCriticalMultiplier;
            }
            
            return _enemyDef;
        }

        private static float GetHitVariation(DamageInformation dmgInfo, bool criticalHit, int hitIndex)
            => criticalHit ? dmgInfo.HitVariation[hitIndex].Damage 
                            * GlobalConfig.Instance.globalCardDataSO.criticalDamageModifier 
                            : dmgInfo.HitVariation[hitIndex].Damage;
    }
}