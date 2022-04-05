using Patterns.StateMachine;

namespace Card.CardStateMachine.States
{
    public class CardDisable : CardBaseState
    {
        public CardDisable(ICard handler, BaseStateMachine stateMachine, CardData cardData) : base(handler, stateMachine, cardData)
        {
        }

        public override void OnEnterState() => Disable();
    }
}