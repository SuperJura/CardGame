using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class SpecialAttacksManager : MonoBehaviour {

    private Transform playerA_CDField;
    private Transform playerB_CDField;
    private Transform playerA_PlayField;
    private Transform playerB_PlayField;

    void Start()
    {
        Transform gameboard = GameObject.Find("Canvas/Gameboard/MainPanel").transform;
        playerA_CDField = gameboard.Find("A_PlayerSide/PlayerCDField");
        playerB_CDField = gameboard.Find("B_PlayerSide/PlayerCDField");
        playerA_PlayField = gameboard.Find("A_PlayerSide/PlayerPlayField");
        playerB_PlayField = gameboard.Find("B_PlayerSide/PlayerPlayField");
    }
    
    public bool DoSpecialAttack(RectTransform attackingCard, char player)
    {
        string specialAttack = GetSpecialAttack(attackingCard);
        if (specialAttack == "")
        {
            return false;
        }

        switch (specialAttack)
        {
            case "SA_1":
                DoSpreadAttack(attackingCard, player);
                return true;
            default:
                return false;
        }
    }

    public string GetSpecialAttack(RectTransform card)
    {
        string cardStaticID = card.Find("CardStaticID").GetComponent<Text>().text;
        Card c = Repository.GetCardDatabaseInstance().GetCard(cardStaticID);

        return c.SpecialAttackID;
    }

    private void DoSpreadAttack(RectTransform attackingCard, char player)
    {
        Transform parent = GetCardPlayField(player);
        Transform opponent = GetOpponentPlayField(player);
        int cardPosition = attackingCard.GetSiblingIndex();

        if (opponent.childCount == 0)   //ako neprijatelj nema djece, nema ni napada
        {
            return;
        }

        //napada se jedno djete na ljevo i na desno
        int cardBefore = cardPosition - 1;  //ako je index -1, onda ne napadni
        int cardAfter = cardPosition + 1;

        if (cardAfter >= opponent.childCount)
        {
            cardAfter = -1;
        }
        if (cardBefore >= opponent.childCount)
        {
            cardBefore = -1;
        }

        attackingCard.GetComponent<Animation>().Play("DoSpecialAttackAnimation");
        if (cardBefore != -1)
        {
            Transform defCard = opponent.GetChild(cardBefore);
            defCard.GetComponent<CardCombat>().RecieveDamage(1);
            defCard.GetComponent<Animation>().Play("SpecialAttackDamageAnimation");
        }
        if (cardAfter != -1)
        {
            Transform defCard = opponent.GetChild(cardAfter);
            defCard.GetComponent<CardCombat>().RecieveDamage(1);
            defCard.GetComponent<Animation>().Play("SpecialAttackDamageAnimation");
        }
    }

    private Transform GetOpponentPlayField(char player)
    {
        if (player == 'a')
        {
            return playerB_PlayField;
        }
        return playerA_PlayField;
    }

    private Transform GetCardPlayField(char player)
    {
        if (player == 'a')
        {
            return playerA_PlayField;
        }
        return playerB_PlayField;
    }
}