public class CoopPlayer : BasePlayer
{
    public override void Start()
    {
        base.Start();

        FillHand();
    }

    public override void Awake()
    {
        if (IsPlayerA())
        {
            playerName = PlayerNamesForGame.NicknameForCoopGameA;
        }
        else
        {
            playerName = PlayerNamesForGame.NicknameForCoopGameB;
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