using Card.CardStateMachine.States;
using Patterns.StateMachine;
using UnityEngine;
using Zenject;

namespace Card.CardStateMachine
{
    public class CardStateMachine : BaseStateMachine
    {
        public CardStateMachine(Camera camera, CardData cardData, SignalBus signalBus, ICard handler = null) : base(handler)
        {
            CardDataValue = cardData;
            DisableState = new CardDisable(handler, this, signalBus, CardDataValue);
            DiscardState = new CardDiscard(handler, this, signalBus,  CardDataValue);
            DragState = new CardDrag(camera, handler, this, signalBus,  CardDataValue);
            HoverState = new CardHover(handler, this, signalBus,  CardDataValue);
            DrawState = new CardDraw(handler, this, signalBus,  CardDataValue);
            IdleState = new CardIdle(handler, this, signalBus,  CardDataValue);
            QueueState = new CardQueue(handler, this, signalBus,  CardDataValue);
            DestroyState = new CardDestroy(handler, this, signalBus,  CardDataValue);
            ConflictState = new CardConflict(handler, this, signalBus,  CardDataValue);
            BlockState = new CardBlock(handler, this, signalBus,  CardDataValue);

            RegisterState(DisableState);
            RegisterState(DiscardState);
            RegisterState(DragState);
            RegisterState(HoverState);
            RegisterState(DrawState);
            RegisterState(IdleState);
            RegisterState(QueueState);
            RegisterState(DestroyState);
            RegisterState(ConflictState);
            RegisterState(BlockState);

            Initialize();
        }

        #region Properties

        private CardDisable DisableState { get; }
        private CardDiscard DiscardState { get; }
        private CardDrag DragState { get; }
        private CardHover HoverState { get; }
        private CardDraw DrawState { get; }
        private CardIdle IdleState { get; }
        private CardQueue QueueState { get; }
        private CardDestroy DestroyState { get; }
        private CardConflict ConflictState { get; }
        private CardBlock BlockState { get; }
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

        public void Conflict()
        {
            PushState<CardConflict>();
        }

        public void Queue()
        {
            PushState<CardQueue>();
        }

        public void PlayDestroyAnimation()
        {
            PushState<CardDestroy>();
        }

        public void Unselect()
        {
            Enable();
        }
        
        public void BlockUsage()
        {
            PushState<CardBlock>();
        }

        #endregion
    }
}