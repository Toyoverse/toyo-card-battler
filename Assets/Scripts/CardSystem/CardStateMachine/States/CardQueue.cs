using DefaultNamespace;
using Patterns.StateMachine;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace Card.CardStateMachine.States
{
    public class CardQueue : CardBaseState
    {
        public CardQueue(ICard handler, BaseStateMachine stateMachine, SignalBus signalBus, CardData cardData) : base(handler, stateMachine, signalBus,
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