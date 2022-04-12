using DefaultNamespace;
using Extensions;
using Patterns.StateMachine;

namespace Card.CardStateMachine.States
{
    public abstract class CardBaseState : IState
    {

        protected CardBaseState(ICard handler, BaseStateMachine stateMachine, CardData cardData)
        {
            StateMachine = stateMachine;
            Handler = handler;
            CardData = cardData;
            IsInitialized = true;
        }


        protected ICard Handler { get; }
        protected BaseStateMachine StateMachine { get; }
        protected CardData CardData { get; }
        public bool IsInitialized { get; }

        #region Functions

        protected void Enable()
        {
            if (Handler.Collider)
                EnableCollision();
            if (Handler.Rigidbody)
                Handler.Rigidbody.Sleep();

            MakeRenderNormal();
            RemoveAllTransparency();
        }

        protected virtual void Disable()
        {
            DisableCollision();
            Handler.Rigidbody.Sleep();
            MakeRenderNormal();
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

        /// <summary>
        ///     Renders the textures in the first layer. Each card state is responsible to handle its own layer activity.
        /// </summary>
        protected virtual void MakeRenderFirst()
        {
            return; //Todo Fix Text
            for (var i = 0; i < Handler.Images.Length; i++)
                Handler.Images[i].sortingOrder = GlobalCardData.LayerToRenderTop;
        }

        /// <summary>
        ///     Renders the textures in the regular layer. Each card state is responsible to handle its own layer activity.
        /// </summary>
        protected virtual void MakeRenderNormal()
        {
            return; //Todo Fix Text
            for (var i = 0; i < Handler.Images.Length; i++)
                if (Handler.Images[i])
                    Handler.Images[i].sortingOrder = GlobalCardData.LayerToRenderNormal;
        

            
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