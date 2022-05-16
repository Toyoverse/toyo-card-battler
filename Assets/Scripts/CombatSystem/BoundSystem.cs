using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Card;
using ToyoSystem;
using UnityEngine;

namespace CombatSystem
{
    public static class BoundSystem 
    {
        public static void BoundCardProcess(DamageInformation dmgInfo, Action<TOYO_TYPE, float> callbackHP = null,
            Action<TOYO_TYPE, float> callbackAP = null)
        {
            if (dmgInfo.EffectData == null)
                return;
            foreach (var effect in dmgInfo.EffectData)
            {
                if (effect == null)
                    continue; 
                if (effect.temporary) 
                    AddEffect(dmgInfo, effect); 
                else 
                    PerformEffect(effect.EffectType == EFFECT_TYPE.HP_MOD 
                        ? callbackHP : callbackAP, effect); 
            }
        }
        
        private static void AddEffect(DamageInformation dmgInfo, EffectData effect)
        {
            if (effect.Toyo == TOYO_TYPE.ALLY)
                dmgInfo.MyBuffs?.Add(effect); 
            else
                dmgInfo.EnemyBuffs?.Add(effect); 
        }

        private static void PerformEffect(Action<TOYO_TYPE, float> callback, EffectData effect)
        {
            switch (effect.EffectType)
            {
                case EFFECT_TYPE.HP_MOD:
                    callback?.Invoke(effect.Toyo, effect.HPValue);
                    break;
                case EFFECT_TYPE.AP_MOD:
                    callback?.Invoke(effect.Toyo, effect.APValue);
                    break;
                case EFFECT_TYPE.DISCARD:
                    DiscardRandomCard();
                    break;
            }
        }
        
        private static void DiscardRandomCard()
        {
            //TODO: Make instant discard effect
        }

        public static void CheckRemoveEffect(ref List<EffectData> effectDataList, EffectData effect)
        {
            //TODO: After defining the turn system by the GD, rethink treatment of effect duration.
            effectDataList.Remove(effect);
        }

        public static float GetMultiplierInMyBuffs(DamageInformation dmgInfo, TOYO_STAT statToFind)
            => GetFactorInBuffList(dmgInfo.MyBuffs, statToFind);
        
        public static float GetFactorInEnemyBuffs(DamageInformation dmgInfo, TOYO_STAT statToFind)
            => GetFactorInBuffList(dmgInfo.EnemyBuffs, statToFind);

        private static float GetFactorInBuffList(List<EffectData> buffList, TOYO_STAT statToFind)
        {
            var _selectedEffect = new List<EffectData>();
            for (var i = 0; i < buffList.Count; i++)
            {
                var effect = buffList[i];
                if (effect.EffectType != EFFECT_TYPE.CHANGE_STAT) continue;
                if (effect.statToChange != statToFind) continue;
                _selectedEffect.Add(effect);
                CheckRemoveEffect(ref buffList, effect);
            }

            var _result = GetAllFactorsInEffects(_selectedEffect);
            return _result;
        }

        private static float GetAllFactorsInEffects(IReadOnlyList<EffectData> effectList)
        {
            if (effectList.Count <= 0) return 1;
            float _effectSum = 0;
            for (var i = 0; i < effectList.Count; i++)
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
                    continue;
                _totalLifeSteal += effect.lifeStealFactor;
                CheckRemoveEffect(ref dmgInfo.MyBuffs, effect);
            }
            
            return _totalLifeSteal;
        }

        public static bool IsTrueDamage(DamageInformation dmgInfo)
        {
            for (var i = 0; i < dmgInfo.MyBuffs.Count; i++)
            {
                var effect = dmgInfo.MyBuffs[i];
                if (effect.EffectType != EFFECT_TYPE.CARD_MOD_TRUE_DAMAGE) continue;
                CheckRemoveEffect(ref dmgInfo.MyBuffs, effect);
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
                CheckRemoveEffect(ref dmgInfo.MyBuffs, effect);
            }

            return _result;
        }
        
        public static int GetCostMod(DamageInformation dmgInfo)
        {
            var _result = 0;
            for (var i = 0; i < dmgInfo.MyBuffs.Count; i++)
            {
                var effect = dmgInfo.MyBuffs[i];
                if (effect.EffectType != EFFECT_TYPE.CARD_MOD_COST) continue;
                _result += effect.nextCardCostMod;
                CheckRemoveEffect(ref dmgInfo.MyBuffs, effect);
            }

            return _result;
        }
    }
}
