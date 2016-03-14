using UnityEngine;
using System.Collections;
using WebSocketSharp.Net;
using WebSocketSharp;
using System;
using UnityEngine.UI;

public class ServerGameBehavior : MonoBehaviour {

    public delegate void OnCanStartHandler();
    public event OnCanStartHandler OnCanStart;

    public delegate void OnOpponentDrawedHandler(string staticID);
    public event OnOpponentDrawedHandler OnOpponentDrawed;

    public delegate void OnOpponentPlayedHandler(string staticID);
    public event OnOpponentPlayedHandler OnOpponentPlayed;

    private WebSocket ws;
    private Transform notificationPanel;
    private EndGameManager endGameManager;
    private TurnsManager turnsManager;

    void Awake()
    {
        notificationPanel = GameObject.Find("Canvas/Gameboard/MainPanel/NotificationPanel").transform;
        endGameManager = GameObject.Find("Canvas/EndGameMenu").GetComponent<EndGameManager>();
        turnsManager = transform.GetComponent<TurnsManager>();
    }

    void Start()
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
    public void CardDrawn(string staticID)
    {
        ws.Send("cardDrawed|" + staticID);
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
                Dispatcher.Current.BeginInvoke(() => { CanStartStatement(); });
                break;
            case "unexpectedEnd":
                Dispatcher.Current.BeginInvoke(() => { UnexpectedEndStatement(); });
                break;
            case "playerOnTurn":
                Dispatcher.Current.BeginInvoke(() => { PlayerOnTurnStatement(message[1]); });
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

    private void OpponentPlayedStatement(string staticID)
    {
        if (OnOpponentPlayed != null)
        {
            OnOpponentPlayed(staticID);
        }
    }

    private void OpponentDrawedStatement(string staticID)
    {
        if (OnOpponentDrawed != null)
        {
            OnOpponentDrawed(staticID);
        }
    }

    private void UnexpectedEndStatement()
    {
        endGameManager.EndGame('a', 'b');
        ServerLobbyBehavior.CloseWebSocket();
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
    }

}
