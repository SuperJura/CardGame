using System.Collections.Generic;

public static class Deck
{
    public static List<Card> Cards { get; set; }    //dek ima 20 karata
    public static DeckEnums DeckType { get; set; }
    public static string DeckName { get; set; }


    static Deck()
    {
        Cards = Repository.GetCardDatabaseInstance().GetRandomDeck();
        DeckType = DeckEnums.Random;
        DeckName = "random";
    }

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

    public static bool AddCard(string staticID)
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
            if (card.StaticIDCard == staticID)
            {
                counter++;
            }
        }

        Card cardToPut = Repository.GetCardDatabaseInstance().GetCard(staticID);
        if (cardToPut != null)
        {
            Cards.Add(cardToPut);
            return true;
        }
        return false;
    }

    public static bool RemoveCard(string staticID)
    {
        foreach (Card card in Cards)
        {
            if (card.StaticIDCard == staticID)
            {
                Cards.Remove(card);
                return true;
            }
        }
        return false;
    }
}