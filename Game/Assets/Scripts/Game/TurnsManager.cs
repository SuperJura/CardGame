﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof (SpecialAttacksManager))]
public class TurnsManager : MonoBehaviour
{
    public delegate void OnEndTurnHandler(EndTurnEventArgs args);

    public delegate void OnNotificationHandler(char player, string message);

    public delegate void OnPlayerLoseHealthHandler(PlayerLoseHealthEventArgs args);

    protected BasePlayer aPlayer;

    private RectTransform APlayerSide;
    protected BasePlayer bPlayer;
    private RectTransform BPlayerSide;
    private EndGameManager endGameManager;
    private RectTransform graveyard;
    private int nublerOfTurns;
    private SpecialAttacksManager specialAttacks;

    protected char whoMoves; //a = igrac A; b = igrac B
    public event OnEndTurnHandler OnEndTurn;
    public event OnPlayerLoseHealthHandler OnPlayerLoseHealth;
    public event OnNotificationHandler OnNotification;

    // Use this for initialization
    private void Start()
    {
        Transform gameboardPanel = GameObject.Find("Canvas/Gameboard/MainPanel").transform;
        endGameManager = GameObject.Find("Canvas/EndGameMenu").GetComponent<EndGameManager>();
        specialAttacks = transform.GetComponent<SpecialAttacksManager>();
        APlayerSide = gameboardPanel.Find("A_PlayerSide").GetComponent<RectTransform>();
        BPlayerSide = gameboardPanel.Find("B_PlayerSide").GetComponent<RectTransform>();
        graveyard = gameboardPanel.Find("InfoPanel/Graveyard").GetComponent<RectTransform>();
        aPlayer = APlayerSide.GetComponent<BasePlayer>();
        bPlayer = BPlayerSide.GetComponent<BasePlayer>();
        InitializeGUI();
    }

    private void InitializeGUI()
    {
        nublerOfTurns = 0;
        whoMoves = 'b';
        CallOnNotification("welcome!");
        DisablePicking();
        whoMoves = 'a';
        CallOnNotification("welcome!");
        CallOnPlayerLoseHealth();

        CallOnEndTurn();
    }

    //FAZE
    public void EndPickPhase(RectTransform card)
    {
        card.GetComponent<CardInteraction>().enabled = false;
        DisablePicking();

        string cardName = card.Find("CardName").GetComponentInChildren<Text>().text;
        string msg = "I put " + cardName;
        CallOnNotification(msg); //prikaz notificationa

        StartCoroutine(StartCoolDownPhase());
    } //1. faza, biranje karte za igranje

    private IEnumerator StartCoolDownPhase()
    {
        RectTransform cdField = GetCdFieldOfCurrentPlayer();

        foreach (RectTransform card in cdField)
        {
            FocusCard(card);
            card.GetComponent<CardCombat>().DecreaseCooldown();

            yield return new WaitForSeconds(0.5f); //pauziranje metode

            UnfocusCard(card);
        }

        CheckForReadyCards();
    }

    private void CheckForReadyCards()
    {
        RectTransform cdField = GetCdFieldOfCurrentPlayer();
        RectTransform playField = GetPlayFieldOfCurrentPlayer();

        List<RectTransform> listOfReadyCards = new List<RectTransform>();

        foreach (RectTransform card in cdField)
        {
            if (card.Find(Card.cardCooldownPath).GetComponentInChildren<Text>().text == "0")
            {
                listOfReadyCards.Add(card);
            }
        }
        while (listOfReadyCards.Count != 0)
        {
            if (GetPlayFieldOfCurrentPlayer().childCount >= 5)
            {
                break;
            }

            listOfReadyCards[0].SetParent(playField);
            listOfReadyCards.RemoveAt(0);
        }

        StartCoroutine(StartAttackPhase());
    } //-medufaza- poslje 2. faze se prebacuju karte s 0 cd na PlayField

    private IEnumerator StartAttackPhase()
    {
        RectTransform attackerPlayField = GetPlayFieldOfCurrentPlayer();
        RectTransform defenderPlayField = GetPlayFieldOfOtherPlayer();

        for (int i = 0; i < attackerPlayField.childCount; i++) //svaka karta napada
        {
            RectTransform attackerCard = attackerPlayField.GetChild(i).GetComponent<RectTransform>();
            FocusCard(attackerCard); //prikazi koja karta napada

            Animation anim = attackerCard.GetComponent<Animation>();
            anim.Play("AttackingAnimation"); //animiraj napad

            RectTransform defenderCard = null;
            if (defenderPlayField.childCount >= i + 1) //ako postoji neprijatelj, napadni ga
            {
                defenderCard = defenderPlayField.GetChild(i).GetComponent<RectTransform>();
                AttackTarget(attackerCard, defenderCard);
                LowFocusCard(defenderCard);
            }
            else //inace napadni playera
            {
                AttackOpositePlayer(attackerCard);
            }

            string specialAttack = specialAttacks.GetSpecialAttack(attackerCard); //napravi specialni napad
            if (specialAttack != "") //ako karta ima specialni napad
            {
                yield return new WaitForSeconds(1.7f);
                specialAttacks.DoSpecialAttack(attackerCard, whoMoves);
            }

            yield return new WaitForSeconds(1);
            if (defenderCard != null)
            {
                UnfocusAliveCard(defenderCard);
            }
            UnfocusCard(attackerCard);
        }

        CheckForDeadCards();
    } //3. faza

