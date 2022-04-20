using System;
using System.Collections.Generic;
using APSystem;
using Card;
using HealthSystem;
using PlayerHand;
using Tools;
using ToyoSystem;
using UnityEngine;

namespace CombatSystem.DamageSystem
{
    public class DamageSystem : MonoBehaviour
    {
        protected IPlayerHand CardHand { get; set; }
        protected IHealth PlayerHealth { get; set; }
        protected IAp PlayerAP{ get; set; }
        protected IFullToyo FullToyo{ get; set; }

        protected virtual void Awake()
        {
            CardHand = GlobalConfig.Instance.PlayerReferences.hand.GetComponent<IPlayerHand>();
            PlayerHealth = GlobalConfig.Instance.UI.GetComponentInChildren<IHealth>();
            PlayerAP = GlobalConfig.Instance.UI.GetComponentInChildren<IAp>();
            FullToyo = GlobalConfig.Instance.PlayerReferences.Toyo.GetComponent<IFullToyo>();
        }

        void OnEnable()
        {
            CardHand.OnCardPlayed += ProcesCardPlayed;
        }

        private void OnDisable()
        {
            CardHand.OnCardPlayed -= ProcesCardPlayed;
        }

        void ProcesCardPlayed(ICard _card)
        {
            ProcessCardDamage(_card);
            ProcessCardAPCost(_card);
        }
        
        void ProcessCardAPCost(ICard _card)
        {
            int _apCost = _card.CardData?.ApCost ?? 0;
            PlayerAP?.OnUseAP.Invoke(_apCost);
                    
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
    
    public struct DamageInformation
    {
        public float HitVariation;
        public ATTACK_TYPE AttackType;
        public ATTACK_SUB_TYPE AttackSubType;
        public CARD_TYPE CardType;
        public Dictionary<TOYO_STAT, float> ToyoStats;
        public int CurrentCombo;

        public DamageInformation(ICard card, IFullToyo fullToyo, int hitIndex)
        {
            HitVariation = card.CardData.HitListInfos[hitIndex].Damage;
            CardType = card.CardData.Cardtype;
            AttackType = card.CardData.AttackType;
            AttackSubType = card.CardData.AttackSubType;
            ToyoStats = fullToyo.ToyoStats;
            CurrentCombo = 1; //Todo Implement Combo System
        }
    }
}

