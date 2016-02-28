using UnityEngine;

public class EndGameManager : MonoBehaviour {

    public void EndGameAI(BasePlayer winner)
    {
        Debug.Log(winner.playerName + " won the game");
        GetComponent<GamesManager>().LoadMainMenu();
    }
}
