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
        
        private float _cardDuration;
        private float _attackBuffer;
        private bool _canExecuteAttack;
        private CARD_STATUS _currentStatus = CARD_STATUS.BUFFER;

        public override void OnEnterState()
        {
            _attackBuffer = CardData.AttackBuffer;
            _cardDuration = 0f;
            _canExecuteAttack = false;
            FireSignalOnStatusChange(CARD_STATUS.BUFFER);
            
            CardConflictAnimation();   
        }

        public override void OnUpdate()
        {
            if (_attackBuffer <= 0)
                _canExecuteAttack = true;
            else
                _attackBuffer -= Time.deltaTime;

            if (_canExecuteAttack)
            {
                if (_cardDuration < CardData.CardDuration)
                {
                    _cardDuration += Time.deltaTime;

                    foreach (var _hitListInfo in CardData.HitListInfos)
                    {
                        if (_hitListInfo.SecondToHit[0] >= _cardDuration && _hitListInfo.SecondToHit[1] < _cardDuration)
                            FireSignalOnStatusChange(CARD_STATUS.HIT);
                        else
                            FireSignalOnStatusChange(CARD_STATUS.ACTIVE);
                    }
                }
            }

        }

        private void FireSignalOnStatusChange(CARD_STATUS cardStatus)
        {
            if (cardStatus != _currentStatus)
            {
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
        }
        
        

    }
}