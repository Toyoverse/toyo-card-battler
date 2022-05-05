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
                //TODO: Use 'ref' on defense. Needs get "success" and "counter" booleans.
                CARD_TYPE.DEFENSE => CalculateDefenseCard(dmgInfo),
                CARD_TYPE.BOND => CalculateBondCard(dmgInfo),
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        static float CalculateDefenseCard(DamageInformation dmgInfo)
        {   //TODO: We need the defense card subtype (block or dodge) to know what to apply.
            var _blockCard = true;
            var _counter = false;
            var _success = false;
            if (_blockCard)
            {
                //TODO: Turn this into a method. GetBlockChance() or GetDefChance()
                var _blockChance = 100 + ((dmgInfo.ToyoStats[TOYO_STAT.TECHNIQUE]
                                           * GlobalConfig.Instance.globalCardDataSO.techDefMultiplier)
                                           - (dmgInfo.EnemyToyoStats[TOYO_STAT.ANALYSIS] *
                                           GlobalConfig.Instance.globalCardDataSO.analysisMultiplier));
                _success = Random.Range(0, 100) <= _blockChance;
                if (_success)
                {
                    //TODO: Turn this into a method. GetCounterChance()
                    var _counterBlockChance = GlobalConfig.Instance.globalCardDataSO.baseCounterChance
                                              + ((dmgInfo.ToyoStats[TOYO_STAT.TECHNIQUE] * 
                                                  GlobalConfig.Instance.globalCardDataSO.counterTechMultiplier) + 
                                                 (dmgInfo.ToyoStats[TOYO_STAT.LUCK] * 
                                                  GlobalConfig.Instance.globalCardDataSO.counterLuckFactor));
                    _counter = Random.Range(0, 100) <= _counterBlockChance;
                }
            }
            else
            {
                var _dodgeChance = 100 + ((dmgInfo.ToyoStats[TOYO_STAT.AGILITY]
                                           * GlobalConfig.Instance.globalCardDataSO.agilityDefMultiplier)
                                           - (dmgInfo.EnemyToyoStats[TOYO_STAT.SPEED] 
                                           * GlobalConfig.Instance.globalCardDataSO.speedMultiplier));
                _success = Random.Range(0, 100) <= _dodgeChance;
                if (_success)
                {
                    var _counterDodgeChance = GlobalConfig.Instance.globalCardDataSO.baseCounterChance 
                                                + ((dmgInfo.ToyoStats[TOYO_STAT.AGILITY] 
                                                * GlobalConfig.Instance.globalCardDataSO.counterAgilityMultiplier) 
                                                + (dmgInfo.ToyoStats[TOYO_STAT.LUCK] 
                                                * GlobalConfig.Instance.globalCardDataSO.counterLuckFactor));
                    _counter = Random.Range(0, 100) < _counterDodgeChance;
                }
            }
            
            //TODO: Return success and counter
            return 0.0f;
        }
        
        static float CalculateBondCard(DamageInformation dmgInfo)
        {
            return GlobalConfig.Instance.globalCardDataSO.bondCardDamage;
        }

        static void CalculateResistance(DamageInformation dmgInfo)
        {
            
        }

        static void CalculateResilience(DamageInformation dmgInfo)
        {
            
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

        static void CalculatePrecision(DamageInformation dmgInfo)
        {
            
        }

        /// <summary>
        /// Returns damage dealt after applying modifiers.
        /// </summary>
        static float ApplyDamageCalculation(DamageInformation dmgInfo, float damage)
        {
            float _sum = 0;
            float _multiplier = 1;
            GetSumAndMultiplier(dmgInfo.CardType, ref _sum, ref _multiplier);

            //TODO: Turn this into a method. GetComboFactor()
            var _comboFactor = 1;
            if (dmgInfo.CardType == CARD_TYPE.HEAVY)
            {
                _comboFactor = GlobalConfig.Instance.globalCardDataSO.comboSystemFactor;
            }
            var comboFactorResult = dmgInfo.CurrentCombo / _comboFactor;
            
            //TODO: Turn this into a method. GetEnemyDef()
            var _enemyDef = dmgInfo.AttackType == ATTACK_TYPE.PHYSICAL ?
                (dmgInfo.EnemyToyoStats[TOYO_STAT.RESISTANCE] * 
                 GlobalConfig.Instance.globalCardDataSO.resistenceMultiplier) : 
                (dmgInfo.EnemyToyoStats[TOYO_STAT.RESILIENCE] *
                 GlobalConfig.Instance.globalCardDataSO.resilianceMultiplier);

            var _critical = CheckCriticalHit(dmgInfo);
            
            _enemyDef = _critical ? GlobalConfig.Instance.globalCardDataSO.defenseInCritical : _enemyDef;
            var _hitVar = _critical ? 
                dmgInfo.HitVariation * GlobalConfig.Instance.globalCardDataSO.criticalDamageModifier
                : dmgInfo.HitVariation;
            
            var _damageResult = _multiplier * (_sum + comboFactorResult * _hitVar * damage);
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
    }
}