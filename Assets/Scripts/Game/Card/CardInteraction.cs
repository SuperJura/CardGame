using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class CardInteraction : MonoBehaviour, IPointerClickHandler
{
    public delegate void OnCardPickTurnEndHandler(RectTransform card);
    public event OnCardPickTurnEndHandler OnCardPickTurnEnd;

    public RectTransform CDField { get; set; }

    private RectTransform myRectTransform;  //rect transform same karte

    // Use this for initialization
    void Start()
    {
        myRectTransform = GetComponent<RectTransform>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        PlayCard();
    }

    public void PlayCard()
    {
        if (CDField.childCount < 5)
        {
            transform.SetParent(CDField);
            OnCardPickTurnEnd(myRectTransform);
        }
    }
}