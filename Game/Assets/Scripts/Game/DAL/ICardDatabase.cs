using System.Collections.Generic;

public interface ICardDatabase
{
    List<Card> AllCards { get; set; }

    Card GetNewCard(string staticId);
    Card GetCard(string staticId);
    Card GetRandomCard();
    List<Card> GetRandomDeck();
}