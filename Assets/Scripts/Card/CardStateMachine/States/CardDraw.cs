using Patterns.StateMachine;
using UnityEngine;

namespace Card.CardStateMachine.States
{
    public class CardDraw : CardBaseState
    {
        public CardDraw(ICard handler, BaseStateMachine stateMachine, CardData cardData) : base(handler, stateMachine, cardData)
        {
        }
        
        Vector3 StartScale { get; set; }
        #region Operations

        public override void OnEnterState()
        {
            CachePreviousValue();
            DisableCollision();
            SetScale();
            Handler.Movement.OnFinishMotion += GoToIdle;
        }

        public override void OnExitState() => Handler.Movement.OnFinishMotion -= GoToIdle;

        void GoToIdle() => Handler.Enable();

        void CachePreviousValue()
        {
            StartScale = Handler.transform.localScale;
            Handler.transform.localScale *= StartSizeWhenDraw;
        }

        void SetScale() => Handler.ScaleTo(StartScale, ScaleSpeed);

        #endregion
        
    }
}