using System.Collections.Generic;
using System.Linq;

//klasa koja ima sve karte u igracevom deku i pomocu nje se dodaju ili micu karte u deku
public static class Deck
{
    public static List<Card> Cards; //dek ima 20 karata
    public static List<Card> AdventureCardsOpponent;
    public static Enumerations.DeckEnums DeckType;
    public static string DeckName;

    static Deck()
    {
        Cards = Repository.GetCardDatabaseInstance().GetRandomDeck();
        DeckType = Enumerations.DeckEnums.Random;
        DeckName = DeckType.ToString();
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

        //int counter = Cards.Count(card => card.StaticIdCard == staticId);
        //TODO: Igrac nemoze imati vise od ~3 iste karte u deku

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