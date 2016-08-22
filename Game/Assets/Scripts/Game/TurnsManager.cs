﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof (SpecialAttacksManager))]
public class TurnsManager : MonoBehaviour
{
    public delegate void OnEndTurnHandler(EndTurnEventArgs args);

    public event OnEndTurnHandler OnEndTurn;

    public static TurnsManager instance;

    [HideInInspector]
    public BasePlayer aPlayer;
    [HideInInspector]
    public RectTransform APlayerSide;
    [HideInInspector]
    public BasePlayer bPlayer;
    [HideInInspector]
    public RectTransform BPlayerSide;
    [HideInInspector]
    public RectTransform graveyard;
    [HideInInspector]
    public int nublerOfTurns;
    [HideInInspector]
    public SpecialAttacksManager specialAttacks;

    protected char whoMoves; //a = igrac A; b = igrac B

    void Awake()
    {
        instance = this;
    }

    // Use this for initialization
    public virtual void Start()
    {
        Transform gameboardPanel = GameObject.Find("Canvas/Gameboard/MainPanel").transform;
        specialAttacks = transform.GetComponent<SpecialAttacksManager>();
        APlayerSide = gameboardPanel.Find("A_PlayerSide").GetComponent<RectTransform>();
        BPlayerSide = gameboardPanel.Find("B_PlayerSide").GetComponent<RectTransform>();
        graveyard = gameboardPanel.Find("InfoPanel/Graveyard").GetComponent<RectTransform>();
        aPlayer = APlayerSide.GetComponent<BasePlayer>();
        bPlayer = BPlayerSide.GetComponent<BasePlayer>();
        InitializeGUI();
    }

    public virtual void InitializeGUI()
    {
        nublerOfTurns = 0;
        whoMoves = 'b';
        MakeNotification("welcome!");
        DisablePicking();
        whoMoves = 'a';
        MakeNotification("welcome!");
        UpdateHealthValues();
        EnablePicking();
        CallOnEndTurn();
    }

    //FAZE
    public void EndPickPhase(RectTransform card)
    {
        DisablePicking();
        if (card != null)
        {
            card.GetComponent<CardInteraction>().enabled = false;
            string cardName = card.Find("CardName").GetComponentInChildren<Text>().text;
            string msg = "I put " + cardName;
            MakeNotification(msg); //prikaz notificationa
        }
        StartCoroutine(StartCoolDownPhase());
    } //1. faza, biranje karte za igranje (u radu je obiljezeno kao faza 2)

