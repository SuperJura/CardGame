using UnityEngine;
using UnityEngine.EventSystems;

public class CardHover : MonoBehaviour, IPointerEnterHandler
{
    public delegate void OnCardPointerEnterHandler(RectTransform card);

    public event OnCardPointerEnterHandler OnCardPointerEnter;

    public void OnPointerEnter(PointerEventData eventData)
    {
        RectTransform myRectTransform = GetComponent<RectTransform>();
        OnCardPointerEnter(myRectTransform);
    }

}