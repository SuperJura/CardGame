using System;
using System.Collections;

[Serializable]
public class Card
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

    public Card()
    {
        IDCard = ++idCounter;
        Name = "Generic Card";
        Health = 5;
        Attack = 5;
        DefaultCooldown = 5;
        CurrentCooldown = DefaultCooldown;
    }
}