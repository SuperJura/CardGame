using UnityEngine;
using UnityEngine.UI;

public class SpecialAttacksManager : MonoBehaviour
{
    private Transform playerA_CDField;
    private Transform playerA_PlayField;
    private Transform playerB_CDField;
    private Transform playerB_PlayField;

    private void Start()
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
            case "SA_2":
                DoLowestHeal(attackingCard, player);
                return true;
            default:
                return false;
        }
    }

    public string GetSpecialAttack(RectTransform card)
    {
        string cardStaticId = card.Find("CardStaticID").GetComponent<Text>().text;
        Card c = Repository.GetCardDatabaseInstance().GetNewCard(cardStaticId);

        return c.SpecialAttackId;
    }

    private void DoSpreadAttack(RectTransform attackingCard, char player)
    {
        Transform opponent = GetOpponentPlayField(player);
        int cardPosition = attackingCard.GetSiblingIndex();

        if (opponent.childCount == 0) //ako neprijatelj nema djece, nema ni napada
        {
            return;
        }

        //napada se jedno djete na ljevo i na desno
        int cardBefore = cardPosition - 1; //ako je index -1, onda ne napadni
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

    private void DoLowestHeal(RectTransform attackingCard, char player)
    {
        Transform playerPlayField = GetPlayerPlayField(player);
        Transform lowestCard = playerPlayField.GetChild(0);
        foreach (Transform child in playerPlayField)
        {
            int health = int.Parse(child.Find("CardInfo/CardHealth/CardHealthText").GetComponent<Text>().text);
            if (health > 0)
            {
                if (int.Parse(lowestCard.Find("CardInfo/CardHealth/CardHealthText").GetComponent<Text>().text) > health)
                {
                    lowestCard = child;
                }
            }
        }
        int currentHealth = int.Parse(lowestCard.Find("CardInfo/CardHealth/CardHealthText").GetComponent<Text>().text);
        lowestCard.GetComponent<Animation>().Play("SpecialAttackDamageAnimation");
        lowestCard.Find("CardInfo/CardHealth/CardHealthText").GetComponent<Text>().text = (currentHealth + 1).ToString();

    }

    private Transform GetOpponentPlayField(char player)
    {
        if (player == 'a')
        {
            return playerB_PlayField;
        }
        return playerA_PlayField;
    }

    private Transform GetPlayerPlayField(char player)
    {
        if (player == 'a')
        {
            return playerA_PlayField;
        }
        return playerB_PlayField;
    }
}