using UnityEngine;
using UnityEngine.UI;

public class OnlineOpponent : BasePlayer
{
    public static OnlineOpponent instance;

    public override void Awake()
    {
        playerName = PlayerNamesForGame.OpponentForOnlineGame;
        instance = this;
        base.Awake();
    }

    public void PlayOpponentCard(string staticId)
    {
        foreach (RectTransform card in myHand)
        {
            string childStaticId = card.Find("CardStaticID").GetComponent<Text>().text;

            if (childStaticId == staticId)
            {
                ShowCardDetails(card);
                card.GetComponent<CardInteraction>().PlayCard();
                return;
            }
        }
    }

    public void DrawOpponentCard(string staticId)
    {
        RectTransform card = GetRectTransformCard(staticId);
        card.GetComponent<CardInteraction>().enabled = false;
        card.GetComponent<CardInteraction>().Playable = false;

        HideCardDetails(card);

        card.SetParent(myHand);
        card.localScale = new Vector3(1, 1, 1); //neznam zasto sam mjenja pa moram ja vratiti na default
        card.GetComponent<LayoutElement>().preferredWidth = 150;
    }

    private void HideCardDetails(RectTransform card)
    {
        foreach (Transform child in card.transform) //makni detalje karte s prikaza
        {
            child.GetComponent<CanvasGroup>().alpha = 0;
        }

        card.Find("HidePanel").GetComponent<CanvasGroup>().alpha = 1;
        card.GetComponent<CardHover>().enabled = false;
    }

    private void ShowCardDetails(RectTransform card)
    {
        foreach (RectTransform child in card)
        {
            child.GetComponent<CanvasGroup>().alpha = 1;
        }

        card.transform.Find("HidePanel").GetComponent<CanvasGroup>().alpha = 0;
        //nademo "brata" HidePanel i podesimo tako da se on ne vidi
        card.GetComponent<CardHover>().enabled = true;
    }
}