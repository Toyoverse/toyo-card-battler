using Patterns.StateMachine;

namespace Card.CardStateMachine.States
{
    public class CardIdle : CardBaseState
    {
        public CardIdle(ICard handler, BaseStateMachine stateMachine, CardData cardData) : base(handler, stateMachine, cardData)
        {
        }
    }
}