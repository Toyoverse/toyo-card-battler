using Patterns.StateMachine;
using UnityEngine;
using Zenject;

namespace Card.CardStateMachine.States
{
    public class CardDisable : CardBaseState
    {
        public CardDisable(ICard handler, BaseStateMachine stateMachine,SignalBus signalBus, CardData cardData) : base(handler,
            stateMachine, signalBus, cardData)
        {
        }

        private SpriteRenderer _renderer;

        public override void OnEnterState()
        {
            //Disable();
            _renderer = Handler.gameObject.GetComponentInChildren<SpriteRenderer>();
            _renderer.enabled = false;
        }

        public override void OnExitState()
        {
            _renderer.enabled = true;
        }
    }
}