    private void CheckForDeadCards()
    {
        RectTransform defenderPlayField = GetPlayFieldOfOtherPlayer();

        List<RectTransform> cardsToDestroy = new List<RectTransform>();
        foreach (RectTransform card in defenderPlayField)
        {
            int health = int.Parse(card.Find(Card.cardHealthPath).GetComponentInChildren<Text>().text);
            if (health <= 0)
            {
                cardsToDestroy.Add(card);
            }
        }

        DestroyDeadCards(cardsToDestroy);
        CheckIfPlayerWon();
    } //4. faza

    private void CheckIfPlayerWon()
    {
        if (aPlayer.Health <= 0)
        {
            endGameManager.EndGame(bPlayer, aPlayer);
        }
        if (bPlayer.Health <= 0)
        {
            endGameManager.EndGame(aPlayer, bPlayer);
        }
        EndPlayerTurn();
    }

    //POZIVANJE DELEGATA
    private void CallOnEndTurn()
    {
        EndTurnEventArgs args = null;
        switch (whoMoves)
        {
            case 'a':
                args = new EndTurnEventArgs(++nublerOfTurns, aPlayer.playerName, whoMoves);
                break;
            case 'b':
                args = new EndTurnEventArgs(++nublerOfTurns, bPlayer.playerName, whoMoves);
                break;
            default:
                break;
        }

        OnEndTurn(args);
    }

    private void CallOnPlayerLoseHealth()
    {
        OnPlayerLoseHealth(new PlayerLoseHealthEventArgs('b', bPlayer.Health));
        OnPlayerLoseHealth(new PlayerLoseHealthEventArgs('a', aPlayer.Health));
    }

    private void CallOnNotification(string message)
    {
        OnNotification(whoMoves, message);
    }

    private void CallOnNotification(char player, string message)
    {
        OnNotification(player, message);
    }

    //POMOCNE METODE
    private void EndPlayerTurn()
    {
        ChangePlayer();
        EnablePicking();

        CallOnEndTurn();
        FillHand();
    }

    public virtual void FillHand()
    {
        switch (whoMoves)
        {
            case 'a':
                aPlayer.FillHand();
                break;
            case 'b':
                bPlayer.FillHand();
                break;
        }
    }

    private void ChangePlayer()
    {
        switch (whoMoves)
        {
            case 'a':
                whoMoves = 'b';
                break;
            case 'b':
                whoMoves = 'a';
                break;
            default:
                whoMoves = ' ';
                break;
        }
    } //ako je whoMoves 'a', postaje 'b' i obrnuto

    private void UnfocusCard(RectTransform card)
    {
        card.transform.localScale = new Vector3(1, 1, 1);
    } //Postavlja Scale karte na 1, 1, 1

    private void UnfocusAliveCard(RectTransform card)
    {
        int health = int.Parse(card.Find(Card.cardHealthPath).GetComponent<Text>().text);

        if (health > 0)
        {
            card.localScale = new Vector3(1, 1, 1);
        }
    } //Postavlja Scale karte na 1, 1, 1 samo ako je karta ziva

    private void FocusCard(RectTransform card)
    {
        card.transform.localScale = new Vector3(1.1f, 1.1f, 1.1f);
    } //Postavlja Scale karte na 1.1, 1.1, 1.1

    private void LowFocusCard(RectTransform card)
    {
        card.localScale = new Vector3(0.9f, 0.9f, 0.9f);
    } //Postavlja Scale karte na 0.9, 0.9, 0.9

    private void DisablePicking()
    {
        RectTransform playerHand = GetPlayerHandOfCurrentPlayer();

        foreach (RectTransform card in playerHand)
        {
            //card.GetComponent<CanvasGroup>().blocksRaycasts = false;
            //card.GetComponent<CanvasGroup>().interactable = false;
            card.GetComponent<CardInteraction>().enabled = false;
        }
    } //onaj tko je na whoMoves nemoze vise birati karte

