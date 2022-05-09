using System.Collections;
using System.Collections.Generic;
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

        static void AddEffect(DamageInformation dmgInfo)
        {
            dmgInfo.Buffs.Add(dmgInfo.EffectData);
        }
        
        public static void RemoveEffect(DamageInformation dmgInfo, EffectData effect)
        {
            if (!effect.temporary)
            {
                dmgInfo.Buffs.Remove(effect);
            }
            else
            {
                //TODO: Treat case of temporary effect.
            }
        }
    }
}
