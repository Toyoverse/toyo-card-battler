using System;
using System.Collections.Generic;
using APSystem;
using Card;
using HealthSystem;
using PlayerHand;
using Tools;
using ToyoSystem;
using UnityEngine;

namespace CombatSystem
{
    public class DamageSystem : MonoBehaviour
    {
        protected IPlayerHand CardHand { get; set; }
        protected IHealth PlayerHealth { get; set; }
        protected IHealth EnemyHealth { get; set; }
        protected IAp PlayerAP{ get; set; }
        protected IAp EnemyAP{ get; set; }
        protected IFullToyo FullToyo{ get; set; }

        private void Start()
        {
            CardHand = GlobalConfig.Instance.battleReferences.hand.GetComponent<IPlayerHand>();
            PlayerHealth = GlobalConfig.Instance.battleReferences.PlayerUI.GetComponentInChildren<IHealth>();
            EnemyHealth = GlobalConfig.Instance.battleReferences.EnemyUI.GetComponentInChildren<IHealth>();
            PlayerAP = GlobalConfig.Instance.battleReferences.PlayerUI.GetComponentInChildren<IAp>();
            EnemyAP = GlobalConfig.Instance.battleReferences.EnemyUI.GetComponentInChildren<IAp>();
            FullToyo = GlobalConfig.Instance.battleReferences.Toyo.GetComponent<IFullToyo>();
            CardHand.OnCardPlayed += ProcessCardPlayed;
        }

        void OnEnable()
        {
            if(CardHand?.OnCardPlayed != null)
                CardHand.OnCardPlayed += ProcessCardPlayed;
        }

        private void OnDisable()
        {
            CardHand.OnCardPlayed -= ProcessCardPlayed;
        }

        private void ProcessCardPlayed(ICard _card)
        {
            var _dmgInfo = new DamageInformation(_card, FullToyo);
            if (!BoundSystem.ICanPlayThisCard(_dmgInfo)) { return; }
            
            ProcessCardDamage(_card);
            ProcessCardAPCost(_card);
        }

        private void ProcessCardAPCost(ICard _card)
        {
            var _apCost = _card.CardData?.ApCost ?? 0;
            var _dmgInfo = new DamageInformation(_card, FullToyo);
            _apCost += BoundSystem.GetCostMod(_dmgInfo);
            PlayerAP?.OnUseAP.Invoke(_apCost);
        }

        private void ProcessCardDamage(ICard card)
        {
            switch (card.CardData.Cardtype)
            {
                case CARD_TYPE.HEAVY or CARD_TYPE.FAST or CARD_TYPE.SUPER:
                    var _hitListInfos = card.CardData?.HitListInfos;
                    if (_hitListInfos?.Count > 0)
                    {
                        for (var index = 0; index < _hitListInfos.Count; index++)
                        {
                            //TODO: Consider hit time in the animation.
                            var _dmgInfo = new DamageInformation(card, FullToyo, index);
                            var _damage = DamageCalculation.CalculateDamage(_dmgInfo);
                            _damage *= BoundSystem.GetDamageFactor(_dmgInfo);
                            DoDamage(_damage);
                            CheckLifeSteal(_dmgInfo, _damage);
                        }
                    }
                    break;
                case CARD_TYPE.DEFENSE:
                    var _dmgInfo1 = new DamageInformation(card, FullToyo);
                    if (DefenseSystem.DefenseSuccess(_dmgInfo1))
                    {
                        //TODO: Send defense success to the Enemy and interrupt if his attack is a fast attack
                        //TODO: If the enemy has a stronger card than the defense cards, don't check the counter-attack
                        if (DefenseSystem.CounterSuccess(_dmgInfo1))
                        {
                            //TODO: Get fast attack card and Execute the counterattack - Start new combo
                        }
                    }
                    else
                    {
                        //TODO: UX for Defense fail
                        Debug.Log("Defense fail");
                    }
                    break;
                case CARD_TYPE.BOND:
                    var _dmgInfo2 = new DamageInformation(card, FullToyo);
                    var damageSystem = this;
                    BoundSystem.BoundProcess(_dmgInfo2, ref damageSystem);
                    break;
                default: throw new ArgumentOutOfRangeException();
            }
        }

        private void DoDamage(/*HitListInfo hit*/ float damage)
        {
            EnemyHealth?.OnTakeDamage.Invoke(/*hit.Damage*/ damage);
        }

        private void CheckLifeSteal(DamageInformation dmgInfo, float damage)
        {
            var _lifeStealFactor = BoundSystem.GetLifeStealFactor(dmgInfo);
            if (!(_lifeStealFactor > 0)) return;
            var _hpMod = damage * _lifeStealFactor;
            PlayerHealth?.OnGainHP.Invoke(_hpMod);
        }

        public void HpMod(TOYO_TYPE toyo, float sumValue)
        {
            if(sumValue == 0) { return; }

            if (sumValue > 0)
            {
                if (toyo == TOYO_TYPE.ALLY)
                { PlayerHealth?.OnGainHP.Invoke(sumValue); }
                else
                { EnemyHealth?.OnGainHP.Invoke(sumValue); }
            }
            else
            {
                if (toyo == TOYO_TYPE.ALLY)
                { PlayerHealth?.OnTakeDamage.Invoke(Math.Abs(sumValue)); }
                else
                { EnemyHealth?.OnTakeDamage.Invoke(Math.Abs(sumValue)); }
            }
        }
        
        public void ApMod(TOYO_TYPE toyo, int sumValue)
        {
            if(sumValue == 0) { return; }

            if (sumValue > 0)
            {
                if (toyo == TOYO_TYPE.ALLY)
                { PlayerAP?.OnGainAP.Invoke(sumValue); }
                else
                { EnemyAP?.OnGainAP.Invoke(sumValue); }
            }
            else
            {
                if (toyo == TOYO_TYPE.ALLY)
                { PlayerAP?.OnUseAP.Invoke(Math.Abs(sumValue)); }
                else
                { EnemyAP?.OnUseAP.Invoke(Math.Abs(sumValue)); }
            }
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
        public List<EffectData> MyBuffs;
        public List<EffectData> EnemyBuffs;
        public EffectData[] EffectData;

        public DamageInformation(ICard card, IFullToyo fullToyo, int hitIndex = -1)
        {
            HitVariation = hitIndex >= 0 ? card.CardData.HitListInfos[hitIndex].Damage : 0;
            CardType = card.CardData.Cardtype;
            AttackType = card.CardData.AttackType;
            AttackSubType = card.CardData.AttackSubType;
            DefenseType = card.CardData.DefenseType;
            ToyoStats = fullToyo.ToyoStats;
            EnemyToyoStats = fullToyo.ToyoStats; //TODO: Get Enemy Toyo Stats
            CurrentCombo = 1; //Todo Implement Combo System
            MyBuffs = fullToyo.Buffs;
            EnemyBuffs = fullToyo.Buffs; //TODO: Get Enemy Toyo Buffs
            EffectData = card.CardData.EffectData;
        }
    }
}

