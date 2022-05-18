using System;
using System.Collections.Generic;
using APSystem;
using Card;
using HealthSystem;
using PlayerHand;
using ToyoSystem;
using UnityEngine;

namespace CombatSystem
{
    public class DamageSystem : MonoBehaviour
    {
        protected IPlayerHand CardHand { get; set; }
        protected IHealth PlayerHealth { get; set; }
        protected IHealth EnemyHealth { get; set; }
        protected IAp PlayerAP { get; set; }
        protected IAp EnemyAP { get; set; }
        protected IFullToyo FullToyo { get; set; }
        protected DamageInformation DamageInformation { get; set; }

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
            DamageInformation = new DamageInformation(_card, FullToyo);

            ProcessCardAPCost(_card);
            ProcessCardPlay(_card);
        }

        private void ProcessCardAPCost(ICard _card)
        {
            var _apCost = _card.CardData?.ApCost ?? 0;
            _apCost += BoundSystem.GetCostMod(DamageInformation);
            _apCost = _apCost < 0 ? 0 : _apCost;
            PlayerAP?.OnUseAP.Invoke(_apCost);
        }

        private void ProcessCardPlay(ICard card)
        {
            switch (card.CardData.CardType)
            {
                case CARD_TYPE.HEAVY or CARD_TYPE.FAST or CARD_TYPE.SUPER:
                    AttackCardProcess(card);
                    break;
                case CARD_TYPE.DEFENSE:
                    DefenseCardProcess();
                    break;
                case CARD_TYPE.BOND:
                    BoundSystem.BoundCardProcess(DamageInformation, HpMod, ApMod);
                    break;
                default: throw new ArgumentOutOfRangeException();
            }
        }

        private void AttackCardProcess(ICard card)
        {
            var _hitListInfos = card.CardData?.HitListInfos;
            if (!(_hitListInfos?.Count > 0)) return;

            if (GlobalConfig.Instance.IgnoreDamageCalculations)
            {
                foreach (var t in _hitListInfos)
                    DoDamage(t.Damage);
            }

            //TODO: Consider hit time in the animation.
            var _damage = DamageCalculation.CalculateDamage(DamageInformation);
            for (var i = 0; i < _damage.Length; i++)
            {
                _damage[i] *= BoundSystem.GetDamageFactor(DamageInformation);
                if (DefenseSystem.NaturalEnemyDefense(DamageInformation))
                {
                    if (DefenseSystem.NaturalEnemyCounter(DamageInformation))
                        ApplyEnemyCounter();
                }
                else
                {
                    DoDamage(_damage[i]);
                    CheckLifeSteal(_damage[i]);
                }
            }
        }

        private void DefenseCardProcess()
        {
            if (DefenseSystem.DefenseCardSuccess(DamageInformation))
            {
                //TODO: Send defense success to the Enemy and interrupt if his attack is a fast attack
                //TODO: If the enemy has a stronger card than the defense cards, don't check the counter-attack
                if (DefenseSystem.CounterCardSuccess(DamageInformation))
                    ApplyMyCounter();
            }
            else
            {
                //TODO: UX for Defense fail
                Debug.Log("Defense fail");
            }
        }

        private void ApplyEnemyCounter()
        {
            //TODO: Apply enemy counter attack
        }

        private void ApplyMyCounter()
        {
            //TODO: Get fast attack card and Execute the counterattack - Start new combo
        }

        private void DoDamage(/*HitListInfo hit*/ float damage)
            => EnemyHealth?.OnTakeDamage.Invoke(/*hit.Damage*/ damage);

        private void CheckLifeSteal(float damage)
        {
            var _lifeStealFactor = BoundSystem.GetLifeStealFactor(DamageInformation);
            if (!(_lifeStealFactor > 0)) return;
            var _hpMod = damage * _lifeStealFactor;
            PlayerHealth?.OnGainHP.Invoke(_hpMod);
        }

        public void HpMod(TOYO_TYPE toyo, float sumValue)
        {
            if (toyo == TOYO_TYPE.ALLY)
                PlayerHealth?.OnChangeHP.Invoke(sumValue); 
            else
                EnemyHealth?.OnChangeHP.Invoke(sumValue); 
        }

        public void ApMod(TOYO_TYPE toyo, float sumValue)
        {
            if (toyo == TOYO_TYPE.ALLY)
                PlayerAP?.OnChangeAP.Invoke((int)sumValue); 
            else
                EnemyAP?.OnChangeAP.Invoke((int)sumValue); 
        }
    }
    
    public struct DamageInformation
    {
        public List<HitListInfo> HitVariation;
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

        public DamageInformation(ICard card, IFullToyo fullToyo)
        {
            HitVariation = card.CardData.HitListInfos;
            CardType = card.CardData.CardType;
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

