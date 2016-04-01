using UnityEngine;
using UnityEngine.EventSystems;

public class CardHover : MonoBehaviour, IPointerEnterHandler
{
    public delegate void OnCardPointerEnterHandler(RectTransform card);

    public void OnPointerEnter(PointerEventData eventData)
    {
        RectTransform myRectTransform = GetComponent<RectTransform>();
        OnCardPointerEnter(myRectTransform);
    }

    public event OnCardPointerEnterHandler OnCardPointerEnter;
}