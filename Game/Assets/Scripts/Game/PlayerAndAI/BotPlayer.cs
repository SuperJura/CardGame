public class BotPlayer : BasePlayer {

    public override void Start()
    {
        base.Start();

        FillHand();
    }

    public override void Awake()
    {
        playerName = PlayerNamesForGame.NicknameForBotGame;
        base.Awake();
    }
}