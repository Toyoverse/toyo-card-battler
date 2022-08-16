using DefaultNamespace;
using Patterns.StateMachine;
using UnityEngine;

namespace Card.CardStateMachine.States
{
    public class CardDestroy : CardBaseState
    {
        public CardDestroy(ICard handler, BaseStateMachine stateMachine, CardData cardData) : base(handler,
            stateMachine, cardData)
        {
        }

        #region Functions

        public override void OnEnterState()
        {
            Disable();
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