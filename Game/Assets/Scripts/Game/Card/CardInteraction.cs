using UnityEngine;
using UnityEngine.EventSystems;

public class CardInteraction : MonoBehaviour, IPointerClickHandler
{
    public delegate void OnCardPickTurnEndHandler(RectTransform card);
    public event OnCardPickTurnEndHandler OnCardPickTurnEnd;

    public RectTransform CdField;
    public bool Playable; //bot cards cant be playable

    public void OnPointerClick(PointerEventData eventData)
    {
        if (Playable)
        {
            PlayCard();
        }
    }

    public void PlayCard()
    {
        RectTransform myRectTransform = GetComponent<RectTransform>();
        if (CdField.childCount < 5)
        {
            transform.SetParent(CdField);
            OnCardPickTurnEnd(myRectTransform);
        }
    }
}