using Patterns.StateMachine;
using Zenject;

namespace Card.CardStateMachine.States
{
    public class CardBlock : CardBaseState
    {
        public CardBlock(ICard handler, BaseStateMachine stateMachine, SignalBus signalBus, CardData cardData) : base(handler,
            stateMachine,signalBus, cardData)
        {
        }

        #region Functions

        public override void OnEnterState()
        {
            BlockUsage();
        }

        public override void OnExitState()
        {
            EnableUsage();
        }

        #endregion
    }
}