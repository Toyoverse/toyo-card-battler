using PlayerHand;
using Tools.UI;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Card
{
    public class CardDrawer : MonoBehaviour
    {
        PlayerHandUtils Drawer { get; set; }
        IMouseInput Input { get; set; }

        void Awake()
        {
            Drawer = transform.parent.GetComponentInChildren<PlayerHandUtils>();
            Input = GetComponent<IMouseInput>();
            Input.OnPointerClick += DrawCard;
        }

        void DrawCard(PointerEventData obj) => Drawer.DrawCard();
    }
}