using System;
using System.Collections.Generic;

public static class AdventureDeck
{
    public static List<Card> DeckOpponent;
    public static List<Card> DeckPlayer;
    public static List<Card> CollectionPlayer;

    static AdventureDeck()
    {
        DeckOpponent = new List<Card>();
        DeckPlayer = new List<Card>();
        CollectionPlayer = new List<Card>();
        DebugAddCardsToDeck();   //TODO: Load from PlayerCards deck
    }

    public static void DebugAddCardsToDeck()
    {
        while (DeckPlayer.Count < 20)
        {
            DeckPlayer.Add(Repository.GetCardDatabaseInstance().GetRandomCard());
        }
    }

    [Serializable]
    public class PlayerCards
    {
        public List<Card> Deck;
        public List<Card> Collection;

    }
}
