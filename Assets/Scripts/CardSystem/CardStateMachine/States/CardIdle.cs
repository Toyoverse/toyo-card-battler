using DefaultNamespace;
using Patterns.StateMachine;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Card.CardStateMachine.States
{
    public class CardIdle : CardBaseState
    {
        public CardIdle(ICard handler, BaseStateMachine stateMachine, CardData cardData) : base(handler, stateMachine,
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
                Handler.Movement.OnFinishMotion += Enable;
            }
            else
            {
                Enable();
            }

            MakeRenderNormal();
            Handler.ScaleTo(DefaultSize, GlobalCardData.ScaleSpeed);
        }

        public override void OnExitState()
        {
            Handler.Input.OnPointerDown -= OnPointerDown;
            Handler.Input.OnPointerEnter -= OnPointerEnter;
            Handler.Movement.OnFinishMotion -= Enable;
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