using System;
using System.Collections.Generic;

public class EndTurnEventArgs : EventArgs
{
    public int TurnNumber { get; set; }
    public string NextPlayer { get; set; }

    public EndTurnEventArgs(int turnNumber, string nextPlayer)
    {
        TurnNumber = turnNumber;
        NextPlayer = nextPlayer;
    }
}
