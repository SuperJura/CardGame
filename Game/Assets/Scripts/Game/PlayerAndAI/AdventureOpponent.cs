using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class AdventureOpponent : BasePlayer
{
    private bool playing;

    public override void Awake()
    {
        playerName = PlayerNamesForGame.NicknameForAdventureOpponent;
        myHand = transform.Find("PlayerHand").GetComponent<RectTransform>();

        guiManager = turnsManager.GetComponent<GUIManager>();
        deck = new List<Card>(); //kloniraj sve karte iz deka za avanture u dek protivnika
        foreach (Card card in AdventureDeck.DeckOpponent)
        {
            deck.Add((Card)card.Clone());
        }
        playing = false;
    }

    void Start () {
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
            card.SetParent(myHand);
            card.localScale = new Vector3(1, 1, 1); //neznam zasto sam mjenja pa moram ja vratiti na default
            card.GetComponent<LayoutElement>().preferredWidth = 150;
        }
    }

    private IEnumerator PlayTurn()
    {
        if (myHand.childCount == 0) yield break;
        RectTransform playingCard = myHand.GetChild(Random.Range(0, myHand.childCount)).GetComponent<RectTransform>();
        yield return new WaitForSeconds(Random.Range(0.75f, 2));
        playingCard.GetComponent<CardInteraction>().PlayCard();
    }
}