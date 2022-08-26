using DefaultNamespace;
using Extensions;
using Patterns.StateMachine;
using Zenject;

namespace Card.CardStateMachine.States
{
    public abstract class CardBaseState : IState
    {

        protected CardBaseState(ICard handler, BaseStateMachine stateMachine, SignalBus signalBus, CardData cardData)
        {
            StateMachine = stateMachine;
            Handler = handler;
            CardData = cardData;
            SignalBus = signalBus;
            IsInitialized = true;
        }


        protected ICard Handler { get; }
        protected SignalBus SignalBus { get; }
        protected BaseStateMachine StateMachine { get; }
        protected CardData CardData { get; }
        public bool IsInitialized { get; }

        #region Functions

        protected void EnableUsage()
        {
            if (Handler.Collider)
                EnableCollision();
            if (Handler.Rigidbody)
                Handler.Rigidbody.Sleep();

            RemoveAllTransparency();
        }

        protected virtual void BlockUsage()
        {
            DisableCollision();
            Handler.Rigidbody.Sleep();
            foreach (var renderer in Handler.Images)
            {
                var myColor = renderer.color;
                myColor.a = GlobalCardData.DisabledAlpha;
                renderer.color = myColor;
            }
        }

        protected void DisableCollision()
        {
            Handler.Collider.enabled = false;
        }

        protected void EnableCollision()
        {
            Handler.Collider.enabled = true;
        }

        protected void RemoveAllTransparency()
        {
            foreach (var _renderer in Handler.Images)
                if (_renderer)
                {
                    var _myColor = _renderer.color;
                    _myColor.a = 1;
                    _renderer.color = _myColor;
                }
        }

        #endregion

        #region StateMachine

        public virtual void OnInitialize()
        {
        }

        public virtual void OnEnterState()
        {
        }

        public virtual void OnExitState()
        {
        }

        public virtual void OnUpdate()
        {
        }

        public virtual void OnClear()
        {
        }

        public virtual void OnNextState(IState next)
        {
        }

        #endregion
    }
}