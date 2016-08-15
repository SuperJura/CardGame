using System;

public class PlayerLoseHealthEventArgs : EventArgs
{
    public char PlayerPosition;
    public int CurrentHealth;

    public PlayerLoseHealthEventArgs(char playerPosition, int currentHealth)
    {
        PlayerPosition = playerPosition;
        CurrentHealth = currentHealth;
    }
}