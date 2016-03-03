using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class OnlineOpponent : BasePlayer {

    public override void Start()
    {
        base.Start();
    }

    public override void Awake()
    {
        playerName = PlayerNamesForGame.OpponentForOnlineGame;

        base.Awake();
    }

    //razlika izmedu ovog FillHand() i od BasePlayera je u tome da se tu sakriva karta
    public override void FillHand()
    {
        while (myHand.childCount < 5)
        {
            if (deck.Count <= 0)
            {
                return;
            }
            RectTransform card = GetRectTransformCard();
            card.GetComponent<CardInteraction>().enabled = false;

            HideCardDetails(card);

            card.SetParent(myHand);
            card.localScale = new Vector3(1, 1, 1); //neznam zasto sam mjenja pa moram ja vratiti na default
            card.GetComponent<LayoutElement>().preferredWidth = 150;
        }
    }

    private static void HideCardDetails(RectTransform card)
    {
        foreach (Transform child in card.transform) //makni detalje karte s prikaza
        {
            child.GetComponent<CanvasGroup>().alpha = 0;
        }

        card.Find("HidePanel").GetComponent<CanvasGroup>().alpha = 1;
    }

    private static void ShowCardDetails(RectTransform card)
    {

        foreach (RectTransform child in card)
        {
            child.GetComponent<CanvasGroup>().alpha = 1;
        }

        card.transform.Find("HidePanel").GetComponent<CanvasGroup>().alpha = 0;   //nademo "brata" HidePanel i podesimo tako da se on ne vidi
    }

}
