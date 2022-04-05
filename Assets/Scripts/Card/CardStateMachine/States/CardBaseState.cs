using Patterns.StateMachine;

namespace Card.CardStateMachine.States
{
    public abstract class CardBaseState : IState
    {
        //CardData.DisabledAlpha
        protected const float DisabledAlpha = 0.3f;
        protected const int LayerToRenderNormal = 0;
        protected const int LayerToRenderTop = 1;
        protected const float DiscardedSize = 0.5f;
        protected const float ScaleSpeed = 8f;
        protected const float RotationSpeed = 50;
        protected const float RotationSpeedEnemy = 200;
        
        protected ICard Handler { get; }
        protected BaseStateMachine StateMachine { get; }
        protected CardData CardData { get; }
        public bool IsInitialized { get; }

        protected CardBaseState(ICard handler, BaseStateMachine stateMachine, CardData cardData)
        {
            StateMachine = stateMachine;
            Handler = handler;
            CardData = cardData;
            IsInitialized = true;
        }

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
            foreach (var renderer in Handler.Renderers)
            {
                var myColor = renderer.color;
                myColor.a = DisabledAlpha;
                renderer.color = myColor;
            }
        }
        
        protected void DisableCollision() => Handler.Collider.enabled = false;

        protected void EnableCollision() => Handler.Collider.enabled = true;
        
        protected void RemoveAllTransparency()
        {
            foreach (var _renderer in Handler.Renderers)
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
            for (var i = 0; i < Handler.Renderers.Length; i++)
                Handler.Renderers[i].sortingOrder = LayerToRenderTop;
        }
        
        /// <summary>
        ///     Renders the textures in the regular layer. Each card state is responsible to handle its own layer activity.
        /// </summary>
        protected virtual void MakeRenderNormal()
        {
            for (var i = 0; i < Handler.Renderers.Length; i++)
                if (Handler.Renderers[i])
                    Handler.Renderers[i].sortingOrder = LayerToRenderNormal;
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