using UnityEngine;
using UnityEngine.UI;
using WebSocketSharp;

public class ServerGameBehavior : MonoBehaviour
{
    public delegate void OnCanStartHandler();

    public delegate void OnOpponentDrawedHandler(string staticId);

    public delegate void OnOpponentPlayedHandler(string staticId);

    private EndGameManager endGameManager;
    private Transform notificationPanel;
    private TurnsManager turnsManager;

    private WebSocket ws;
    public event OnCanStartHandler OnCanStart;
    public event OnOpponentDrawedHandler OnOpponentDrawed;
    public event OnOpponentPlayedHandler OnOpponentPlayed;

    private void Awake()
    {
        notificationPanel = GameObject.Find("Canvas/Gameboard/MainPanel/NotificationPanel").transform;
        endGameManager = GameObject.Find("Canvas/EndGameMenu").GetComponent<EndGameManager>();
        turnsManager = transform.GetComponent<TurnsManager>();
    }

    private void Start()
    {
        //WebSocket ws = new WebSocket("ws://192.168.1.249:8080/GameBehavior"); //laptop
        ws = new WebSocket("ws://192.168.1.247:8080/GameBehavior"); //ovo racunalo, ip adresa
        //ws = new WebSocket("ws://localhost:8080/GameBehavior"); //ovo racunalo
        ws.OnClose += Ws_OnClose;
        ws.OnMessage += Ws_OnMessage;
        ws.OnError += Ws_OnError;
        ws.Connect();

        string myNickname = GetMyNickname();
        string opponentNickname = GetOpponentNickname();

        ws.Send("startGame|" + myNickname + ";" + opponentNickname);
    }

    //metoda se poziva svaki put kada IGRAC odigra kartu (postavljeno u OnlinePlayer)
    public void Card_OnCardPickTurnEnd(RectTransform card)
    {
        string staticId = card.Find("CardStaticID").GetComponent<Text>().text;
        ws.Send("cardPlayed|" + staticId);
    }

    //metoda se poziva svaki put kada IGRAC povuce kartu
    public void CardDrawn(string staticId)
    {
        ws.Send("cardDrawed|" + staticId);
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
        if (OnCanStart != null)
        {
            OnCanStart();
        }
    }

    private void PlayerOnTurnStatement(string nicknamePlaying)
    {
        turnsManager.PlayerOnTurn(nicknamePlaying);
    }

    private void OpponentPlayedStatement(string staticId)
    {
        if (OnOpponentPlayed != null)
        {
            OnOpponentPlayed(staticId);
        }
    }

    private void OpponentDrawedStatement(string staticId)
    {
        if (OnOpponentDrawed != null)
        {
            OnOpponentDrawed(staticId);
        }
    }

    private void UnexpectedEndStatement()
    {
        endGameManager.EndGame('a', 'b');
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

    private void OnApplicationQuit()
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