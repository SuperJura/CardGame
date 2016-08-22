using UnityEngine;
using UnityEngine.UI;

public class OnlinePlayer : BasePlayer
{
    public static OnlinePlayer instance;

    public override void Awake()
    {
        playerName = PlayerNamesForGame.NicknameForOnlineGame;
        instance = this;
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