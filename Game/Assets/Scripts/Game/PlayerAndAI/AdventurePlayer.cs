using UnityEngine;
using System.Collections.Generic;

public class AdventurePlayer : BotPlayer {

    public override void Awake()
    {
        playerName = PlayerNamesForGame.NicknameForAdventurePlayer;
        myHand = transform.Find("PlayerHand").GetComponent<RectTransform>();

        guiManager = turnsManager.GetComponent<GUIManager>();
        deck = new List<Card>(); //kloniraj sve karte iz deka za avanture u dek protivnika
        foreach (Card card in AdventureDeck.DeckPlayer)
        {
            deck.Add((Card)card.Clone());
        }
    }
}