﻿using PlayerHand;
using Tools;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Card
{
    public class CardDrawer : MonoBehaviour
    {
        private PlayerHandUtils Drawer { get; set; }
        private IMouseInput Input { get; set; }

        private void Awake()
        {
            Drawer = transform.parent.GetComponentInChildren<PlayerHandUtils>();
            Input = GetComponent<IMouseInput>();
            Input.OnPointerClick += DrawCard;
        }

        private void DrawCard(PointerEventData obj)
        {
            Drawer.DrawCard();
        }
    }
}