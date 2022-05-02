using System;
using PlayerHand;
using Tools;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DefaultNamespace
{
    [RequireComponent(typeof(IMouseInput))]
    public abstract class DropZone : MonoBehaviour
    {
        protected IPlayerHand CardHand { get; set; }
        protected IMouseInput Input { get; set; }

        protected virtual void Awake()
        {
            Input = GetComponent<IMouseInput>();
            Input.OnPointerUp += OnPointerUp;
        }

        private void Start()
        {
            CardHand = GlobalConfig.Instance.battleReferences.hand.GetComponent<IPlayerHand>();
        }

        protected virtual void OnPointerUp(PointerEventData eventData)
        {
        }
    }
}