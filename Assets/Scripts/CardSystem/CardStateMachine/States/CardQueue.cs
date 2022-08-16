using DefaultNamespace;
using Patterns.StateMachine;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Card.CardStateMachine.States
{
    public class CardQueue : CardBaseState
    {
        public CardQueue(ICard handler, BaseStateMachine stateMachine, CardData cardData) : base(handler, stateMachine,
            cardData)
        {
        }

        public override void OnEnterState()
        {
        }

        public override void OnExitState()
        {
        }

    }
}