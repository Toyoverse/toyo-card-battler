using DefaultNamespace;
using Patterns.StateMachine;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace Card.CardStateMachine.States
{
    public class CardIdle : CardBaseState
    {
        public CardIdle(ICard handler, BaseStateMachine stateMachine, SignalBus signalBus, CardData cardData) : base(handler, stateMachine, signalBus,
            cardData)
        {
            DefaultSize = Handler.transform.localScale;
        }

        private Vector3 DefaultSize { get; }

        public override void OnEnterState()
        {
            Handler.Input.OnPointerDown += OnPointerDown;
            Handler.Input.OnPointerEnter += OnPointerEnter;

            if (Handler.Movement.IsOperating)
            {
                DisableCollision();
                Handler.Movement.OnFinishMotion += EnableUsage;
            }
            else
            {
                EnableUsage();
            }

            Handler.ScaleTo(DefaultSize, GlobalCardData.ScaleSpeed);
        }

        public override void OnExitState()
        {
            Handler.Input.OnPointerDown -= OnPointerDown;
            Handler.Input.OnPointerEnter -= OnPointerEnter;
            Handler.Movement.OnFinishMotion -= EnableUsage;
        }

        private void OnPointerEnter(PointerEventData obj)
        {
            if (StateMachine.IsCurrent(this))
                Handler.Hover();
        }

        private void OnPointerDown(PointerEventData eventData)
        {
            if (StateMachine.IsCurrent(this))
                Handler.Select();
        }
    }
}