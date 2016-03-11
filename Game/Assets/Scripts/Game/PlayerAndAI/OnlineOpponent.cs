using UnityEngine;
using UnityEngine.UI;
using System;

public class OnlineOpponent : BasePlayer {

    ServerGameBehavior gameBehavior;

    public override void Start()
    {
        gameBehavior = GameObject.Find("GameManager").GetComponent<ServerGameBehavior>();
        gameBehavior.OnOpponentDrawed += GameBehavior_OnOpponentDrawed;
        gameBehavior.OnOpponentPlayed += GameBehavior_OnOpponentPlayed;

        base.Start();
    }

    private void GameBehavior_OnOpponentPlayed(string staticID)
    {
        foreach (RectTransform card in myHand)
        {
            string childStaticID = card.Find("CardStaticID").GetComponent<Text>().text;

            if (childStaticID == staticID)
            {
                ShowCardDetails(card);
                card.GetComponent<CardInteraction>().PlayCard();
                return;
            }
        }
    }

    public override void Awake()
    {
        playerName = PlayerNamesForGame.OpponentForOnlineGame;

        base.Awake();
    }

    private void GameBehavior_OnOpponentDrawed(string staticID)
    {
        DrawCard(staticID);
    }

    public void DrawCard(string staticID)
    {
        RectTransform card = GetRectTransformCard(staticID);
        card.GetComponent<CardInteraction>().enabled = false;

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
    }

    private void ShowCardDetails(RectTransform card)
    {
        foreach (RectTransform child in card)
        {
            child.GetComponent<CanvasGroup>().alpha = 1;
        }

        card.transform.Find("HidePanel").GetComponent<CanvasGroup>().alpha = 0;   //nademo "brata" HidePanel i podesimo tako da se on ne vidi
    }
}