    private void EnablePicking()
    {
        RectTransform playerHand = GetPlayerHandOfCurrentPlayer();

        foreach (RectTransform card in playerHand)
        {
            //card.GetComponent<CanvasGroup>().blocksRaycasts = true;
            //card.GetComponent<CanvasGroup>().interactable = true;
            card.GetComponent<CardInteraction>().enabled = true;
        }
    } //onaj tko je na whoMoves moze birati karte

    private void AttackTarget(RectTransform attackerCard, RectTransform defenderCard)
    {
        int attack =
            int.Parse(attackerCard.Find(Card.cardAttackPath).GetComponentInChildren<Text>().text);

        CardCombat combatDefender = defenderCard.GetComponent<CardCombat>();
        combatDefender.RecieveDamage(attack);

        string attackName = attackerCard.Find("CardName").GetComponentInChildren<Text>().text;
        string defenceName = defenderCard.Find("CardName").GetComponentInChildren<Text>().text;
        string msg = attackName + " attacked your " + defenceName + " for " + attack;

        CallOnNotification(msg);
    }

    private void AttackOpositePlayer(RectTransform attackerCard)
    {
        int attack =
            int.Parse(attackerCard.Find(Card.cardAttackPath).GetComponentInChildren<Text>().text);
        switch (whoMoves)
        {
            case 'a':
                bPlayer.Health -= attack;
                break;
            case 'b':
                aPlayer.Health -= attack;
                break;
        }

        string attackName = attackerCard.Find("CardName").GetComponentInChildren<Text>().text;
        string msg = attackName + " attacked you for " + attack;

        CallOnNotification(msg);
        CallOnPlayerLoseHealth();
    }

    private void DestroyDeadCards(List<RectTransform> cardsToDestroy)
    {
        while (cardsToDestroy.Count != 0)
        {
            RectTransform card = cardsToDestroy[0];
            string destroyCardName = card.Find("CardName").GetComponentInChildren<Text>().text;
            string message = "My " + destroyCardName + " is destroyed!";
            CallOnNotification(GetOppositePlayer(), message);

            if (graveyard.childCount > 0)
            {
                foreach (Transform child in graveyard)
                {
                    Destroy(child.gameObject);
                }
            }

            card.SetParent(graveyard);
            cardsToDestroy.Remove(card);
        }
    }

    private char GetOppositePlayer()
    {
        switch (whoMoves)
        {
            case 'a':
                return 'b';
            case 'b':
                return 'a';
        }

        return ' ';
    }

    //JAVNE METODE
    public void PlayerOnTurn(string nickname)
    {
        char playerOnTurn = '-';
        if (aPlayer.playerName == nickname)
        {
            playerOnTurn = 'a';
        }
        else
        {
            playerOnTurn = 'b';
        }

        if (whoMoves != playerOnTurn)
        {
            DisablePicking();
            whoMoves = playerOnTurn;
            nublerOfTurns = 0;
            CallOnEndTurn();
        }
    }

    //RECTTRANSFORM POMOCNE METODE
    private RectTransform GetCdFieldOfCurrentPlayer()
    {
        RectTransform cdField;
        switch (whoMoves)
        {
            case 'a':
                cdField = APlayerSide.Find("PlayerCDField").GetComponent<RectTransform>();
                break;
            case 'b':
                cdField = BPlayerSide.Find("PlayerCDField").GetComponent<RectTransform>();
                break;
            default:
                cdField = null;
                break;
        }
        return cdField;
    }

    private RectTransform GetPlayerHandOfCurrentPlayer()
    {
        RectTransform playerHand;
        switch (whoMoves)
        {
            case 'a':
                playerHand = APlayerSide.Find("PlayerHand").GetComponent<RectTransform>();
                break;
            case 'b':
                playerHand = BPlayerSide.Find("PlayerHand").GetComponent<RectTransform>();
                break;
            default:
                playerHand = null;
                break;
        }
        return playerHand;
    }

    private RectTransform GetPlayFieldOfCurrentPlayer()
    {
        RectTransform playField;
        switch (whoMoves)
        {
            case 'a':
                playField = APlayerSide.Find("PlayerPlayField").GetComponent<RectTransform>();
                break;
            case 'b':
                playField = BPlayerSide.Find("PlayerPlayField").GetComponent<RectTransform>();
                break;
            default:
                playField = null;
                break;
        }
        return playField;
    } //play field TRENUTNOG igraca

    private RectTransform GetPlayFieldOfOtherPlayer()
    {
        RectTransform playField;
        switch (whoMoves)
        {
            case 'a':
                playField = BPlayerSide.Find("PlayerPlayField").GetComponent<RectTransform>();
                break;
            case 'b':
                playField = APlayerSide.Find("PlayerPlayField").GetComponent<RectTransform>();
                break;
            default:
                playField = null;
                break;
        }
        return playField;
    } //play field DRUGOG igraca
}