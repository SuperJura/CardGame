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

    public int IdCard;

    public string StaticIdCard;
    public string Name;
    public string ImagePath;

    public Enumerations.EquipmentQuality Quality;

    public int Health;
    public int Attack;
    public int DefaultCooldown;
    public int CurrentCooldown;
    public string SpecialAttackId;
    public string CardFlavour;

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