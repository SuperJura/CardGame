using System;

public class EndTurnEventArgs : EventArgs
{
    public int TurnNumber;
    public string NextPlayer;
    public char NextPlayerChar;

    public EndTurnEventArgs(int turnNumber, string nextPlayer, char nextPlayerChar)
    {
        TurnNumber = turnNumber;
        NextPlayer = nextPlayer;
        NextPlayerChar = nextPlayerChar;
    }
}