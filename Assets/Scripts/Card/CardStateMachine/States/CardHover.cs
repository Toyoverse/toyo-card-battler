using Patterns.StateMachine;

namespace Card.CardStateMachine.States
{
    public class CardHover : CardBaseState
    {
        public CardHover(ICard handler, BaseStateMachine stateMachine, CardData cardData) : base(handler, stateMachine, cardData)
        {
        }
    }
}