public class NetPlayer : BasePlayer {

    public override void Start()
    {
        base.Start();
    }

    public override void Awake()
    {
        if (IsPlayerA())
        {
            playerName = PlayerNamesForGame.NicknameForOnlineGame;
        }
        else
        {
            playerName = PlayerNamesForGame.OpponentForOnlineGame;
        }

        base.Awake();
    }

    private bool IsPlayerA()
    {
        if (transform.name.StartsWith("A"))
        {
            return true;
        }
        return false;
    }

}