    public IEnumerator StartCoolDownPhase()
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
    }   //2. faza, smanjivanje cooldowna kartama (u radu je obiljezeno kao faza 3)

    public void CheckForReadyCards()
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
    } //3. faza, prebacuju se karte s 0 cd na PlayField (u radu je obiljezeno kao faza 4)

    public IEnumerator StartAttackPhase()
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

            string specialAttack = specialAttacks.GetSpecialAttack(attackerCard); //nadi specialni napad
            if (specialAttack != "") //ako karta ima specialni napad
            {
                yield return new WaitForSeconds(1.7f);
                specialAttacks.DoSpecialAttack(attackerCard, whoMoves); //napravi ga
            }

            yield return new WaitForSeconds(1);
            if (defenderCard != null)
            {
                UnfocusAliveCard(defenderCard);
            }
            UnfocusCard(attackerCard);
        }

        CheckForDeadCards();
    } //3. faza, napad (u radu obiljezeno kao faza 5)

    public void CheckForDeadCards()
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
    } //zavrsavanje poteza (u radu obiljezeno kao faza 8)

    public virtual void CheckIfPlayerWon()
    {
        if (aPlayer.Health <= 0)
        {
            GameMenuManager.instance.EndGame(bPlayer, aPlayer);
        }
        if (bPlayer.Health <= 0)
        {
            GameMenuManager.instance.EndGame(aPlayer, bPlayer);
        }
        EndPlayerTurn();
    }

    //POZIVANJE DELEGATA
    public void CallOnEndTurn()
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
        }
        OnEndTurn(args);
    }

    public void UpdateHealthValues()
    {
        GUIManager.instance.UpdateHealthValues(new PlayerLoseHealthEventArgs('b', bPlayer.Health));
        GUIManager.instance.UpdateHealthValues(new PlayerLoseHealthEventArgs('a', aPlayer.Health));
    }

    public void MakeNotification(string message)
    {
        GUIManager.instance.MakeNotification(whoMoves, message);
    }

    public void MakeNotification(char player, string message)
    {
        GUIManager.instance.MakeNotification(player, message);
    }

    //POMOCNE METODE
    public void EndPlayerTurn()
    {
        ChangePlayer();
        FillHand();
        CallOnEndTurn();
        DecreaseHealthIfFieldsAreFull();
        if (CheckIfPlayerCanPlay())
        {
            EnablePicking();
        }
        else
        {
            DisablePicking();
        }
    }

    private void DecreaseHealthIfFieldsAreFull()
    {
        if (GetCdFieldOfCurrentPlayer().childCount == 5 && GetPlayFieldOfCurrentPlayer().childCount == 5
            && GetCdFieldOfOtherPlayer().childCount == 5 && GetPlayFieldOfCurrentPlayer().childCount == 5)
        {
            aPlayer.Health -= 1;
            bPlayer.Health -= 1;
            UpdateHealthValues();
        }
    }

    public virtual bool CheckIfPlayerCanPlay()
    {
        if (GetCdFieldOfCurrentPlayer().childCount >= 5)
        {
            MakeNotification("I cant put anymore cards");
            StartCoroutine(StartCoolDownPhase());
            return false;
        }
        if (GetPlayerHandOfCurrentPlayer().childCount == 0)
        {
            MakeNotification("I have no more cards");
            StartCoroutine(StartCoolDownPhase());
            return false;
        }
        return true;
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

    public void ChangePlayer()
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

    public void UnfocusCard(RectTransform card)
    {
        card.transform.localScale = new Vector3(1, 1, 1);
    } //Postavlja Scale karte na 1, 1, 1

    public void UnfocusAliveCard(RectTransform card)
    {
        int health = int.Parse(card.Find(Card.cardHealthPath).GetComponent<Text>().text);

        if (health > 0)
        {
            card.localScale = new Vector3(1, 1, 1);
        }
    } //Postavlja Scale karte na 1, 1, 1 samo ako je karta ziva

    public void FocusCard(RectTransform card)
    {
        card.transform.localScale = new Vector3(1.1f, 1.1f, 1.1f);
    } //Postavlja Scale karte na 1.1, 1.1, 1.1

    public void LowFocusCard(RectTransform card)
    {
        card.localScale = new Vector3(0.9f, 0.9f, 0.9f);
    } //Postavlja Scale karte na 0.9, 0.9, 0.9

    public void DisablePicking()
    {
        RectTransform playerHand = GetPlayerHandOfCurrentPlayer();

        foreach (RectTransform card in playerHand)
        {
            card.GetComponent<CardInteraction>().enabled = false;
        }
    } //onaj tko je na whoMoves nemoze vise birati karte

    public void EnablePicking()
    {
        RectTransform playerHand = GetPlayerHandOfCurrentPlayer();

        foreach (RectTransform card in playerHand)
        {
            card.GetComponent<CardInteraction>().enabled = true;
        }
    } //onaj tko je na whoMoves moze birati karte

    public void AttackTarget(RectTransform attackerCard, RectTransform defenderCard)
    {
        int attack =
            int.Parse(attackerCard.Find(Card.cardAttackPath).GetComponentInChildren<Text>().text);

        CardCombat combatDefender = defenderCard.GetComponent<CardCombat>();
        combatDefender.RecieveDamage(attack);

        string attackName = attackerCard.Find("CardName").GetComponentInChildren<Text>().text;
        string defenceName = defenderCard.Find("CardName").GetComponentInChildren<Text>().text;
        string msg = attackName + " attacked your " + defenceName + " for " + attack;

        MakeNotification(msg);
    }

    public void AttackOpositePlayer(RectTransform attackerCard)
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

        MakeNotification(msg);
        UpdateHealthValues();
    }

    public void DestroyDeadCards(List<RectTransform> cardsToDestroy)
    {
        while (cardsToDestroy.Count != 0)
        {
            RectTransform card = cardsToDestroy[0];
            string destroyCardName = card.Find("CardName").GetComponentInChildren<Text>().text;
            string message = "My " + destroyCardName + " is destroyed!";
            MakeNotification(GetOppositePlayer(), message);

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

    public char GetOppositePlayer()
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

    public BasePlayer GetCurrentPlayer()
    {
        switch (whoMoves)
        {
            case 'a':
                return aPlayer;
            case 'b':
                return bPlayer;
        }

        return null;
    }

    public bool IsCurrentPlayerA()
    {
        if (whoMoves == 'a') return true;
        return false;
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
    public RectTransform GetCdFieldOfCurrentPlayer()
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

    public RectTransform GetPlayerHandOfCurrentPlayer()
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

    public RectTransform GetPlayFieldOfCurrentPlayer()
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

    public RectTransform GetPlayFieldOfOtherPlayer()
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

    public RectTransform GetCdFieldOfOtherPlayer()
    {
        RectTransform playField;
        switch (whoMoves)
        {
            case 'a':
                playField = BPlayerSide.Find("PlayerCDField").GetComponent<RectTransform>();
                break;
            case 'b':
                playField = APlayerSide.Find("PlayerCDField").GetComponent<RectTransform>();
                break;
            default:
                playField = null;
                break;
        }
        return playField;
    } //play field DRUGOG igraca
}