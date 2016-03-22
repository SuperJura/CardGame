using UnityEngine;
using System.Collections;

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


}
