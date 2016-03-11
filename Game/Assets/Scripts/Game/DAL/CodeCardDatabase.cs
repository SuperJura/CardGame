using UnityEngine;
using System.Collections.Generic;

public class CodeCardDatabase : ICardDatabase {

    public List<Card> AllCards { get; set; }

    public CodeCardDatabase()
    {
        AllCards = new List<Card>();
        FillList();
    }

    private void FillList()
    {
        AllCards.Add(new Card() { Name = "Blue bird", Health = 2, Attack = 1, StaticIDCard = "R_1", DefaultCooldown = 2, Quality = Enumerations.EquipmentQuality.Rare });
        AllCards.Add(new Card() { Name = "Red panda", Health = 4, Attack = 2, StaticIDCard = "R_2", DefaultCooldown = 4, Quality = Enumerations.EquipmentQuality.Legendary });
        AllCards.Add(new Card() { Name = "CUbe", Health = 2, Attack = 2, StaticIDCard = "R_3", DefaultCooldown = 3, Quality = Enumerations.EquipmentQuality.Common });
    }

    public Card GetCard(string staticID)
    {
        foreach (Card c in AllCards)
        {
            if (c.StaticIDCard == staticID)
            {
                return GetCopy(c);
            }
        }
        return null;
    }

    public Card GetRandomCard()
    {
        return AllCards[Random.Range(0, AllCards.Count)];
    }

    public Card GetCopy(Card original)
    {
        Card copy = new Card();
        copy.IDCard = copy.IDCard;
        copy.StaticIDCard = original.StaticIDCard;
        copy.Name = original.Name;
        copy.ImagePath = original.ImagePath;
        copy.Quality = original.Quality;
        copy.Health = original.Health;
        copy.Attack = original.Attack;
        copy.DefaultCooldown = original.DefaultCooldown;
        copy.CurrentCooldown = copy.DefaultCooldown;

        return copy;
    }

    public List<Card> GetRandomDeck()
    {
        List<Card> outputDeck = new List<Card>(20);

        for (int i = 0; i < 20; i++)
        {
            outputDeck.Add(GetRandomCard());
        }
        return outputDeck;
    }
}
