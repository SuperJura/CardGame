using UnityEngine;
using UnityEngine.EventSystems;

public class CardHover : MonoBehaviour, IPointerEnterHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        GUIManager.instance.DisplayCardHoverDetails(GetComponent<RectTransform>());
    }
}