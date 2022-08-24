using System.Diagnostics;
using System.Linq;
using Card.QueueSystem;
using DefaultNamespace;
using Patterns.StateMachine;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;
namespace Card.CardStateMachine.States
{
    public class CardConflict : CardBaseState
    {
        public CardConflict (ICard handler, BaseStateMachine stateMachine,SignalBus signalBus, CardData cardData ) : base(handler, stateMachine, signalBus,
            cardData)
        {
        }
        
        private float _currentCardDuration;
        private float _attackBuffer;
        private bool _canExecuteAttack;
        private CARD_STATUS _currentStatus = CARD_STATUS.DISABLE;
        private bool IsFinalAttackBufferTime => CardData.CardDuration  - _currentCardDuration <= CardData.AttackBuffer;

        public override void OnEnterState()
        {
            _attackBuffer = CardData.AttackBuffer;
            _currentCardDuration = 0f;
            _canExecuteAttack = false;
            FireSignalOnStatusChange(CARD_STATUS.BUFFER);
            
            CardConflictAnimation();   
        }

        public override void OnUpdate()
        {
            ProcessAttackBuffer();
            
            if (_canExecuteAttack && _currentCardDuration < CardData.CardDuration)
                ValidadeCardHits();
            
            if (IsFinalAttackBufferTime)
                StartLastAttackBuffer();
            
            _currentCardDuration += Time.deltaTime;
        }
        
        private void ProcessAttackBuffer()
        {
            if (_attackBuffer <= 0)
                _canExecuteAttack = true;
            else
                _attackBuffer -= Time.deltaTime;
        }

        private void ValidadeCardHits() => FireSignalOnStatusChange(CheckForActiveHitsOnThisFrame() ? CARD_STATUS.HIT : CARD_STATUS.ACTIVE);

        private bool CheckForActiveHitsOnThisFrame() => CardData.HitListInfos.Any(hitListInfo => _currentCardDuration >= hitListInfo.SecondToHit[0] 
                                                                                                && _currentCardDuration < hitListInfo.SecondToHit[1]);

        private void StartLastAttackBuffer()
        {
            _canExecuteAttack = false;
            _attackBuffer = CardData.AttackBuffer;
            FireSignalOnStatusChange(CARD_STATUS.BUFFER);
        }

        private void FireSignalOnStatusChange(CARD_STATUS cardStatus)
        {
            if (cardStatus != _currentStatus)
            {
                if(cardStatus == CARD_STATUS.HIT)
                    Handler.PlayerHand.OnCardPlayed?.Invoke(Handler);
                
                _currentStatus = cardStatus;
                SignalBus.Fire<CardQueueSystemPresenter.UpdateCardStatusSignal>(new() 
                { 
                    IsPlayer = true, 
                    CardStatus = cardStatus
                });
            }
        }

        private void CardConflictAnimation()
        {
            //Do the card comparison animation
            OnEndConflict(true);
        }

        private void OnEndConflict(bool won)
        {
            //if(won)
        }

        public override void OnExitState()
        {
            FireSignalOnStatusChange(CARD_STATUS.DISABLE);
        }
        
        

    }
}