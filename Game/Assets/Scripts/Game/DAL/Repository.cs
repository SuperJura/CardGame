using System;

public static class Repository
{
    private static ICardDatabase cardDatabase;

    public static ICardDatabase GetCardDatabaseInstance()
    {
        if (cardDatabase == null)
        {
            cardDatabase = new CodeCardDatabase();
        }
        return cardDatabase;
    }
}