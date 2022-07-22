using System;
using PlayerHand;
using Tools;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace DefaultNamespace
{
    [RequireComponent(typeof(IMouseInput))]
    public abstract class DropZone : MonoBehaviour
    {
        [Inject]
        protected IPlayerHand CardHand;
        protected IMouseInput Input { get; set; }

        protected virtual void Awake()
        {
            Input = GetComponent<IMouseInput>();
            Input.OnPointerUp += OnPointerUp;
        }

        protected virtual void OnPointerUp(PointerEventData eventData)
        {
        }
    }
}