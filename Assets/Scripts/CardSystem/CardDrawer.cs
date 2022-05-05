using System;
using PlayerHand;
using Tools;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Card
{
    public class CardDrawer : MonoBehaviour
    {
        private Lazy<PlayerHandUtils> _drawer = new (FindObjectOfType<PlayerHandUtils>);
        public PlayerHandUtils Drawer => _drawer.Value;
        
        private IMouseInput Input { get; set; }

        private void Awake()
        {
            Input = GetComponent<IMouseInput>();
            Input.OnPointerClick += DrawCard;
        }

        private void DrawCard(PointerEventData obj)
        {
            Drawer.DrawCard();
        }
    }
}