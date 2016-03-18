using UnityEngine;
using System.Collections;

public class ChangeDeckMenuManager : MonoBehaviour {

    public DeckPanelManager deckPanelManager;

    public void RandomBtnClick()
    {
        Deck.DeckType = DeckEnums.Random;
        deckPanelManager.SetDeckType();
        Deck.Cards = Repository.GetCardDatabaseInstance().GetRandomDeck();
    }

    public void CustomBtnClick()
    {
        Deck.DeckType = DeckEnums.Custom;
        deckPanelManager.SetDeckType();
        //prikazi menu za biranje karti
    }
}
