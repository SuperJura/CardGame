using UnityEngine;
using UnityEngine.EventSystems;

public class CardInteraction : MonoBehaviour, IPointerClickHandler
{
    public delegate void OnCardPickTurnEndHandler(RectTransform card);

    public RectTransform CdField { get; set; }
    public bool Playable { get; set; } //bot cards cant be playable

    public void OnPointerClick(PointerEventData eventData)
    {
        if (Playable)
        {
            PlayCard();
        }
    }

    public event OnCardPickTurnEndHandler OnCardPickTurnEnd;

    private void Start()
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