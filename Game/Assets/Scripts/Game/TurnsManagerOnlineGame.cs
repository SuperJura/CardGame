using UnityEngine;
using System.Collections;

public class TurnsManagerOnlineGame : TurnsManager {


    public override void FillHand()
    {
        //kod online igre, puni samo svoju ruku
        switch (whoMoves)
        {
            case 'a':
                aPlayer.FillHand();
                break;
            default:
                break;
        }
    }
}
