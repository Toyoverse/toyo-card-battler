using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CombatSystem
{
    public class DefenseSystem : MonoBehaviour
    {
        /*public static void DefenseCalculation(DamageInformation dmgInfo)
        {
            var success = DefenseSuccess(dmgInfo);
            var counter = success && CounterSuccess(dmgInfo);
        }*/

        static float GetDefenseChance(DamageInformation dmgInfo)
        {
            float _defChance = 100;
            switch (dmgInfo.DefenseType)
            {
                case DEFENSE_TYPE.BLOCK:
                    var _tech = dmgInfo.ToyoStats[TOYO_STAT.TECHNIQUE];
                    if (DamageSystem.GetMyEffectInBuffs(dmgInfo, TOYO_STAT.TECHNIQUE))
                    {
                        var _effect = DamageSystem.GetMyEffectInBuffs(dmgInfo, TOYO_STAT.TECHNIQUE);
                        _tech *= _effect.changeFactor;
                        BoundSystem.RemoveEffect(dmgInfo, _effect);
                    }
                    var _enemyAnalysis = dmgInfo.EnemyToyoStats[TOYO_STAT.ANALYSIS];
                    if (DamageSystem.GetEnemyEffectInEnemyBuffs(dmgInfo, TOYO_STAT.ANALYSIS))
                    {
                        var _effect = DamageSystem.GetEnemyEffectInEnemyBuffs(
                            dmgInfo, TOYO_STAT.ANALYSIS);
                        _enemyAnalysis *= _effect.changeFactor;
                        BoundSystem.RemoveEffect(dmgInfo, _effect);
                    }
                    _defChance += ((_tech * GlobalConfig.Instance.globalCardDataSO.techDefMultiplier) 
                           - (_enemyAnalysis * GlobalConfig.Instance.globalCardDataSO.analysisMultiplier));
                    break;
                case  DEFENSE_TYPE.DODGE:
                    var _agility = dmgInfo.ToyoStats[TOYO_STAT.AGILITY];
                    if (DamageSystem.GetMyEffectInBuffs(dmgInfo, TOYO_STAT.AGILITY))
                    {
                        var _effect = DamageSystem.GetMyEffectInBuffs(
                            dmgInfo, TOYO_STAT.AGILITY);
                        _agility *= _effect.changeFactor;
                        BoundSystem.RemoveEffect(dmgInfo, _effect);
                    }
                    var _enemySpeed = dmgInfo.EnemyToyoStats[TOYO_STAT.SPEED];
                    if (DamageSystem.GetEnemyEffectInEnemyBuffs(dmgInfo, TOYO_STAT.SPEED))
                    {
                        var _effect = DamageSystem.GetEnemyEffectInEnemyBuffs(
                            dmgInfo, TOYO_STAT.SPEED);
                        _enemySpeed *= _effect.changeFactor;
                        BoundSystem.RemoveEffect(dmgInfo, _effect);
                    }
                    _defChance += ((_agility * GlobalConfig.Instance.globalCardDataSO.agilityDefMultiplier)
                                  - (_enemySpeed * GlobalConfig.Instance.globalCardDataSO.speedMultiplier));
                    break;
            }

            return _defChance;
        }

        public static bool DefenseSuccess(DamageInformation dmgInfo)
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
                    var _tech = dmgInfo.ToyoStats[TOYO_STAT.TECHNIQUE];
                    if (DamageSystem.GetMyEffectInBuffs(dmgInfo, TOYO_STAT.TECHNIQUE))
                    {
                        var _effect = DamageSystem.GetMyEffectInBuffs(
                            dmgInfo, TOYO_STAT.TECHNIQUE);
                        _tech *= _effect.changeFactor;
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
                    _counterChance += ((_tech * 
                                        GlobalConfig.Instance.globalCardDataSO.counterTechMultiplier) 
                                       + (_luck * 
                                       GlobalConfig.Instance.globalCardDataSO.counterLuckFactor));
                    break;
                case  DEFENSE_TYPE.DODGE:
                    var _agility = dmgInfo.ToyoStats[TOYO_STAT.AGILITY];
                    if (DamageSystem.GetMyEffectInBuffs(dmgInfo, TOYO_STAT.AGILITY))
                    {
                        var _effect = DamageSystem.GetMyEffectInBuffs(
                            dmgInfo, TOYO_STAT.AGILITY);
                        _agility *= _effect.changeFactor;
                        BoundSystem.RemoveEffect(dmgInfo, _effect);
                    }
                    var _lucky = dmgInfo.ToyoStats[TOYO_STAT.LUCK];
                    if (DamageSystem.GetMyEffectInBuffs(dmgInfo, TOYO_STAT.LUCK))
                    {
                        var _effect = DamageSystem.GetMyEffectInBuffs(
                            dmgInfo, TOYO_STAT.LUCK);
                        _lucky *= _effect.changeFactor;
                        BoundSystem.RemoveEffect(dmgInfo, _effect);
                    }
                    _counterChance += ((_agility
                                      * GlobalConfig.Instance.globalCardDataSO.counterAgilityMultiplier) 
                                      + (_lucky 
                                      * GlobalConfig.Instance.globalCardDataSO.counterLuckFactor));
                    break;
            }

            return _counterChance;
        }
        
        public static bool CounterSuccess(DamageInformation dmgInfo)
        {
            float counterChance = GetCounterChance(dmgInfo);
            return Random.Range(0, 100) <= counterChance;
        }
    }
}
