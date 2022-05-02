using System;
using PlayerHand;
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
            Input = GetComponent<IMouseInput>();
            Input.OnPointerClick += DrawCard;
        }

        private void Start()
        {
            Drawer = GlobalConfig.Instance.battleReferences.deck.GetComponent<PlayerHandUtils>();
        }

        private void DrawCard(PointerEventData obj)
        {
            Drawer.DrawCard();
        }
    }
}