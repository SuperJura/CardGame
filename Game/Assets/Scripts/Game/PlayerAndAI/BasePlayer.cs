using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class BasePlayer : MonoBehaviour {

    public string playerName;
    private int health;
    public int Health
    {
        get { return health; }
        set { health = value; }
    }

    public TurnsManager turnsManager;

    protected List<Card> deck;
    protected RectTransform myHand;
    protected ICardDatabase database;

    public virtual void Awake()
    {
        deck = new List<Card>(20);
        Transform ph = transform.Find("PlayerHand");
        myHand = transform.Find("PlayerHand").GetComponent<RectTransform>();
    }

    public virtual void Start()
    {
        database = Repository.GetCardDatabaseInstance();
        health = 1;
        deck = database.GetRandomDeck();
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

            card.SetParent(myHand);
            card.localScale = new Vector3(1, 1, 1); //neznam zasto sam mjenja pa moram ja vratiti na default
            card.GetComponent<LayoutElement>().preferredWidth = 150;
        }
    }

    protected RectTransform GetRectTransformCard()
    {
        GameObject go = (GameObject)Resources.Load("GameResources/Card");

        RectTransform cardRectTransform = Instantiate((RectTransform)go.transform);
        Card card = GetCardFromDeck();

        return FillRectTranformWithDetails(cardRectTransform, card);
    }

    protected RectTransform GetRectTransformCard(string staticID)
    {
        GameObject go = (GameObject)Resources.Load("GameResources/Card");

        RectTransform cardRectTransform = Instantiate((RectTransform)go.transform);
        Card card = database.GetCard(staticID);
        return FillRectTranformWithDetails(cardRectTransform, card);
    }

    private RectTransform FillRectTranformWithDetails(RectTransform cardRectTransform, Card card)
    {
        cardRectTransform.Find("CardName").GetComponentInChildren<Text>().text = card.Name;
        cardRectTransform.Find("CardStaticID").GetComponentInChildren<Text>().text = card.StaticIDCard;
        cardRectTransform.Find("CardInfo/CardCooldown/CardCooldownText").GetComponentInChildren<Text>().text = card.DefaultCooldown.ToString();
        cardRectTransform.Find("CardInfo/CardHealth/CardHealthText").GetComponentInChildren<Text>().text = card.Health.ToString();
        cardRectTransform.Find("CardInfo/CardAttack/CardAttackText").GetComponentInChildren<Text>().text = card.Attack.ToString();
        cardRectTransform.GetComponent<CardInteraction>().CDField = transform.Find("PlayerCDField").GetComponent<RectTransform>();
        cardRectTransform.GetComponent<CardInteraction>().OnCardPickTurnEnd += turnsManager.EndPickPhase;

        //kasnije dodaj sliku
        switch (card.Quality)
        {
            case Enumerations.EquipmentQuality.Common:
                cardRectTransform.GetComponent<Image>().color = Color.white;
                break;
            case Enumerations.EquipmentQuality.Rare:
                cardRectTransform.GetComponent<Image>().color = new Color(0 / 255f, 107 / 255f, 255 / 255f);  //blue
                break;
            case Enumerations.EquipmentQuality.Legendary:
                cardRectTransform.GetComponent<Image>().color = new Color(212 / 255f, 199 / 255f, 48 / 255f); //yellow
                break;
        }

        return cardRectTransform;
    }

    protected Card GetCardFromDeck()
    {
        Card card = deck[Random.Range(0, deck.Count)];
        deck.Remove(card);

        return card;
    }
}