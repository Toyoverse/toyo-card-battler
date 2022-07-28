using DefaultNamespace;
using Patterns.StateMachine;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Card.CardStateMachine.States
{
    public class CardConflict : CardBaseState
    {
        public CardConflict (ICard handler, BaseStateMachine stateMachine, CardData cardData) : base(handler, stateMachine,
            cardData)
        {
        }

        public override void OnEnterState()
        {
            CardConflictAnimation();   
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