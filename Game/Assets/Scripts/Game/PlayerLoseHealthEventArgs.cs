using System;

public class PlayerLoseHealthEventArgs : EventArgs
{
    public PlayerLoseHealthEventArgs(char playerPosition, int currentHealth)
    {
        PlayerPosition = playerPosition;
        CurrentHealth = currentHealth;
    }

    public char PlayerPosition;
    public int CurrentHealth;
}