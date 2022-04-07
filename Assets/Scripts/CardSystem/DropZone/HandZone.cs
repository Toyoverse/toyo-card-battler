using UnityEngine.EventSystems;

namespace DefaultNamespace
{
    public class HandZone : DropZone
    {
        protected override void OnPointerUp(PointerEventData eventData)
        {
            CardHand?.Unselect();
        }
    }
}