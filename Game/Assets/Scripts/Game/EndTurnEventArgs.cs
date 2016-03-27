using System;

public class EndTurnEventArgs : EventArgs
{
    public int TurnNumber { get; set; }
    public string NextPlayer { get; set; }
    public char NextPlayerChar { get; set; }

    public EndTurnEventArgs(int turnNumber, string nextPlayer, char nextPlayerChar)
    {
        TurnNumber = turnNumber;
        NextPlayer = nextPlayer;
        NextPlayerChar = nextPlayerChar;
    }
}