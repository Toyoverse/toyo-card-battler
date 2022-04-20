using System;
using Card;
using ToyoSystem;

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

            var _damageFromType = dmgInfo.CardType switch
            {
                CARD_TYPE.HEAVY => CalculateHeavyCard(dmgInfo),
                CARD_TYPE.FAST => CalculateFastCard(dmgInfo),
                CARD_TYPE.DEFENSE => CalculateDefenseCard(dmgInfo),
                CARD_TYPE.BOND => CalculateBondCard(dmgInfo),
                CARD_TYPE.SUPER => CalculateSuperCard(dmgInfo),
                _ => throw new ArgumentOutOfRangeException()
            };

        }

        static float CalculateHeavyCard(DamageInformation dmgInfo)
        {
            var _value = 1 + (dmgInfo.CurrentCombo / GlobalConfig.Instance.globalCardDataSO.comboSystemFactor);
            return 0.0f;
        }
        
        static float CalculateFastCard(DamageInformation dmgInfo)
        {
            return 0.0f;
        }
        
        static float CalculateDefenseCard(DamageInformation dmgInfo)
        {
            return 0.0f;
        }
        
        static float CalculateBondCard(DamageInformation dmgInfo)
        {
            return GlobalConfig.Instance.globalCardDataSO.bondCardDamage;
        }
        
        static float CalculateSuperCard(DamageInformation dmgInfo)
        {
            return 0.0f;
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
        
    }
}