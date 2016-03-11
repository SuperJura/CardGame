using UnityEngine;
using System.Collections.Generic;

public interface ICardDatabase{

    List<Card> AllCards { get; set; }

    Card GetCard(string staticID);
    Card GetRandomCard();
    Card GetCopy(Card original);
    List<Card> GetRandomDeck();

}
