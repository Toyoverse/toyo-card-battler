using UnityEngine.EventSystems;

namespace DefaultNamespace
{
    public class PlayZone : DropZone
    {
        protected override void OnPointerUp(PointerEventData eventData)
        {
            CardHand?.PlaySelected();
        }
    }
}