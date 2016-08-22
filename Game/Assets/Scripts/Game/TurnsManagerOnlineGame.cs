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

    public override bool CheckIfPlayerCanPlay()
    {
        if (GetCdFieldOfCurrentPlayer().childCount >= 5)
        {
            CallOnNotification("I cant put anymore cards");
            if (IsCurrentPlayerA())
            {
                ServerGameBehavior.SendMessage("cardPlayed|-1");
            }
            StartCoroutine(StartCoolDownPhase());
            return false;
        }
        if (GetPlayerHandOfCurrentPlayer().childCount == 0)
        {
            CallOnNotification("I have no more cards");
            if (IsCurrentPlayerA())
            {
                ServerGameBehavior.SendMessage("cardPlayed|-1");
            }
            StartCoroutine(StartCoolDownPhase());
            return false;
        }
        return true;
    }
}