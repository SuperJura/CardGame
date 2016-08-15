using UnityEngine;

public class ChangeDeckMenuManager : MonoBehaviour
{
    public DeckPanelManager deckPanelManager;

    public void RandomBtnClick()
    {
        Deck.DeckType = Enumerations.DeckEnums.Random;
        deckPanelManager.SetDeckType();
        Deck.Cards = Repository.GetCardDatabaseInstance().GetRandomDeck();
    }

    public void CustomBtnClick()
    {
        Deck.DeckType = Enumerations.DeckEnums.Custom;
        Deck.Cards.Clear();
        deckPanelManager.SetDeckType();
    }
}