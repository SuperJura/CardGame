using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System;

public class CardInteraction : MonoBehaviour, IPointerClickHandler
{
    public delegate void OnCardPickTurnEndHandler(RectTransform card);
    public event OnCardPickTurnEndHandler OnCardPickTurnEnd;

    public RectTransform CDField { get; set; }

    void Start()
    {
        if (transform.parent.parent.name.StartsWith("B"))
        {
            TurnAroundTheCard();
        }
    }

    public void TurnAroundTheCard()
    {
        Quaternion newRotation = new Quaternion(0, 0, 180, 0);
        transform.localRotation = newRotation;
        transform.Find("CardName").localRotation = newRotation;
        transform.Find("CardInfo/CardHealth/CardHealthText").localRotation = newRotation;
        transform.Find("CardInfo/CardAttack/CardAttackText").localRotation = newRotation;
        transform.Find("CardInfo/CardCooldown/CardCooldownText").localRotation = newRotation;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        PlayCard();
    }

    public void PlayCard()
    {
        RectTransform myRectTransform = GetComponent<RectTransform>();
        if (CDField.childCount < 5)
        {
            transform.SetParent(CDField);
            OnCardPickTurnEnd(myRectTransform);
        }
    }
}