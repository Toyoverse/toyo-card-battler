using Card.CardStateMachine.States;
using Patterns.StateMachine;
using UnityEngine;

namespace Card.CardStateMachine
{
    public class CardStateMachine : BaseStateMachine
    {
        public CardStateMachine(Camera camera, CardData cardData, ICard handler = null) : base(handler)
        {
            CardDataValue = cardData;
            DisableState = new CardDisable(handler, this, CardDataValue);
            DiscardState = new CardDiscard(handler, this, CardDataValue);
            DragState = new CardDrag(camera, handler, this, CardDataValue);
            HoverState = new CardHover(handler, this, CardDataValue);
            DrawState = new CardDraw(handler, this, CardDataValue);
            IdleState = new CardIdle(handler, this, CardDataValue);

            RegisterState(DisableState);
            RegisterState(DiscardState);
            RegisterState(DragState);
            RegisterState(HoverState);
            RegisterState(DrawState);
            RegisterState(IdleState);

            Initialize();
        }

        #region Properties

        private CardDisable DisableState { get; }
        private CardDiscard DiscardState { get; }
        private CardDrag DragState { get; }
        private CardHover HoverState { get; }
        private CardDraw DrawState { get; }
        private CardIdle IdleState { get; }
        private CardData CardDataValue { get; }

        #endregion

        #region Operations

        public void Disable()
        {
            PushState<CardDisable>();
        }

        public void Discard()
        {
            PushState<CardDiscard>();
        }

        public void Select()
        {
            PushState<CardDrag>();
        }

        public void Hover()
        {
            PushState<CardHover>();
        }

        public void Draw()
        {
            PushState<CardDraw>();
        }

        public void Enable()
        {
            PushState<CardIdle>();
        }

        public void Unselect()
        {
            Enable();
        }

        #endregion
    }
}