﻿using PlayerHand;
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
            CardHand = GlobalConfig.Instance.playerReferences.hand.GetComponent<IPlayerHand>();
            Input = GetComponent<IMouseInput>();
            Input.OnPointerUp += OnPointerUp;
        }

        protected virtual void OnPointerUp(PointerEventData eventData)
        {
        }
    }
}