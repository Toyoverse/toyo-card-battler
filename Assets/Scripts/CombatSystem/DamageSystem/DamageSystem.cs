using System;
using Card;
using HealthSystem;
using PlayerHand;
using Tools;
using UnityEngine;

namespace CombatSystem.DamageSystem
{
    public class DamageSystem : MonoBehaviour
    {
        protected IPlayerHand CardHand { get; set; }
        protected IHealth PlayerHealth { get; set; }

        protected virtual void Awake()
        {
            CardHand = GlobalConfig.Instance.PlayerReferences.hand.GetComponent<IPlayerHand>();
            PlayerHealth = GlobalConfig.Instance.UI.GetComponentInChildren<IHealth>();
        }

        void OnEnable()
        {
            CardHand.OnCardPlayed += ProcessCardDamage;
        }

        private void OnDisable()
        {
            CardHand.OnCardPlayed -= ProcessCardDamage;
        }

        void ProcessCardDamage(ICard card)
        {
            var _hitListInfos = card.CardData?.HitListInfos;

            if (_hitListInfos?.Count > 0)
                foreach (var _hit in _hitListInfos)
                    DoDamage(_hit);
                    
        }

        void DoDamage(HitListInfo hit)
        {
            PlayerHealth?.OnTakeDamage.Invoke(hit.Damage);
        }
    }
}