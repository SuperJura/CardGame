public class TurnsManagerOnlineGame : TurnsManager
{
    public override void FillHand()
    {
        //kod online igre, puni samo svoju ruku
        if (whoMoves == 'a')
        {
            aPlayer.FillHand();
        }
    }
}