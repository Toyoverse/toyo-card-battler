using UnityEngine;

namespace Card.CardUX
{
    public class CardMotionScale : CardMotionBase
    {
        public CardMotionScale(ICard handler) : base(handler)
        {
        }

        protected override bool CheckFinalState()
        {
            var _delta = Target - Handler.transform.localScale;
            return _delta.magnitude <= Threshold;
        }

        protected override void OnMotionEnds()
        {
            Handler.transform.localScale = Target;
            IsOperating = false;
        }

        protected override void KeepMotion()
        {
            var _current = Handler.transform.localScale;
            var _amount = Time.deltaTime * Speed;
            Handler.transform.localScale = Vector3.Lerp(_current, Target, _amount);
        }
    }
}