public class BotPlayer : BasePlayer {

    public override void Start()
    {
        base.Start();
    }

    public override void Awake()
    {
        playerName = PlayerNamesForGame.NicknameForBotGame;
        base.Awake();
    }
}