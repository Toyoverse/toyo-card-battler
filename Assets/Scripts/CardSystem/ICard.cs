using Fusion;
using Patterns.StateMachine;

namespace Card
{
    public interface ICard : IStateMachineHandler, ICardComponents, ICardTransform
    {
        bool IsDragging { get; }
        bool IsHovering { get; }
        bool IsDisabled { get; }
        bool IsPlayer { get; }
        void Disable();
        void Enable();
        void Select();
        void Unselect();
        void Hover();
        void Draw();
        void Discard();
    }
}