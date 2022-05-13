using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CombatSystem
{
    public class DefenseSystem : MonoBehaviour
    {
        public static bool DefenseCardSuccess(DamageInformation dmgInfo)
            => GetDefenseChance(dmgInfo) >= Random.Range(0, 100);

        private static float GetDefenseChance(DamageInformation dmgInfo)
            => dmgInfo.DefenseType switch
            {
                DEFENSE_TYPE.BLOCK => GetDefChanceInBlock(dmgInfo),
                DEFENSE_TYPE.DODGE => GetDefChanceInDodge(dmgInfo),
                _ => throw new ArgumentOutOfRangeException()
            };

        private static float GetDefChanceInBlock(DamageInformation dmgInfo)
        {
            float _defChance = 100;
            var _tech = dmgInfo.ToyoStats[TOYO_STAT.TECHNIQUE];
            _tech *= BoundSystem.GetFactorInMyBuffs(dmgInfo, TOYO_STAT.TECHNIQUE);

            var _enemyAnalysis = dmgInfo.EnemyToyoStats[TOYO_STAT.ANALYSIS];
            _enemyAnalysis *= BoundSystem.GetFactorInEnemyBuffs(dmgInfo, TOYO_STAT.ANALYSIS);

            _defChance += ((_tech * GlobalConfig.Instance.globalCardDataSO.techDefMultiplier)
                           - (_enemyAnalysis * GlobalConfig.Instance.globalCardDataSO.analysisMultiplier));
            return _defChance;
        }

        private static float GetDefChanceInDodge(DamageInformation dmgInfo)
        {
            float _defChance = 100;
            var _agility = dmgInfo.ToyoStats[TOYO_STAT.AGILITY];
            _agility *= BoundSystem.GetFactorInMyBuffs(dmgInfo, TOYO_STAT.AGILITY);

            var _enemySpeed = dmgInfo.EnemyToyoStats[TOYO_STAT.SPEED];
            _enemySpeed *= BoundSystem.GetFactorInEnemyBuffs(dmgInfo, TOYO_STAT.SPEED);

            _defChance += ((_agility * GlobalConfig.Instance.globalCardDataSO.agilityDefMultiplier)
                           - (_enemySpeed * GlobalConfig.Instance.globalCardDataSO.speedMultiplier));
            return _defChance;
        }

        private static float GetCounterChance(DamageInformation dmgInfo)
            => dmgInfo.DefenseType switch
            {
                DEFENSE_TYPE.BLOCK => GetCounterChanceInBlock(dmgInfo),
                DEFENSE_TYPE.DODGE => GetCounterChanceInDodge(dmgInfo),
                _ => throw new ArgumentOutOfRangeException()
            };

        private static float GetCounterChanceInBlock(DamageInformation dmgInfo)
        {
            var _counterChance = GlobalConfig.Instance.globalCardDataSO.baseCounterChance;
            var _tech = dmgInfo.ToyoStats[TOYO_STAT.TECHNIQUE];
            _tech *= BoundSystem.GetFactorInMyBuffs(dmgInfo, TOYO_STAT.TECHNIQUE);

            var _luck = dmgInfo.ToyoStats[TOYO_STAT.LUCK];
            _luck *= BoundSystem.GetFactorInMyBuffs(dmgInfo, TOYO_STAT.LUCK);

            _counterChance += ((_tech * GlobalConfig.Instance.globalCardDataSO.counterTechMultiplier)
                               + (_luck * GlobalConfig.Instance.globalCardDataSO.counterLuckFactor));
            return _counterChance;
        }

        private static float GetCounterChanceInDodge(DamageInformation dmgInfo)
        {
            var _counterChance = GlobalConfig.Instance.globalCardDataSO.baseCounterChance;
            var _agility = dmgInfo.ToyoStats[TOYO_STAT.AGILITY];
            _agility *= BoundSystem.GetFactorInMyBuffs(dmgInfo, TOYO_STAT.AGILITY);

            var _lucky = dmgInfo.ToyoStats[TOYO_STAT.LUCK];
            _lucky *= BoundSystem.GetFactorInMyBuffs(dmgInfo, TOYO_STAT.LUCK);

            _counterChance += ((_agility * GlobalConfig.Instance.globalCardDataSO.counterAgilityMultiplier)
                               + (_lucky * GlobalConfig.Instance.globalCardDataSO.counterLuckFactor));
            return _counterChance;
        }

        public static bool CounterCardSuccess(DamageInformation dmgInfo)
            => GetCounterChance(dmgInfo) >= Random.Range(0, 100);

        public static bool NaturalEnemyDefense(DamageInformation dmgInfo)
            => GetEnemyDefenseChance(dmgInfo) >= Random.Range(0, 100);

        public static bool NaturalEnemyCounter(DamageInformation dmgInfo)
            => GetEnemyCounterChance(dmgInfo) >= Random.Range(0, 100);

        private static float GetEnemyDefenseChance(DamageInformation dmgInfo)
            => dmgInfo.DefenseType switch
            {
                DEFENSE_TYPE.BLOCK => GetEnemyDefChanceInBlock(dmgInfo),
                DEFENSE_TYPE.DODGE => GetEnemyDefChanceInDodge(dmgInfo),
                _ => throw new ArgumentOutOfRangeException()
            };

        private static float GetEnemyDefChanceInBlock(DamageInformation dmgInfo)
        {
            float _defChance = 0;
            var _tech = dmgInfo.EnemyToyoStats[TOYO_STAT.TECHNIQUE];
            _tech *= BoundSystem.GetFactorInMyBuffs(dmgInfo, TOYO_STAT.TECHNIQUE);

            var _enemyAnalysis = dmgInfo.ToyoStats[TOYO_STAT.ANALYSIS];
            _enemyAnalysis *= BoundSystem.GetFactorInEnemyBuffs(dmgInfo, TOYO_STAT.ANALYSIS);

            _defChance += ((_tech * GlobalConfig.Instance.globalCardDataSO.techDefMultiplier)
                           - (_enemyAnalysis * GlobalConfig.Instance.globalCardDataSO.analysisMultiplier));
            return _defChance;
        }

        private static float GetEnemyDefChanceInDodge(DamageInformation dmgInfo)
        {
            float _defChance = 0;
            var _agility = dmgInfo.EnemyToyoStats[TOYO_STAT.AGILITY];
            _agility *= BoundSystem.GetFactorInMyBuffs(dmgInfo, TOYO_STAT.AGILITY);

            var _enemySpeed = dmgInfo.ToyoStats[TOYO_STAT.SPEED];
            _enemySpeed *= BoundSystem.GetFactorInEnemyBuffs(dmgInfo, TOYO_STAT.SPEED);

            _defChance += ((_agility * GlobalConfig.Instance.globalCardDataSO.agilityDefMultiplier)
                           - (_enemySpeed * GlobalConfig.Instance.globalCardDataSO.speedMultiplier));
            return _defChance;
        }

        private static float GetEnemyCounterChance(DamageInformation dmgInfo)
            => dmgInfo.DefenseType switch
            {
                DEFENSE_TYPE.BLOCK => GetEnemyCounterChanceInBlock(dmgInfo),
                DEFENSE_TYPE.DODGE => GetEnemyCounterChanceInDodge(dmgInfo),
                _ => throw new ArgumentOutOfRangeException()
            };

        private static float GetEnemyCounterChanceInBlock(DamageInformation dmgInfo)
        {
            var _counterChance = GlobalConfig.Instance.globalCardDataSO.baseCounterChance;
            var _tech = dmgInfo.EnemyToyoStats[TOYO_STAT.TECHNIQUE];
            _tech *= BoundSystem.GetFactorInMyBuffs(dmgInfo, TOYO_STAT.TECHNIQUE);

            var _luck = dmgInfo.EnemyToyoStats[TOYO_STAT.LUCK];
            _luck *= BoundSystem.GetFactorInMyBuffs(dmgInfo, TOYO_STAT.LUCK);

            _counterChance += ((_tech * GlobalConfig.Instance.globalCardDataSO.counterTechMultiplier)
                               + (_luck * GlobalConfig.Instance.globalCardDataSO.counterLuckFactor));
            return _counterChance;
        }

        private static float GetEnemyCounterChanceInDodge(DamageInformation dmgInfo)
        {
            var _counterChance = GlobalConfig.Instance.globalCardDataSO.baseCounterChance;
            var _agility = dmgInfo.EnemyToyoStats[TOYO_STAT.AGILITY];
            _agility *= BoundSystem.GetFactorInMyBuffs(dmgInfo, TOYO_STAT.AGILITY);

            var _lucky = dmgInfo.EnemyToyoStats[TOYO_STAT.LUCK];
            _lucky *= BoundSystem.GetFactorInMyBuffs(dmgInfo, TOYO_STAT.LUCK);

            _counterChance += ((_agility * GlobalConfig.Instance.globalCardDataSO.counterAgilityMultiplier)
                               + (_lucky * GlobalConfig.Instance.globalCardDataSO.counterLuckFactor));
            return _counterChance;
        }
    }
}
