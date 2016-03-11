using UnityEngine;
using UnityEngine.UI;

public class OnlinePlayer : BasePlayer {

    ServerGameBehavior gameBehavior;

    public override void Start()
    {
        gameBehavior = GameObject.Find("GameManager").GetComponent<ServerGameBehavior>();
        base.Start();

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

            card.SetParent(myHand);
            card.localScale = new Vector3(1, 1, 1); //neznam zasto sam mjenja pa moram ja vratiti na default
            card.GetComponent<LayoutElement>().preferredWidth = 150;
            card.GetComponent<CardInteraction>().OnCardPickTurnEnd += gameBehavior.Card_OnCardPickTurnEnd;
            gameBehavior.CardDrawn(card.Find("CardStaticID").GetComponent<Text>().text);
        }
    }
}
