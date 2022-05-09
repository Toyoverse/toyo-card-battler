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
                ? CalculateStrenght(dmgInfo)
                : CalculateCyberForce(dmgInfo);

            return ApplyDamageCalculation(dmgInfo, _damage);
        }

        static float CalculateBondCard(DamageInformation dmgInfo)
        {
            return GlobalConfig.Instance.globalCardDataSO.bondCardDamage;
        }
        
        static float CalculateStrenght(DamageInformation dmgInfo)
        {
            var _strenght = dmgInfo.ToyoStats[TOYO_STAT.PHYSICAL_STRENGHT];
            if (DamageSystem.GetMyEffectInBuffs(dmgInfo, TOYO_STAT.PHYSICAL_STRENGHT))
            {
                var _effect = DamageSystem.GetMyEffectInBuffs(
                    dmgInfo, TOYO_STAT.PHYSICAL_STRENGHT);
                _strenght *= _effect.changeFactor;
                BoundSystem.RemoveEffect(dmgInfo, _effect);
            }
            var _hitVariation = dmgInfo.HitVariation;
            return _strenght * _hitVariation;
        }
        
        static float CalculateCyberForce(DamageInformation dmgInfo)
        {
            var _cyberForce = dmgInfo.ToyoStats[TOYO_STAT.CYBER_FORCE];
            if (DamageSystem.GetMyEffectInBuffs(dmgInfo, TOYO_STAT.CYBER_FORCE))
            {
                var _effect = DamageSystem.GetMyEffectInBuffs(
                    dmgInfo, TOYO_STAT.CYBER_FORCE);
                _cyberForce *= _effect.changeFactor;
                BoundSystem.RemoveEffect(dmgInfo, _effect);
            }
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
            if (DamageSystem.GetMyEffectInBuffs(dmgInfo, TOYO_STAT.PRECISION))
            {
                var _effect = DamageSystem.GetMyEffectInBuffs(
                    dmgInfo, TOYO_STAT.PRECISION);
                _precision *= _effect.changeFactor;
                BoundSystem.RemoveEffect(dmgInfo, _effect);
            }
            var _luck = dmgInfo.ToyoStats[TOYO_STAT.LUCK];
            if (DamageSystem.GetMyEffectInBuffs(dmgInfo, TOYO_STAT.LUCK))
            {
                var _effect = DamageSystem.GetMyEffectInBuffs(
                    dmgInfo, TOYO_STAT.LUCK);
                _luck *= _effect.changeFactor;
                BoundSystem.RemoveEffect(dmgInfo, _effect);
            }

            var _criticalChance =
                (_precision * GlobalConfig.Instance.globalCardDataSO.criticalPrecisionFactor)
                + (_luck * GlobalConfig.Instance.globalCardDataSO.criticalLuckFactor);

            var _maxCritical = GlobalConfig.Instance.globalCardDataSO.maxCriticalChance;
            _criticalChance = _criticalChance > _maxCritical ? _maxCritical : _criticalChance;
            return Random.Range(0, 100) >= _criticalChance;
        }

        static float GetEnemyDef(DamageInformation dmgInfo, bool criticalHit)
        {
            var _enemyResistance = dmgInfo.EnemyToyoStats[TOYO_STAT.RESISTANCE];
            if (DamageSystem.GetEnemyEffectInEnemyBuffs(dmgInfo, TOYO_STAT.RESISTANCE))
            {
                var _effect = DamageSystem.GetEnemyEffectInEnemyBuffs(
                    dmgInfo, TOYO_STAT.RESISTANCE);
                _enemyResistance *= _effect.changeFactor;
                BoundSystem.RemoveEffect(dmgInfo, _effect);
            }
            var _enemyResilience = dmgInfo.EnemyToyoStats[TOYO_STAT.RESILIENCE];
            if (DamageSystem.GetEnemyEffectInEnemyBuffs(dmgInfo, TOYO_STAT.RESILIENCE))
            {
                var _effect = DamageSystem.GetEnemyEffectInEnemyBuffs(
                    dmgInfo, TOYO_STAT.RESILIENCE);
                _enemyResilience *= _effect.changeFactor;
                BoundSystem.RemoveEffect(dmgInfo, _effect);
            }
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

        static float GetHitVariation(DamageInformation dmgInfo, bool criticalHit)
        {
            return criticalHit ? 
                dmgInfo.HitVariation * GlobalConfig.Instance.globalCardDataSO.criticalDamageModifier
                : dmgInfo.HitVariation;
        }
    }
}