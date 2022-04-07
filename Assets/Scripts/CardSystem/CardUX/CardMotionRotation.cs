using UnityEngine;

namespace Card.CardUX
{
    public class CardMotionRotation : CardMotionBase
    {
        public CardMotionRotation(ICard handler) : base(handler)
        {
        }

        protected override float Threshold => 0.05f;

        protected override void OnMotionEnds()
        {
            Handler.transform.eulerAngles = Target;
            IsOperating = false;
            OnFinishMotion?.Invoke();
        }

        protected override bool CheckFinalState()
        {
            var _distance = Target - Handler.transform.eulerAngles;
            var _smallerThanLimit = _distance.magnitude <= Threshold;
            var _equals360 = (int)_distance.magnitude == 360;
            var _isFinal = _smallerThanLimit || _equals360;
            return _isFinal;
        }

        protected override void KeepMotion()
        {
            var _current = Handler.transform.rotation;
            var _amount = Speed * Time.deltaTime;
            var _rotation = Quaternion.Euler(Target);
            var _newRotation = Quaternion.RotateTowards(_current, _rotation, _amount);
            Handler.transform.rotation = _newRotation;
        }
    }
}