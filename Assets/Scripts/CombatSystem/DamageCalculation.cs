using System;
using System.Collections.Generic;
using Card;
using ToyoSystem;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

namespace CombatSystem
{
    public class DamageCalculation
    {
        public static float CalculateDamage(DamageInformation dmgInfo)
        {
            var _attackType = dmgInfo.AttackType;
            var _damage = _attackType == ATTACK_TYPE.PHYSICAL
                ? CalculateStrength(dmgInfo)
                : CalculateCyberForce(dmgInfo);

            return ApplyDamageCalculation(dmgInfo, _damage);
        }

        private static float CalculateStrength(DamageInformation dmgInfo)
        {
            var _strength = dmgInfo.ToyoStats[TOYO_STAT.PHYSICAL_STRENGHT];
            _strength *= BoundSystem.GetFactorInMyBuffs(dmgInfo, TOYO_STAT.PHYSICAL_STRENGHT);

            var _hitVariation = dmgInfo.HitVariation;
            return _strength * _hitVariation;
        }

        private static float CalculateCyberForce(DamageInformation dmgInfo)
        {
            var _cyberForce = dmgInfo.ToyoStats[TOYO_STAT.CYBER_FORCE];
            _cyberForce *= BoundSystem.GetFactorInMyBuffs(dmgInfo, TOYO_STAT.CYBER_FORCE);

            var _hitVariation = dmgInfo.HitVariation;
            return _cyberForce * _hitVariation;
        }

        /// <summary>
        /// Returns damage dealt after applying modifiers.
        /// </summary>
        private static float ApplyDamageCalculation(DamageInformation dmgInfo, float damage)
        {
            float _sum = 0;
            float _multiplier = 1;
            GetSumAndMultiplier(dmgInfo.CardType, ref _sum, ref _multiplier);
            
            var _comboFactor = GetComboFactor(dmgInfo);
            
            var _criticalHit = CheckCriticalHit(dmgInfo);
            
            var _enemyDef = GetEnemyDef(dmgInfo, _criticalHit);

            var _hitVar = GetHitVariation(dmgInfo, _criticalHit);
            
            var _damageResult = _multiplier * ((_sum + _comboFactor) * _hitVar * damage);
            _damageResult -= _enemyDef;
            
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

            var _luck = dmgInfo.ToyoStats[TOYO_STAT.LUCK];
            _luck *= BoundSystem.GetFactorInMyBuffs(dmgInfo, TOYO_STAT.LUCK);

            var _criticalChance =
            (_precision * GlobalConfig.Instance.globalCardDataSO.criticalPrecisionFactor)
            + (_luck * GlobalConfig.Instance.globalCardDataSO.criticalLuckFactor);

            var _maxCritical = GlobalConfig.Instance.globalCardDataSO.maxCriticalChance;
            _criticalChance = _criticalChance > _maxCritical ? _maxCritical : _criticalChance;
            return Random.Range(0, 100) >= _criticalChance;
        }

        private static float GetEnemyDef(DamageInformation dmgInfo, bool criticalHit)
        {
            var _enemyResistance = dmgInfo.EnemyToyoStats[TOYO_STAT.RESISTANCE];
            _enemyResistance *= BoundSystem.GetFactorInEnemyBuffs(dmgInfo, TOYO_STAT.RESISTANCE);

            var _enemyResilience = dmgInfo.EnemyToyoStats[TOYO_STAT.RESILIENCE];
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

        private static float GetHitVariation(DamageInformation dmgInfo, bool criticalHit)
        {
            return criticalHit ? 
                dmgInfo.HitVariation * GlobalConfig.Instance.globalCardDataSO.criticalDamageModifier
                : dmgInfo.HitVariation;
        }
    }
}