using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof (SpecialAttacksManager))]
public class TurnsManager : MonoBehaviour
{
    public delegate void OnEndTurnHandler(EndTurnEventArgs args);
    public delegate void OnNotificationHandler(char player, string message);
    public delegate void OnPlayerLoseHealthHandler(PlayerLoseHealthEventArgs args);

    public event OnEndTurnHandler OnEndTurn;
    public event OnNotificationHandler OnNotification;
    public event OnPlayerLoseHealthHandler OnPlayerLoseHealth;

    [HideInInspector]
    public BasePlayer aPlayer;
    [HideInInspector]
    public RectTransform APlayerSide;
    [HideInInspector]
    public BasePlayer bPlayer;
    [HideInInspector]
    public RectTransform BPlayerSide;
    [HideInInspector]
    public EndGameManager endGameManager;
    [HideInInspector]
    public RectTransform graveyard;
    [HideInInspector]
    public int nublerOfTurns;
    [HideInInspector]
    public SpecialAttacksManager specialAttacks;

    protected char whoMoves; //a = igrac A; b = igrac B

    // Use this for initialization
    public virtual void Start()
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

    public virtual void InitializeGUI()
    {
        nublerOfTurns = 0;
        whoMoves = 'b';
        CallOnNotification("welcome!");
        DisablePicking();
        whoMoves = 'a';
        CallOnNotification("welcome!");
        CallOnPlayerLoseHealth();
        EnablePicking();
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
    }   //2. faza, smanjivanje cooldowna kartama

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
    } //-medufaza- poslje 2. faze se prebacuju karte s 0 cd na PlayField

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
    } //3. faza

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
    } //zavrsavanje poteza

    public virtual void CheckIfPlayerWon()
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

    public void CallOnPlayerLoseHealth()
    {
        OnPlayerLoseHealth(new PlayerLoseHealthEventArgs('b', bPlayer.Health));
        OnPlayerLoseHealth(new PlayerLoseHealthEventArgs('a', aPlayer.Health));
    }

    public void CallOnNotification(string message)
    {
        OnNotification(whoMoves, message);
    }

    public void CallOnNotification(char player, string message)
    {
        OnNotification(player, message);
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
            CallOnPlayerLoseHealth();
        }
    }

    public bool CheckIfPlayerCanPlay()
    {
        if (GetCdFieldOfCurrentPlayer().childCount >= 5)
        {
            CallOnNotification("I cant put anymore cards");
            StartCoroutine(StartCoolDownPhase());
            return false;
        }
        if (GetPlayerHandOfCurrentPlayer().childCount == 0)
        {
            CallOnNotification("I have no more cards");
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

        CallOnNotification(msg);
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

        CallOnNotification(msg);
        CallOnPlayerLoseHealth();
    }

    public void DestroyDeadCards(List<RectTransform> cardsToDestroy)
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