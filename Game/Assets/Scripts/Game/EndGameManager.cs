using UnityEngine;
using UnityEngine.UI;

public class EndGameManager : MonoBehaviour
{
    public RectTransform originalMessages;

    private MenuManager menuManager;

    private void Start()
    {
        menuManager = GameObject.Find("Canvas").GetComponent<MenuManager>();
    }

    public void EndGame(BasePlayer winner, BasePlayer loser)
    {
        Debug.Log(winner.playerName + " won the game");
        DisplayDetails(winner.playerName, loser.playerName);
        SetMatchHistory();
        menuManager.LoadMenu(gameObject);
    }

    public void EndGame(char winner, char loser)
    {
        BasePlayer aPlayer = transform.parent.Find("Gameboard/MainPanel/A_PlayerSide").GetComponent<BasePlayer>();
        BasePlayer bPlayer = transform.parent.Find("Gameboard/MainPanel/B_PlayerSide").GetComponent<BasePlayer>();

        if (char.ToLower(winner) == 'a')
        {
            EndGame(aPlayer, bPlayer);
        }
        else
        {
            EndGame(bPlayer, aPlayer);
        }
        if (TurnsManager.gameMode ==Enumerations.GameModes.Online) Destroy(TurnsManager.instance);
    }

    private void SetMatchHistory()
    {
        Transform matchHistory = transform.Find("MainPanel/Panel/MatchHistory/Messages");
        while (originalMessages.childCount != 0)
        {
            originalMessages.GetChild(originalMessages.childCount - 1).SetParent(matchHistory);
                //poruke idu obrnutim redosljedom
            matchHistory.GetChild(matchHistory.childCount - 1).localScale = new Vector3(1, 1, 1);
        }
    }

    private void DisplayDetails(string winner, string loser)
    {
        transform.Find("MainPanel/Title").GetComponent<Text>().text = winner + " won the game!";
        transform.Find("MainPanel/Panel/Winner").GetComponentInChildren<Text>().text = winner;
        transform.Find("MainPanel/Panel/Loser").GetComponentInChildren<Text>().text = loser;
    }
}