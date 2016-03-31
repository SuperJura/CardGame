using UnityEngine;
using System.Collections.Generic;

public interface ICardDatabase{

    List<Card> AllCards { get; set; }

    Card GetNewCard(string staticID);
    Card GetCard(string staticID);
    Card GetRandomCard();
    List<Card> GetRandomDeck();
}