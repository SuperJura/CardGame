using UnityEngine;
using System.Collections;
using UnityEngine.UI;

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
        string cardStaticID = attackingCard.Find("CardStaticID").GetComponent<Text>().text;
        Card card = Repository.GetCardDatabaseInstance().GetCard(cardStaticID);

        if (card.SpecialAttackID == "") //ako je id prazan, znaci da karta nema specijalni napad
        {
            return false;
        }

        switch (card.SpecialAttackID)
        {
            case "SA_1":
                Debug.Log("SA_1");
                return true;
            default:
                return false;
        }
    }
}