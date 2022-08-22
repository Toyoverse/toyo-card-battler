using DefaultNamespace;
using Patterns.StateMachine;
using UnityEngine;
using Zenject;

namespace Card.CardStateMachine.States
{
    public class CardDestroy : CardBaseState
    {
        public CardDestroy(ICard handler, BaseStateMachine stateMachine, SignalBus signalBus,CardData cardData) : base(handler,
            stateMachine, signalBus, cardData)
        {
        }

        #region Functions

        public override void OnEnterState()
        {
            BlockUsage();
            PlayDestroyAnimation();
        }

        private void PlayDestroyAnimation()
        {
            //Play animation
            OnAnimationEnd();
        }

        private void OnAnimationEnd() => Handler.Discard();

        #endregion
    }
}