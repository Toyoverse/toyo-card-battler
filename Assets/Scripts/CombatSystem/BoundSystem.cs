using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Card;
using UnityEngine;

namespace CombatSystem
{
    public class BoundSystem : MonoBehaviour
    {
        public static void BoundProcess(DamageInformation dmgInfo, ref DamageSystem dmgSys)
        {
            if (dmgInfo.EffectData == null) return;
            foreach (var effect in dmgInfo.EffectData)
            {
                if (effect == null) { continue; }
                if (effect.temporary) { AddEffect(dmgInfo, effect); }
                else { PerformEffect(ref dmgSys, effect); }
            }
        }
        
        private static void AddEffect(DamageInformation dmgInfo, EffectData effect)
        {
            Debug.Log("addEffect: " + effect.EffectType);
            effect.timeUsed = 0;
            if (effect.Toyo == TOYO_TYPE.ALLY)
            { dmgInfo.MyBuffs?.Add(effect); }
            else
            { dmgInfo.EnemyBuffs?.Add(effect); }
        }

        private static void PerformEffect(ref DamageSystem dmgSys, EffectData effect)
        {
            /*Possible effects:
             Gains/Loses AP
             Gains/Loses HP
             Steals HP - redundant
             Loses Card in hand */
            
            switch (effect.EffectType)
            {
                case EFFECT_TYPE.HP_MOD:
                    dmgSys.HpMod(effect.Toyo, effect.HPValue);
                    break;
                case EFFECT_TYPE.AP_MOD:
                    dmgSys.ApMod(effect.Toyo, effect.APValue);
                    break;
                case EFFECT_TYPE.DISCARD:
                    //TODO: Make instant discard effect
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private static void CheckRemoveEffect(DamageInformation dmgInfo, EffectData effect)
        {
            //TODO: After defining the turn system by the GD, rethink treatment of effect duration.
            if (!effect.temporary || effect.timeUsed >= (effect.duration - 1))
            {                         //^ Temporary duration control ^
                Debug.Log("removeEffect: " + effect.EffectType);
                dmgInfo.MyBuffs.Remove(effect);
            }
            else if(effect.duration > effect.timeUsed)
            {
                effect.timeUsed++; //Temporary duration control
            }
        }

        public static float GetFactorInMyBuffs(DamageInformation dmgInfo, TOYO_STAT statToFind)
        {
            var _selected = new List<EffectData>();
            for (var i = dmgInfo.MyBuffs.Count - 1; i >= 0; i--)
            {
                var effect = dmgInfo.MyBuffs[i];
                if (effect.EffectType != EFFECT_TYPE.CHANGE_STAT) continue;
                if (effect.statToChange != statToFind) continue;
                _selected.Add(effect);
                CheckRemoveEffect(dmgInfo, effect);
            }

            return GetAllFactorsInEffects(_selected);
        }
        
        public static float GetFactorInEnemyBuffs(DamageInformation dmgInfo, TOYO_STAT statToFind)
        {
            var _selected = new List<EffectData>();
            foreach (var effect in dmgInfo.EnemyBuffs.Where(effect => 
                         effect.EffectType == EFFECT_TYPE.CHANGE_STAT).Where(effect => 
                         effect.statToChange == statToFind))
            {
                _selected.Add(effect);
                CheckRemoveEffect(dmgInfo, effect);
            }

            var _result = GetAllFactorsInEffects(_selected);
            Debug.Log("Factor_" + statToFind + " in EnemyBuffs: " + _result);
            return _result;
        }

        private static float GetAllFactorsInEffects(IReadOnlyList<EffectData> effectList)
        {
            if (effectList.Count <= 0) return 1;
            float _effectSum = 0;
            for (var i = effectList.Count - 1; i >= 0; i--)
            {
                var effect = effectList[i];
                _effectSum += effect.changeStatFactor;
            }

            return _effectSum;
        }

        public static float GetLifeStealFactor(DamageInformation dmgInfo)
        {
            float _totalLifeSteal = 0;
            for (var i = 0; i < dmgInfo.MyBuffs.Count; i++)
            {
                var effect = dmgInfo.MyBuffs[i];
                if (effect.EffectType != EFFECT_TYPE.CARD_MOD_LIFE_STEAL)
                {
                    continue;
                }

                _totalLifeSteal += effect.lifeStealFactor;
                CheckRemoveEffect(dmgInfo, effect);
            }
            
            return _totalLifeSteal;
        }

        public static bool GetTrueDamage(DamageInformation dmgInfo)
        {
            foreach (var effect in dmgInfo.MyBuffs.Where(effect => 
                         effect.EffectType == EFFECT_TYPE.CARD_MOD_TRUE_DAMAGE))
            {
                CheckRemoveEffect(dmgInfo, effect);
                Debug.Log("TRUE DAMAGE!");
                return true;
            }

            return false;
        }
        
        public static float GetDamageFactor(DamageInformation dmgInfo)
        {
            float _result = 1;
            for (var i = 0; i < dmgInfo.MyBuffs.Count; i++)
            {
                var effect = dmgInfo.MyBuffs[i];
                if (effect.EffectType != EFFECT_TYPE.CARD_MOD_DAMAGE) continue;
                _result += effect.nextCardDamageFactor;
                CheckRemoveEffect(dmgInfo, effect);
            }

            return _result;
        }
        
        public static int GetCostMod(DamageInformation dmgInfo)
        {
            var _result = 0;
            for (var i = dmgInfo.MyBuffs.Count - 1; i >= 0; i--)
            {
                var effect = dmgInfo.MyBuffs[i];
                if (effect.EffectType != EFFECT_TYPE.CARD_MOD_COST) continue;
                _result += effect.nextCardCostMod;
                CheckRemoveEffect(dmgInfo, effect);
            }

            return _result;
        }
    }
}
