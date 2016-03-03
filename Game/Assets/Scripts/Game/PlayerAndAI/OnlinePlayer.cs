public class OnlinePlayer : BasePlayer {

    public override void Start()
    {
        base.Start();
    }

    public override void Awake()
    {
        playerName = PlayerNamesForGame.NicknameForOnlineGame;

        base.Awake();
    }

}
