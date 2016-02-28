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
        set
        {
            health = value;
            if (health <= 0)
            {
                GameObject.Find("GameManager").GetComponent<EndGameManager>().EndGameAI(this);
            }
        }
    }
    
    public TurnsManager turnsManager;

    protected List<Card> deck;
    protected RectTransform myHand;
    
    public virtual void Awake()
    {
        deck = new List<Card>(20);

        myHand = transform.Find("PlayerHand").GetComponent<RectTransform>();
    }

    public virtual void Start()
    {
        ICardDatabase database = Repository.GetCardDatabaseInstance();
        health = 5;
        deck = database.GetRandomDeck();

        FillHand();
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

        RectTransform cardRectTransform = GameObject.Instantiate((RectTransform)go.transform);
        Card card = GetCardFromDeck();

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
                cardRectTransform.GetComponent<Image>().color = new Color(78 / 255f, 78 / 255f, 204 / 255f);  //blue
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