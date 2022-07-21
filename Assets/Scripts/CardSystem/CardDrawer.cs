using System;
using PlayerHand;
using Tools;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace Card
{
    public class CardDrawer : MonoBehaviour
    {
        [Inject]
        private PlayerHandUtils _drawer;
        
        private IMouseInput Input { get; set; }

        private void Awake()
        {
            Input = GetComponent<IMouseInput>();
            Input.OnPointerClick += DrawCard;
        }

        private void DrawCard(PointerEventData obj)
        {
            _drawer.DrawCard();
        }
    }
}