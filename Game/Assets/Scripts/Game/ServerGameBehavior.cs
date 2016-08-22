using UnityEngine;
using UnityEngine.UI;
using WebSocketSharp;

public class ServerGameBehavior : MonoBehaviour
{
    private Transform notificationPanel;
    private static WebSocket ws;

    private void Awake()
    {
        notificationPanel = GameObject.Find("Canvas/Gameboard/MainPanel/NotificationPanel").transform;
    }

    private void Start()
    {
        //WebSocket ws = new WebSocket("ws://192.168.1.249:8080/GameBehavior"); //laptop
        ws = new WebSocket("ws://" + ServerLobbyBehavior.serverIPAddress + ":8080/GameBehavior"); //ovo racunalo, ip adresa
        //ws = new WebSocket("ws://localhost:8080/GameBehavior"); //ovo racunalo
        ws.OnClose += Ws_OnClose;
        ws.OnMessage += Ws_OnMessage;
        ws.OnError += Ws_OnError;
        ws.Connect();

        string myNickname = GetMyNickname();
        string opponentNickname = GetOpponentNickname();

        ws.Send("startGame|" + myNickname + ";" + opponentNickname);
    }

    public new static void SendMessage(string message)
    {
        ws.Send(message);
    }

    private void Ws_OnMessage(object sender, MessageEventArgs e)
    {
        Debug.Log("GameBehavior: " + e.Data);

        string[] message = e.Data.Split('|');

        switch (message[0])
        {
            case "startedGame":
                Debug.Log("Game Started!");
                break;
            case "canStart":
                Dispatcher.Current.BeginInvoke(CanStartStatement);
                break;
            case "playerOnTurn":
                Dispatcher.Current.BeginInvoke(() => { PlayerOnTurnStatement(message[1]); });
                break;
            case "unexpectedEnd":
                Dispatcher.Current.BeginInvoke(UnexpectedEndStatement);
                break;
            case "opponentDrawed":
                Dispatcher.Current.BeginInvoke(() => { OpponentDrawedStatement(message[1]); });
                break;
            case "opponentPlayed":
                Dispatcher.Current.BeginInvoke(() => { OpponentPlayedStatement(message[1]); });
                break;
        }
    }

    private void Ws_OnError(object sender, ErrorEventArgs e)
    {
        Debug.Log("Error: " + e.Message);
    }

    private void Ws_OnClose(object sender, CloseEventArgs e)
    {
        Debug.Log("Close: " + e.Reason);
    }

    private void CanStartStatement()
    {
        OnlinePlayer.instance.FillHand();
    }

    private void PlayerOnTurnStatement(string nicknamePlaying)
    {
       TurnsManager.instance.PlayerOnTurn(nicknamePlaying);
    }

    private void OpponentPlayedStatement(string staticId)
    {
        OnlineOpponent.instance.PlayOpponentCard(staticId);
    }

    private void OpponentDrawedStatement(string staticId)
    {
        OnlineOpponent.instance.DrawOpponentCard(staticId);
    }

    private void UnexpectedEndStatement()
    {
        GameMenuManager.instance.EndGame('a', 'b');
        CloseWebSocket();
    }

    private string GetMyNickname()
    {
        string nickname = notificationPanel.Find("A_PlayerName").GetComponentInChildren<Text>().text;
        return nickname;
    }

    private string GetOpponentNickname()
    {
        string nickname = notificationPanel.Find("B_PlayerName").GetComponentInChildren<Text>().text;
        return nickname;
    }

    private void OnDestroy()
    {
        CloseWebSocket();
    }

    public void CloseWebSocket()
    {
        if (ws != null && ws.IsAlive)
        {
            ws.Close();
        }
        ServerLobbyBehavior.CloseWebSocket();
    }
}