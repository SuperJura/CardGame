using System;
using System.Collections.Generic;

public class PlayerLoseHealthEventArgs : EventArgs
{
    public char PlayerPosition { get; set; }
    public int CurrentHealth { get; set; }

    public PlayerLoseHealthEventArgs(char playerPosition, int currentHealth)
    {
        PlayerPosition = playerPosition;
        CurrentHealth = currentHealth;
    }
}
