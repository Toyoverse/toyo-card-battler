using Patterns.StateMachine;

namespace Card.CardStateMachine.States
{
    public class CardDraw : CardBaseState
    {
        public CardDraw(ICard handler, BaseStateMachine stateMachine, CardData cardData) : base(handler, stateMachine, cardData)
        {
        }
    }
}