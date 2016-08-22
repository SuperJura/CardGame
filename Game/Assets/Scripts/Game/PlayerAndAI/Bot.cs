﻿using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Bot : BasePlayer
{
    private bool playing;

    public override void Awake()
    {
        base.Awake();
        playing = false;
    }

    public override void Start()
    {
        base.Start();

        FillHand();
        turnsManager.OnEndTurn += TurnsManager_OnEndTurn;
    }

    private void TurnsManager_OnEndTurn(EndTurnEventArgs args)
    {
        if (playing)
        {
            StartCoroutine(PlayTurn());
            playing = false;
        }
        else
        {
            playing = true;
        }
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
            card.GetComponent<CardInteraction>().Playable = false;

            HideCardDetails(card);

            card.SetParent(myHand);
            card.localScale = new Vector3(1, 1, 1); //neznam zasto sam mjenja pa moram ja vratiti na default
            card.GetComponent<LayoutElement>().preferredWidth = 150;
        }
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

    public IEnumerator PlayTurn()
    {
        RectTransform playingCard = myHand.GetChild(Random.Range(0, myHand.childCount)).GetComponent<RectTransform>();
        //delay je samo da se igrac lakse prati sto se dogada na ploci
        yield return new WaitForSeconds(Random.Range(0.75f, 2));
        ShowCardDetails(playingCard);
        playingCard.GetComponent<CardInteraction>().PlayCard();
    }
}