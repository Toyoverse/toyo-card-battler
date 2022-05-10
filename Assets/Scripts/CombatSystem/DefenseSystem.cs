using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CombatSystem
{
    public class DefenseSystem : MonoBehaviour
    {
        /*public static void DefenseCalculation(DamageInformation dmgInfo)
        {
            var success = DefenseSuccess(dmgInfo);
            var counter = success && CounterSuccess(dmgInfo);
        }*/

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

        public static bool DefenseSuccess(DamageInformation dmgInfo)
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
                    
                    _counterChance += ((_tech * 
                                        GlobalConfig.Instance.globalCardDataSO.counterTechMultiplier) 
                                       + (_luck * 
                                       GlobalConfig.Instance.globalCardDataSO.counterLuckFactor));
                    break;
                case  DEFENSE_TYPE.DODGE:
                    var _agility = dmgInfo.ToyoStats[TOYO_STAT.AGILITY];
                    _agility *= BoundSystem.GetFactorInMyBuffs(dmgInfo, TOYO_STAT.AGILITY);
                    
                    var _lucky = dmgInfo.ToyoStats[TOYO_STAT.LUCK];
                    _lucky *= BoundSystem.GetFactorInMyBuffs(dmgInfo, TOYO_STAT.LUCK);
                    
                    _counterChance += ((_agility
                                        * GlobalConfig.Instance.globalCardDataSO.counterAgilityMultiplier) 
                                       + (_lucky 
                                          * GlobalConfig.Instance.globalCardDataSO.counterLuckFactor));
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return _counterChance;
        }
        
        public static bool CounterSuccess(DamageInformation dmgInfo)
        {
            var counterChance = GetCounterChance(dmgInfo);
            return Random.Range(0, 100) <= counterChance;
        }
    }
}
