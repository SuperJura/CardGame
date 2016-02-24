using System;
using System.Collections.Generic;
using UnityEngine;

public static class CardDatabaseManager
{
    static ICardDatabase database;

    static  CardDatabaseManager()
    {
        database = Repository.GetCardDatabaseInstance();
    }

    public static Card GetCard(string staticID)
    {
        return database.GetCard(staticID);
    }

    public static Card GetRandomCard()
    {
        return database.GetRandomCard();
    }

    public static List<Card> GetRandomDeck()
    {
        return database.GetRandomDeck();
    }
}
