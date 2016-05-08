﻿using UnityEngine;
using UnityEngine.UI;

public class FillCardList : MonoBehaviour
{
    private ICardDatabase database;

    private void Start()
    {
        database = Repository.GetCardDatabaseInstance();
        FillList();
    }

    public void FillList()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        foreach (Card card in database.AllCards)
        {
            GameObject go = (GameObject) Resources.Load("MainMenuResources/CardItem");
            RectTransform prefab = Instantiate((RectTransform) go.transform);

            RectTransform cardRectTransform = (RectTransform) prefab.Find("Card");
            int numberInDeck = GetNumberOfCardInDeck(card.StaticIdCard);
            FillCardInfo(cardRectTransform, card, numberInDeck);

            prefab.SetParent(transform);
            prefab.localScale = new Vector3(1, 1, 1); //neznam zasto sam mjenja pa moram ja vratiti na default
        }
        transform.parent.parent.Find("DeckName").GetComponent<InputField>().text = Deck.DeckName;
    }

    private int GetNumberOfCardInDeck(string staticIdCard)
    {
        int counter = 0;
        foreach (Card card in Deck.Cards)
        {
            if (card.StaticIdCard == staticIdCard)
            {
                counter++;
            }
        }
        return counter;
    }

    private void FillCardInfo(RectTransform cardRectTransform, Card card, int numberInDeck)
    {
        cardRectTransform.Find("CardName").GetComponentInChildren<Text>().text = card.Name;
        cardRectTransform.Find("CardStaticID").GetComponentInChildren<Text>().text = card.StaticIdCard;
        cardRectTransform.Find(Card.cardCooldownPath).GetComponentInChildren<Text>().text =
            card.DefaultCooldown.ToString();
        cardRectTransform.Find(Card.cardHealthPath).GetComponentInChildren<Text>().text =
            card.Health.ToString();
        cardRectTransform.Find(Card.cardAttackPath).GetComponentInChildren<Text>().text =
            card.Attack.ToString();
        cardRectTransform.parent.Find("ControlPanel/Counter").GetComponent<Text>().text = numberInDeck.ToString();

        if (card.SpecialAttackId != "")
        {
            cardRectTransform.Find("SpecialAttackSign").GetComponent<Image>().enabled = true;
        }

        //kasnije dodaj sliku
        switch (card.Quality)
        {
            case Enumerations.EquipmentQuality.Common:
                cardRectTransform.GetComponent<Image>().color = Color.white;
                break;
            case Enumerations.EquipmentQuality.Rare:
                cardRectTransform.GetComponent<Image>().color = new Color(0/255f, 107/255f, 255/255f); //blue
                break;
            case Enumerations.EquipmentQuality.Legendary:
                cardRectTransform.GetComponent<Image>().color = new Color(212/255f, 199/255f, 48/255f); //yellow
                break;
        }
    }
}