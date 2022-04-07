using DefaultNamespace;
using Patterns.StateMachine;
using UnityEngine;

namespace Card.CardStateMachine.States
{
    public class CardDraw : CardBaseState
    {
        public CardDraw(ICard handler, BaseStateMachine stateMachine, CardData cardData) : base(handler, stateMachine,
            cardData)
        {
        }

        private Vector3 StartScale { get; set; }

        #region Operations

        public override void OnEnterState()
        {
            CachePreviousValue();
            DisableCollision();
            SetScale();
            Handler.Movement.OnFinishMotion += GoToIdle;
        }

        public override void OnExitState()
        {
            Handler.Movement.OnFinishMotion -= GoToIdle;
        }

        private void GoToIdle()
        {
            Handler.Enable();
        }

        private void CachePreviousValue()
        {
            StartScale = Handler.transform.localScale;
            Handler.transform.localScale *= GlobalCardData.StartSizeWhenDraw;
        }

        private void SetScale()
        {
            Handler.ScaleTo(StartScale, GlobalCardData.ScaleSpeed);
        }

        #endregion
    }
}