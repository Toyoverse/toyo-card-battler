using DefaultNamespace;
using Patterns.StateMachine;
using UnityEngine;
using Zenject;

namespace Card.CardStateMachine.States
{
    public class CardDiscard : CardBaseState
    {
        public CardDiscard(ICard handler, BaseStateMachine stateMachine,SignalBus signalBus, CardData cardData) : base(handler,
            stateMachine, signalBus, cardData)
        {
        }

        #region Functions

        public override void OnEnterState()
        {
            BlockUsage();
            SetScale();
            SetRotation();
        }

        private void SetScale()
        {
            var _finalScale = Handler.transform.localScale * GlobalCardData.DiscardedSize;
            Handler.ScaleTo(_finalScale, GlobalCardData.ScaleSpeed);
        }

        private void SetRotation()
        {
            var _speed = Handler.IsPlayer ? GlobalCardData.RotationSpeed : GlobalCardData.RotationSpeedEnemy;
            Handler.RotateTo(Vector3.zero, _speed);
        }

        #endregion
    }
}