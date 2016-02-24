﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AI : BasePlayer {

    bool playing;

    public override void Awake()
    {
        base.Awake();

        playing = false;
    }

    public override void Start()
    {
        base.Start();

        turnsManager.OnEndTurn += TurnsManager_OnEndTurn;
    }

    private void TurnsManager_OnEndTurn(EndTurnEventArgs args)
    {
        if (playing == true)
        {
            StartCoroutine(PlayTurn());
            playing = false;
        }
        else
        {
            playing = true;
        }
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

            HideCardDetails(card);

            card.SetParent(myHand);
            card.localScale = new Vector3(1, 1, 1); //neznam zasto sam mjenja pa moram ja vratiti na default
            card.GetComponent<LayoutElement>().preferredWidth = SystemSettings.GetCardWidth();
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

    public IEnumerator PlayTurn()
    {
        RectTransform playingCard = myHand.GetChild(Random.Range(0, myHand.childCount)).GetComponent<RectTransform>();
        yield return new WaitForSeconds(Random.Range(0.75f,2));
        ShowCardDetails(playingCard);
        playingCard.GetComponent<CardInteraction>().PlayCard();
    }
}