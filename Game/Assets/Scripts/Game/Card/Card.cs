using System;

[Serializable]
public class Card : ICloneable
{
    public static string cardHealthPath = "CardInfo/CardHealthContainer/CardHealth/CardHealthText";
    public static string cardCooldownPath = "CardInfo/CardCooldownContainer/CardCooldown/CardCooldownText";
    public static string cardAttackPath = "CardInfo/CardAttackContainer/CardAttack/CardAttackText";

    private static int idCounter;
    
    
    public Card()
    {
        IdCard = ++idCounter;
        Name = "Generic Card";
        Health = 5;
        Attack = 5;
        DefaultCooldown = 5;
        CurrentCooldown = DefaultCooldown;
        SpecialAttackId = "";
    }

    public int IdCard { get; set; }

    public string StaticIdCard { get; set; }
    public string Name { get; set; }
    public string ImagePath { get; set; }

    public Enumerations.EquipmentQuality Quality { get; set; }

    public int Health { get; set; }
    public int Attack { get; set; }
    public int DefaultCooldown { get; set; }
    public int CurrentCooldown { get; set; }
    public string SpecialAttackId { get; set; }
    public string CardFlavour { get; set; }

    public object Clone()
    {
        Card copy = new Card
        {
            IdCard = ++idCounter,
            StaticIdCard = StaticIdCard,
            Name = Name,
            ImagePath = ImagePath,
            Quality = Quality,
            Health = Health,
            Attack = Attack,
            DefaultCooldown = DefaultCooldown
        };
        copy.CurrentCooldown = copy.DefaultCooldown;
        copy.SpecialAttackId = SpecialAttackId;
        copy.CardFlavour = CardFlavour;

        return copy;
    }
}