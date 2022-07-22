using System;
using System.Collections.Generic;
using Card;
using CombatSystem.APSystem;
using HealthSystem;
using Player;
using CardSystem.PlayerHand;
using ToyoSystem;
using UnityEngine;
using Zenject;

namespace CombatSystem
{
    public class DamageSystem : MonoBehaviour
    {
        

        
        protected HealthModel PlayerHealthModel => _playerNetworkManager.GetLocalPlayer().MyPlayerHealthModel;
        protected HealthModel EnemyHealthModel => _playerNetworkManager.GetEnemy().MyPlayerHealthModel;

        [Inject(Id = "PlayerAP")] private ApModel _playerApModel;
        [Inject(Id = "EnemyAP")] private ApModel _enemyApModel;
        private IFullToyo _fullToyo;
        private IPlayerHand _cardHand;
        private DamageInformation _damageInformation;
        private SignalBus _signalBus;
        private PlayerNetworkManager _playerNetworkManager;
        
        [Inject]
        public void Construct(IPlayerHand playerHand, IFullToyo fullToyo, SignalBus signalBus)
        {
            _fullToyo = fullToyo;
            _cardHand = playerHand;
            _signalBus = signalBus;
            _signalBus.Subscribe<PlayerNetworkInitializedSignal>(x => _playerNetworkManager = x.PlayerNetworkManager);
        }
        
        void OnEnable()
        {
            _cardHand.OnCardPlayed += ProcessCardPlayed;
            _cardHand.OnNetworkCardPlayed += ProcessNetworkCardPlayed;
        }

        private void OnDisable()
        {
            _cardHand.OnCardPlayed -= ProcessCardPlayed;
            _cardHand.OnNetworkCardPlayed += ProcessNetworkCardPlayed;
        }

        public void ProcessCardPlayed(ICard _card)
        {
            _damageInformation = new DamageInformation(_card, _fullToyo);
            ProcessCardAPCost(_card);
            ProcessCardByType(_card);
        }
        
        public void ProcessNetworkCardPlayed(ICard _card)
        {
            _damageInformation = new DamageInformation(_card, _fullToyo, true);
            ProcessCardByType(_card);
        }

        private void ProcessCardAPCost(ICard _card)
        {
            var _apCost = _card.CardData?.ApCost ?? 0;
            _apCost += BoundSystem.GetCostMod(_damageInformation);
            _apCost = _apCost < 0 ? 0 : _apCost;
            _playerApModel?.OnUseAp.Invoke(_apCost);
        }

        private void ProcessCardByType(ICard card)
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
                    BoundSystem.BoundCardProcess(_damageInformation, HpMod, ApMod);
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

                return;
            }

            //TODO: Consider hit time in the animation.
            var _damage = DamageCalculation.CalculateDamage(_damageInformation);
            for (var i = 0; i < _damage.Length; i++)
            {
                _damage[i] *= BoundSystem.GetDamageFactor(_damageInformation);
                if (DefenseSystem.NaturalEnemyDefense(_damageInformation))
                {
                    if (DefenseSystem.NaturalEnemyCounter(_damageInformation))
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
            if (DefenseSystem.DefenseCardSuccess(_damageInformation))
            {
                //TODO: Send defense success to the Enemy and interrupt if his attack is a fast attack
                //TODO: If the enemy has a stronger card than the defense cards, don't check the counter-attack
                if (DefenseSystem.CounterCardSuccess(_damageInformation))
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

        private void DoDamage(float damage)
        {
            if(_damageInformation.IsMultiplayerEnemyCard)
                PlayerHealthModel?.OnTakeDamage.Invoke(damage);
            else
                EnemyHealthModel?.OnTakeDamage.Invoke(damage);
        }
        
        private void CheckLifeSteal(float damage)
        {
            var _lifeStealFactor = BoundSystem.GetLifeStealFactor(_damageInformation);
            if (!(_lifeStealFactor > 0)) return;
            var _hpMod = damage * _lifeStealFactor;
            PlayerHealthModel?.OnGainHp.Invoke(_hpMod);
        }

        public void HpMod(TOYO_TYPE toyo, float sumValue)
        {
            if (toyo == TOYO_TYPE.ALLY)
                PlayerHealthModel?.OnChangeHp.Invoke(sumValue); 
            else
                EnemyHealthModel?.OnChangeHp.Invoke(sumValue); 
        }

        public void ApMod(TOYO_TYPE toyo, float sumValue)
        {
            if (toyo == TOYO_TYPE.ALLY)
                _playerApModel?.OnChangeAp.Invoke((int)sumValue); 
            else
                _enemyApModel?.OnChangeAp.Invoke((int)sumValue); 
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
        public List<EffectData> MyBuffs;
        public List<EffectData> EnemyBuffs;
        public EffectData[] EffectData;
        public bool IsMultiplayerEnemyCard; //Information for the host

        public DamageInformation(ICard card, IFullToyo fullToyo, bool isMultiplayerEnemyCard = false)
        {
            HitVariation = card.CardData.HitListInfos ?? new List<HitListInfo>(); 
            CardType = card.CardData.CardType;
            AttackType = card.CardData.AttackType;
            AttackSubType = card.CardData.AttackSubType;
            DefenseType = card.CardData.DefenseType;
            ToyoStats = fullToyo.ToyoStats;
            EnemyToyoStats = fullToyo.ToyoStats; //TODO: Get Enemy Toyo Stats
            MyBuffs = fullToyo.Buffs;
            EnemyBuffs = fullToyo.Buffs; //TODO: Get Enemy Toyo Buffs
            EffectData = card.CardData.EffectData;
            IsMultiplayerEnemyCard = isMultiplayerEnemyCard;
        }
    }
}

