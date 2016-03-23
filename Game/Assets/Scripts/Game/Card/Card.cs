using System;

[Serializable]
public class Card : ICloneable
{
    private static int idCounter = 0;
    public int IDCard { get; set; }

    public string StaticIDCard { get; set; }
    public string Name { get; set; }
    public string ImagePath { get; set; }

    public Enumerations.EquipmentQuality Quality { get; set; }

    public int Health { get; set; }
    public int Attack { get; set; }
    public int DefaultCooldown { get; set; }
    public int CurrentCooldown { get; set; }
    public string SpecialAttackID { get; set; }

    public Card()
    {
        IDCard = ++idCounter;
        Name = "Generic Card";
        Health = 5;
        Attack = 5;
        DefaultCooldown = 5;
        CurrentCooldown = DefaultCooldown;
        SpecialAttackID = "";
    }

    public object Clone()
    {
        Card copy = new Card();
        copy.IDCard = ++idCounter;
        copy.StaticIDCard = StaticIDCard;
        copy.Name = Name;
        copy.ImagePath = ImagePath;
        copy.Quality = Quality;
        copy.Health = Health;
        copy.Attack = Attack;
        copy.DefaultCooldown = DefaultCooldown;
        copy.CurrentCooldown = copy.DefaultCooldown;
        copy.SpecialAttackID = SpecialAttackID;

        return copy;
    }
}