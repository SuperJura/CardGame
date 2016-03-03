using UnityEngine;
using System.Collections;
using WebSocketSharp.Net;
using WebSocketSharp;
using System;
using UnityEngine.UI;

public class ServerGameBehavior : MonoBehaviour {

    private WebSocket ws;
    private Transform notificationPanel;
    private EndGameManager endGameManager;

    void Awake()
    {
        notificationPanel = GameObject.Find("Canvas/Gameboard/MainPanel/NotificationPanel").transform;
        endGameManager = GameObject.Find("Canvas/EndGameMenu").GetComponent<EndGameManager>();
    }

    void Start ()
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

    private void Ws_OnMessage(object sender, MessageEventArgs e)
    {
        Debug.Log("GameBehavior: " + e.Data);

        string[] message = e.Data.Split('|');

        switch (message[0])
        {
            case "startedGame":
                Debug.Log("Game Started!");
                break;
            case "unexpectedEnd":
                Dispatcher.Current.BeginInvoke(() => { UnexpectedEndStatement(); });
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

    void OnApplicationQuit()
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
