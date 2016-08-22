using UnityEngine;
using UnityEngine.UI;

public class OnlinePlayer : BasePlayer
{
    private ServerGameBehavior gameBehavior;

    public override void Start()
    {
        gameBehavior = GameObject.Find("GameManager").GetComponent<ServerGameBehavior>();
        gameBehavior.OnCanStart += GameBehavior_OnCanStart;
        base.Start();
    }

    private void GameBehavior_OnCanStart()
    {
        FillHand();
    }

    public override void Awake()
    {
        playerName = PlayerNamesForGame.NicknameForOnlineGame;

        base.Awake();
    }

    public override void FillHand()
    {
        while (myHand.childCount < 5)
        {
            if (deck.Count <= 0)
            {
                return;
            }
            RectTransform card = GetRectTransformCard();
            card.GetComponent<CardInteraction>().Playable = true;

            card.SetParent(myHand);
            card.localScale = new Vector3(1, 1, 1); //neznam zasto sam mjenja pa moram ja vratiti na default
            card.GetComponent<LayoutElement>().preferredWidth = 150;
            ServerGameBehavior.SendMessage("cardDrawed|" + card.Find("CardStaticID").GetComponent<Text>().text);
        }
    }
}