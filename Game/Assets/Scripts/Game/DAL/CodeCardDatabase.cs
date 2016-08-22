using System.Collections.Generic;
using UnityEngine;

public class CodeCardDatabase : ICardDatabase
{
    public List<Card> AllCards { get; set; }

    public CodeCardDatabase()
    {
        AllCards = new List<Card>();
        FillList();
    }

    public Card GetNewCard(string staticId)
    {
        foreach (Card c in AllCards)
        {
            if (c.StaticIdCard == staticId)
            {
                return (Card) c.Clone();
            }
        }
        return null;
    }

    public Card GetCard(string staticId)
    {
        foreach (Card c in AllCards)
        {
            if (c.StaticIdCard == staticId)
            {
                return c;
            }
        }
        return null;
    }

    public Card GetRandomCard()
    {
        return (Card) AllCards[Random.Range(0, AllCards.Count)].Clone();
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

    private void FillList()
    {
        AllCards.Add(new Card
        {
            Name = "Blue bird",
            Health = 2,
            Attack = 1,
            StaticIdCard = "R_1",
            DefaultCooldown = 2,
            Quality = Enumerations.EquipmentQuality.Rare,
            CardFlavour = "Blue birds are known for flying higher than normal birds"
        });
        AllCards.Add(new Card
        {
            Name = "Red panda",
            Health = 4,
            Attack = 2,
            StaticIdCard = "R_2",
            DefaultCooldown = 4,
            Quality = Enumerations.EquipmentQuality.Legendary,
            CardFlavour = "Rare red pandas have thick fur so they can withstand harsh weater"
        });
        AllCards.Add(new Card
        {
            Name = "CUbe",
            Health = 2,
            Attack = 2,
            StaticIdCard = "R_3",
            DefaultCooldown = 3,
            Quality = Enumerations.EquipmentQuality.Common,
            CardFlavour = "No one knows much about CUbes"
        });
        AllCards.Add(new Card
        {
            Name = "Black wolf",
            Health = 2,
            Attack = 1,
            StaticIdCard = "R_4",
            DefaultCooldown = 3,
            Quality = Enumerations.EquipmentQuality.Common,
            SpecialAttackId = "SA_1",
            CardFlavour = "Special attack: 'Spread' - Attacks enemy to the right and a enemy to the left"
        });
        AllCards.Add(new Card
        {
            Name = "Lab monkey",
            Health = 4,
            Attack = 0,
            StaticIdCard = "R_5",
            DefaultCooldown = 4,
            Quality = Enumerations.EquipmentQuality.Rare,
            SpecialAttackId = "SA_2",
            CardFlavour = "Special attack: 'Lowest Heal' - Heals your lowest creature in play field"
        });
        AllCards.Add(new Card
        {
            Name = "Debug",
            Health = 1,
            Attack = 0,
            StaticIdCard = "R_6",
            DefaultCooldown = 4,
            Quality = Enumerations.EquipmentQuality.Common,
            CardFlavour = "Debug card"
        });
    }
}