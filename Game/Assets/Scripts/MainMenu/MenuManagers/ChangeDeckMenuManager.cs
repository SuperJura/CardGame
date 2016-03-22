using UnityEngine;
using System.Collections;

public class ChangeDeckMenuManager : MonoBehaviour {

    public DeckPanelManager deckPanelManager;

    public void RandomBtnClick()
    {
        Deck.DeckType = DeckEnums.Random;
        Deck.DeckName = "random";
        deckPanelManager.SetDeckType();
        Deck.Cards = Repository.GetCardDatabaseInstance().GetRandomDeck();
    }

    public void CustomBtnClick()
    {
        Deck.DeckType = DeckEnums.Custom;
        Deck.DeckName = "custom";
        Deck.Cards.Clear();
        deckPanelManager.SetDeckType();
    }
}