using System.Collections.Generic;

public static class Deck
{
    public static List<Card> Cards { get; set; }

    public static DeckEnums DeckType { get; set; }

    static Deck()
    {
        Cards = Repository.GetCardDatabaseInstance().GetRandomDeck();
        DeckType = DeckEnums.Random;
    }
}