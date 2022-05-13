using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CombatSystem
{
    public class DefenseSystem : MonoBehaviour
    {
        private static float GetDefenseChance(DamageInformation dmgInfo)
        {
            float _defChance = 100;
            switch (dmgInfo.DefenseType)
            {
                case DEFENSE_TYPE.BLOCK:
                    var _tech = dmgInfo.ToyoStats[TOYO_STAT.TECHNIQUE];
                    _tech *= BoundSystem.GetFactorInMyBuffs(dmgInfo, TOYO_STAT.TECHNIQUE);

                    var _enemyAnalysis = dmgInfo.EnemyToyoStats[TOYO_STAT.ANALYSIS];
                    _enemyAnalysis *= BoundSystem.GetFactorInEnemyBuffs(dmgInfo, TOYO_STAT.ANALYSIS);

                    _defChance += ((_tech * GlobalConfig.Instance.globalCardDataSO.techDefMultiplier) 
                           - (_enemyAnalysis * GlobalConfig.Instance.globalCardDataSO.analysisMultiplier));
                    break;
                case  DEFENSE_TYPE.DODGE:
                    var _agility = dmgInfo.ToyoStats[TOYO_STAT.AGILITY];
                    _agility *= BoundSystem.GetFactorInMyBuffs(dmgInfo, TOYO_STAT.AGILITY);

                    var _enemySpeed = dmgInfo.EnemyToyoStats[TOYO_STAT.SPEED];
                    _enemySpeed *= BoundSystem.GetFactorInEnemyBuffs(dmgInfo, TOYO_STAT.SPEED);

                    _defChance += ((_agility * GlobalConfig.Instance.globalCardDataSO.agilityDefMultiplier)
                                  - (_enemySpeed * GlobalConfig.Instance.globalCardDataSO.speedMultiplier));
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return _defChance;
        }

        public static bool DefenseCardSuccess(DamageInformation dmgInfo)
        {
            var defenseChance = GetDefenseChance(dmgInfo);
            return Random.Range(0, 100) <= defenseChance;
        }

        private static float GetCounterChance(DamageInformation dmgInfo)
        {
            var _counterChance = GlobalConfig.Instance.globalCardDataSO.baseCounterChance;
            switch (dmgInfo.DefenseType)
            {
                case DEFENSE_TYPE.BLOCK:
                    var _tech = dmgInfo.ToyoStats[TOYO_STAT.TECHNIQUE];
                    _tech *= BoundSystem.GetFactorInMyBuffs(dmgInfo, TOYO_STAT.TECHNIQUE);

                    var _luck = dmgInfo.ToyoStats[TOYO_STAT.LUCK];
                    _luck *= BoundSystem.GetFactorInMyBuffs(dmgInfo, TOYO_STAT.LUCK);
                    
                    _counterChance += ((_tech * GlobalConfig.Instance.globalCardDataSO.counterTechMultiplier) 
                                       + (_luck * GlobalConfig.Instance.globalCardDataSO.counterLuckFactor));
                    break;
                case  DEFENSE_TYPE.DODGE:
                    var _agility = dmgInfo.ToyoStats[TOYO_STAT.AGILITY];
                    _agility *= BoundSystem.GetFactorInMyBuffs(dmgInfo, TOYO_STAT.AGILITY);
                    
                    var _lucky = dmgInfo.ToyoStats[TOYO_STAT.LUCK];
                    _lucky *= BoundSystem.GetFactorInMyBuffs(dmgInfo, TOYO_STAT.LUCK);
                    
                    _counterChance += ((_agility * GlobalConfig.Instance.globalCardDataSO.counterAgilityMultiplier) 
                                       + (_lucky * GlobalConfig.Instance.globalCardDataSO.counterLuckFactor));
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return _counterChance;
        }
        
        public static bool CounterCardSuccess(DamageInformation dmgInfo)
            => GetCounterChance(dmgInfo) >= Random.Range(0, 100);
        
        public static bool NaturalEnemyDefense(DamageInformation dmgInfo)
            => GetEnemyDefenseChance(dmgInfo) >= Random.Range(0, 100);

        public static bool NaturalEnemyCounter(DamageInformation dmgInfo) 
            => GetEnemyCounterChance(dmgInfo) >= Random.Range(0, 100);

        private static float GetEnemyDefenseChance(DamageInformation dmgInfo)
        {
            float _defChance = 0;
            switch (dmgInfo.DefenseType)
            {
                case DEFENSE_TYPE.BLOCK:
                    var _tech = dmgInfo.EnemyToyoStats[TOYO_STAT.TECHNIQUE];
                    _tech *= BoundSystem.GetFactorInMyBuffs(dmgInfo, TOYO_STAT.TECHNIQUE);

                    var _enemyAnalysis = dmgInfo.ToyoStats[TOYO_STAT.ANALYSIS];
                    _enemyAnalysis *= BoundSystem.GetFactorInEnemyBuffs(dmgInfo, TOYO_STAT.ANALYSIS);

                    _defChance += ((_tech * GlobalConfig.Instance.globalCardDataSO.techDefMultiplier) 
                                   - (_enemyAnalysis * GlobalConfig.Instance.globalCardDataSO.analysisMultiplier));
                    break;
                case  DEFENSE_TYPE.DODGE:
                    var _agility = dmgInfo.EnemyToyoStats[TOYO_STAT.AGILITY];
                    _agility *= BoundSystem.GetFactorInMyBuffs(dmgInfo, TOYO_STAT.AGILITY);

                    var _enemySpeed = dmgInfo.ToyoStats[TOYO_STAT.SPEED];
                    _enemySpeed *= BoundSystem.GetFactorInEnemyBuffs(dmgInfo, TOYO_STAT.SPEED);

                    _defChance += ((_agility * GlobalConfig.Instance.globalCardDataSO.agilityDefMultiplier)
                                   - (_enemySpeed * GlobalConfig.Instance.globalCardDataSO.speedMultiplier));
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            return _defChance;
        }
        
        private static float GetEnemyCounterChance(DamageInformation dmgInfo)
        {
            var _counterChance = GlobalConfig.Instance.globalCardDataSO.baseCounterChance;
            switch (dmgInfo.DefenseType)
            {
                case DEFENSE_TYPE.BLOCK:
                    var _tech = dmgInfo.EnemyToyoStats[TOYO_STAT.TECHNIQUE];
                    _tech *= BoundSystem.GetFactorInMyBuffs(dmgInfo, TOYO_STAT.TECHNIQUE);

                    var _luck = dmgInfo.EnemyToyoStats[TOYO_STAT.LUCK];
                    _luck *= BoundSystem.GetFactorInMyBuffs(dmgInfo, TOYO_STAT.LUCK);
                    
                    _counterChance += ((_tech * GlobalConfig.Instance.globalCardDataSO.counterTechMultiplier) 
                                       + (_luck * GlobalConfig.Instance.globalCardDataSO.counterLuckFactor));
                    break;
                case  DEFENSE_TYPE.DODGE:
                    var _agility = dmgInfo.EnemyToyoStats[TOYO_STAT.AGILITY];
                    _agility *= BoundSystem.GetFactorInMyBuffs(dmgInfo, TOYO_STAT.AGILITY);
                    
                    var _lucky = dmgInfo.EnemyToyoStats[TOYO_STAT.LUCK];
                    _lucky *= BoundSystem.GetFactorInMyBuffs(dmgInfo, TOYO_STAT.LUCK);
                    
                    _counterChance += ((_agility * GlobalConfig.Instance.globalCardDataSO.counterAgilityMultiplier) 
                                       + (_lucky * GlobalConfig.Instance.globalCardDataSO.counterLuckFactor));
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return _counterChance;
        }
    }
}
