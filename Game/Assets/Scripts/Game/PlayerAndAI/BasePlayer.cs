using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BasePlayer : MonoBehaviour
{
    protected ICardDatabase database;
    protected GUIManager guiManager;
    protected RectTransform myHand;
    protected bool isPlayer;

    [HideInInspector]
    public List<Card> deck;
    [HideInInspector]
    public string playerName;
    public TurnsManager turnsManager;
    public int Health;

    public virtual void Awake()
    {
        myHand = transform.Find("PlayerHand").GetComponent<RectTransform>();
        isPlayer = transform.name.StartsWith("A");

        guiManager = turnsManager.GetComponent<GUIManager>();
        deck = new List<Card>(); //kloniraj sve karte iz deka u dek igraca
        foreach (Card card in Deck.Cards)
        {
            deck.Add((Card) card.Clone());
        }
    }

    public virtual void Start()
    {
        database = Repository.GetCardDatabaseInstance();
        Health = 10;
    }

    public virtual void FillHand()
    {
        while (myHand.childCount < 5)
        {
            if (deck.Count <= 0)
            {
                return;
            }
            RectTransform card = GetRectTransformCard();
            card.GetComponent<CardInteraction>().enabled = false;
            card.GetComponent<CardInteraction>().Playable = true;

            card.SetParent(myHand);
            card.localScale = new Vector3(1, 1, 1); //neznam zasto sam mjenja pa moram ja vratiti na default
            card.GetComponent<LayoutElement>().preferredWidth = 150;
        }
    }

    protected RectTransform GetRectTransformCard()
    {
        GameObject go = (GameObject) Resources.Load("GameResources/Card");

        RectTransform cardRectTransform = Instantiate((RectTransform) go.transform);
        Card card = GetCardFromDeck();

        return FillRectTranformWithDetails(cardRectTransform, card);
    }

    protected RectTransform GetRectTransformCard(string staticId)
    {
        GameObject go = (GameObject) Resources.Load("GameResources/Card");

        RectTransform cardRectTransform = Instantiate((RectTransform) go.transform);
        Card card = database.GetNewCard(staticId);
        return FillRectTranformWithDetails(cardRectTransform, card);
    }

    private RectTransform FillRectTranformWithDetails(RectTransform cardRectTransform, Card card)
    {
        cardRectTransform.Find("CardName").GetComponentInChildren<Text>().text = card.Name;
        cardRectTransform.Find("CardStaticID").GetComponentInChildren<Text>().text = card.StaticIdCard;
        cardRectTransform.Find(Card.cardCooldownPath).GetComponentInChildren<Text>().text =
            card.DefaultCooldown.ToString();
        cardRectTransform.Find(Card.cardHealthPath).GetComponentInChildren<Text>().text =
            card.Health.ToString();
        cardRectTransform.Find(Card.cardAttackPath).GetComponentInChildren<Text>().text =
            card.Attack.ToString();
        if (card.SpecialAttackId != "")
        {
            cardRectTransform.Find("SpecialAttackSign").GetComponentInChildren<Image>().enabled = true;
        }
        cardRectTransform.GetComponent<CardInteraction>().CdField =
            transform.Find("PlayerCDField").GetComponent<RectTransform>();
        cardRectTransform.GetComponent<CardHover>().OnCardPointerEnter += guiManager.Card_OnHover;

        //kasnije dodaj sliku
        switch (card.Quality)
        {
            case Enumerations.EquipmentQuality.Common:
                cardRectTransform.GetComponent<Image>().color = Color.white;
                break;
            case Enumerations.EquipmentQuality.Rare:
                cardRectTransform.GetComponent<Image>().color = new Color(0/255f, 232/ 255f, 232/ 255f); //blue
                break;
            case Enumerations.EquipmentQuality.Legendary:
                cardRectTransform.GetComponent<Image>().color = new Color(212/255f, 199/255f, 48/255f); //yellow
                break;
        }

        if (!isPlayer)
        {
            TurnAroundTheCard(cardRectTransform);
        }
        return cardRectTransform;
    }

    public void TurnAroundTheCard(RectTransform cardRectTransform)
    {
        Quaternion newRotation = new Quaternion(0, 0, 180, 0);
        cardRectTransform.localRotation = newRotation;
        cardRectTransform.Find("CardName").localRotation = newRotation;
        cardRectTransform.Find("CardInfo").localRotation = newRotation;
        cardRectTransform.Find("CardInfo/CardHealthContainer").localRotation = newRotation;
        cardRectTransform.Find("CardInfo/CardCooldownContainer").localRotation = newRotation;
        cardRectTransform.Find("CardInfo/CardAttackContainer").localRotation = newRotation;
        cardRectTransform.Find("CardInfo/CardHealthContainer/CardHealth/CardHealthText").localRotation = newRotation;
        cardRectTransform.Find("CardInfo/CardCooldownContainer/CardCooldown/CardCooldownText").localRotation = newRotation;
        cardRectTransform.Find("CardInfo/CardAttackContainer/CardAttack/CardAttackText").localRotation = newRotation;
    }

    protected Card GetCardFromDeck()
    {
        Card card = deck[Random.Range(0, deck.Count)];
        deck.Remove(card);

        return card;
    }
}