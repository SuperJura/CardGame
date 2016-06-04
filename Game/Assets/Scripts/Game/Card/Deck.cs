using System.Collections.Generic;

public static class Deck
{
    static Deck()
    {
        Cards = Repository.GetCardDatabaseInstance().GetRandomDeck();
        DeckType = DeckEnums.Random;
        DeckName = "random";
    }

    public static List<Card> Cards; //dek ima 20 karata
    public static List<Card> AdventureCardsOpponent;
    public static DeckEnums DeckType;
    public static string DeckName;

    public static int CheckCards()
    {
        int counter = 0;
        while (Cards.Count < 20)
        {
            AddRandomCard();
            counter++;
        }
        return counter;
    }

    private static void AddRandomCard()
    {
        Cards.Add(Repository.GetCardDatabaseInstance().GetRandomCard());
    }

    public static bool AddCard(string staticId)
    {
        if (Cards.Count >= 20)
        {
            return false;
        }
        //if (counter >= 3)
        //{
        //    return false;
        //}

        int counter = 0;
        foreach (Card card in Cards)
        {
            if (card.StaticIdCard == staticId)
            {
                counter++;
            }
        }

        Card cardToPut = Repository.GetCardDatabaseInstance().GetNewCard(staticId);
        if (cardToPut != null)
        {
            Cards.Add(cardToPut);
            return true;
        }
        return false;
    }

    public static bool RemoveCard(string staticId)
    {
        foreach (Card card in Cards)
        {
            if (card.StaticIdCard == staticId)
            {
                Cards.Remove(card);
                return true;
            }
        }
        return false;
    }
}