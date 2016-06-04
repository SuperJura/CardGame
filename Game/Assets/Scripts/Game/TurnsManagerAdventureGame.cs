using UnityEngine;
using UnityEngine.SceneManagement;

public class TurnsManagerAdventureGame : TurnsManager {

	// Use this for initialization
	public override void Start () {
        Transform gameboardPanel = GameObject.Find("Canvas/Gameboard/MainPanel").transform;
        specialAttacks = transform.GetComponent<SpecialAttacksManager>();
        APlayerSide = gameboardPanel.Find("A_PlayerSide").GetComponent<RectTransform>();
        BPlayerSide = gameboardPanel.Find("B_PlayerSide").GetComponent<RectTransform>();
        graveyard = gameboardPanel.Find("InfoPanel/Graveyard").GetComponent<RectTransform>();
        aPlayer = APlayerSide.GetComponent<BasePlayer>();
        bPlayer = BPlayerSide.GetComponent<BasePlayer>();
        InitializeGUI();
    }

    public override void InitializeGUI()
    {
        nublerOfTurns = 0;
        whoMoves = 'b';
        CallOnNotification(Enemy.combatMsg);
        DisablePicking();
        whoMoves = 'a';
        CallOnPlayerLoseHealth();
        EnablePicking();
        CallOnEndTurn();
    }

    public override void CheckIfPlayerWon()
    {
        if (BPlayerSide.Find("PlayerCDField").childCount == 0 &&
            BPlayerSide.Find("PlayerHand").childCount == 0 &&
            BPlayerSide.Find("PlayerPlayField").childCount == 0)
        {
            SceneManager.LoadScene("AdventureGame");
        }
        EndPlayerTurn();
    }
}
