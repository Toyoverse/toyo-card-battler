using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Android;

namespace CombatSystem
{
    public class BoundSystem : MonoBehaviour
    {
        public static void BoundEffectApply(DamageInformation dmgInfo)
        {
            AddEffect(dmgInfo);
            if (dmgInfo.EffectData.temporary)
            {
                //TODO: Treat case of temporary effect.
            }
        }

        private static void AddEffect(DamageInformation dmgInfo)
        {
            if (dmgInfo.EffectData != null)
            {
                dmgInfo.MyBuffs?.Add(dmgInfo.EffectData);
            }
        }

        private static void RemoveEffect(DamageInformation dmgInfo, EffectData effect)
        {
            if (!effect.temporary)
            {
                dmgInfo.MyBuffs.Remove(effect);
            }
            else
            {
                //TODO: Treat case of temporary effect.
            }
        }

        public static float GetFactorInMyBuffs(DamageInformation dmgInfo, TOYO_STAT statToFind)
        {
            var selected = new List<EffectData>();
            foreach (var effect in dmgInfo.MyBuffs.Where(effect => 
                         effect.EffectType == EFFECT_TYPE.CHANGE_STAT).Where(effect => 
                         effect.statToChange == statToFind))
            {
                selected.Add(effect);
                RemoveEffect(dmgInfo, effect);
            }

            return GetAllFactorsInEffects(selected);
        }
        
        public static float GetFactorInEnemyBuffs(DamageInformation dmgInfo, TOYO_STAT statToFind)
        {
            var selected = new List<EffectData>();
            foreach (var effect in dmgInfo.EnemyBuffs.Where(effect => 
                         effect.EffectType == EFFECT_TYPE.CHANGE_STAT).Where(effect => 
                         effect.statToChange == statToFind))
            {
                selected.Add(effect);
                RemoveEffect(dmgInfo, effect);
            }

            return GetAllFactorsInEffects(selected);
        }

        private static float GetAllFactorsInEffects(List<EffectData> effectList)
        {
            if (effectList.Count > 0)
            {
                float effectSum = 0;
                foreach (var effect in effectList)
                {
                    effectSum += effect.changeStatFactor;
                }

                return effectSum;
            }
            return 1;
        }
    }
}
