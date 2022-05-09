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
        
        protected IHealth EnemyHealth { get; set; }
        
        protected IAp PlayerAP{ get; set; }
        protected IFullToyo FullToyo{ get; set; }

        private void Start()
        {
            CardHand = GlobalConfig.Instance.battleReferences.hand.GetComponent<IPlayerHand>();
            PlayerHealth = GlobalConfig.Instance.battleReferences.PlayerUI.GetComponentInChildren<IHealth>();
            EnemyHealth = GlobalConfig.Instance.battleReferences.EnemyUI.GetComponentInChildren<IHealth>();
            PlayerAP = GlobalConfig.Instance.battleReferences.PlayerUI.GetComponentInChildren<IAp>();
            FullToyo = GlobalConfig.Instance.battleReferences.Toyo.GetComponent<IFullToyo>();
            CardHand.OnCardPlayed += ProcesCardPlayed;
        }

        void OnEnable()
        {
            if(CardHand?.OnCardPlayed != null)
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
                for (var index = 0; index < _hitListInfos.Count; index++)
                {
                    var _hit = _hitListInfos[index];
                    /*DamageInformation _dmgInfo = new DamageInformation(card, FullToyo, index);
                    DamageCalculation.CalculateDamage(_dmgInfo);*/
                    DoDamage(_hit);
                }
        }

        void DoDamage(HitListInfo hit)
        {
            EnemyHealth?.OnTakeDamage.Invoke(hit.Damage);
        }
        
    }
    
    public struct DamageInformation
    {
        public float HitVariation;
        public ATTACK_TYPE AttackType;
        public ATTACK_SUB_TYPE AttackSubType;
        public CARD_TYPE CardType;
        public DEFENSE_TYPE DefenseType;
        public Dictionary<TOYO_STAT, float> ToyoStats;
        public Dictionary<TOYO_STAT, float> EnemyToyoStats;
        public int CurrentCombo;

        public DamageInformation(ICard card, IFullToyo fullToyo, int hitIndex)
        {
            HitVariation = card.CardData.HitListInfos[hitIndex].Damage;
            CardType = card.CardData.Cardtype;
            AttackType = card.CardData.AttackType;
            AttackSubType = card.CardData.AttackSubType;
            DefenseType = card.CardData.DefenseType;
            ToyoStats = fullToyo.ToyoStats;
            EnemyToyoStats = fullToyo.ToyoStats; //TODO: Get Enemy Toyo Stats
            CurrentCombo = 1; //Todo Implement Combo System
        }
    }
}

