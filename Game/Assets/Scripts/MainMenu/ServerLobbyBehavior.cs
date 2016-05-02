using System;
using UnityEngine;
using UnityEngine.UI;
using WebSocketSharp;

internal class ServerLobbyBehavior : MonoBehaviour
{
    public delegate void OnPlayerJoinedHandler(string nick);

    public delegate void OnReceivePlayerListHandler(string[] playerList);

    public delegate void OnServerErrorHandler(int errorCode);

    public delegate void OnStartOnlineGameHandler(string opponent);

    private static WebSocket ws;
    private string nickname;
    public Text txtNick;
    public event OnReceivePlayerListHandler OnReceivePlayerList;
    public event OnPlayerJoinedHandler OnPlayerJoined;
    public event OnStartOnlineGameHandler OnStartOnlineGame;
    public event OnServerErrorHandler OnServerError;

    public void ConnectToServer()
    {
        if (txtNick.text == "")
        {
            CallOnServerError("2");
            return;
        }
        if (txtNick.text.Contains(";"))
        {
            CallOnServerError("3");
            return;
        }
        //WebSocket ws = new WebSocket("ws://192.168.1.249:8080/LobbyBehavior"); //laptop
        ws = new WebSocket("ws://192.168.1.247:8080/LobbyBehavior"); //ovo racunalo, ip adresa
        //ws = new WebSocket("ws://93.138.64.118:8080/LobbyBehavior"); //ovo racunalo, ip adresa koja nije iz NAT tablice
        //ws = new WebSocket("ws://localhost:8080/LobbyBehavior"); //ovo racunalo
        ws.OnOpen += ws_OnOpen;
        ws.OnError += ws_OnError;
        ws.OnClose += Ws_OnClose;
        ws.OnMessage += Ws_OnMessage;

        ws.Connect();
    }

    public void SendWantToPlay()
    {
        ws.Send("wantToPlay|");
    }

    public static void CloseWebSocket()
    {
        if (ws != null && ws.IsAlive)
        {
            ws.Close();
        }
    }

    private void Ws_OnMessage(object sender, MessageEventArgs e)
    {
        Debug.Log("Lobby Behavior: " + e.Data);
        string[] message = e.Data.Split('|');

        switch (message[0])
        {
            case "joined":
                nickname = message[1];
                Dispatcher.Current.BeginInvoke(() => { CallOnPlayerJoined(nickname); });
                break;
            case "list":
                Dispatcher.Current.BeginInvoke(() => { CallOnReceivePlayerList(message[1]); });
                break;
            case "playing":
                Dispatcher.Current.BeginInvoke(() => { CallOnStartOnlineGame(message[1]); });
                break;
            case "error":
                Dispatcher.Current.BeginInvoke(() => { CallOnServerError(message[1]); });
                break;
        }
    }

    private void Ws_OnClose(object sender, CloseEventArgs e)
    {
        if (e.Code == 1006) //ako je code 1006 znaci da se server uopce ne javlja
        {
            Dispatcher.Current.BeginInvoke(() => { CallOnServerError("4"); });
        }
        Debug.Log("Close: " + e.Reason);
    }

    private void ws_OnError(object sender, ErrorEventArgs e)
    {
        Debug.Log("Error: " + e.Exception);
    }

    private void ws_OnOpen(object sender, EventArgs e)
    {
        ws.Send("join|" + txtNick.text);
    }

    private void OnApplicationQuit()
    {
        CloseWebSocket();
    }

    private void CallOnReceivePlayerList(string playerList)
    {
        string[] nicks = playerList.Split(';');
        if (OnReceivePlayerList != null)
        {
            OnReceivePlayerList(nicks);
        }
    }

    private void CallOnPlayerJoined(string nick)
    {
        if (OnPlayerJoined != null)
        {
            OnPlayerJoined(nick);
        }
    }

    private void CallOnServerError(string errorCode)
    {
        if (OnServerError != null)
        {
            OnServerError(int.Parse(errorCode));
        }
    }

    public void CallOnStartOnlineGame(string opponent)
    {
        if (OnStartOnlineGame != null)
        {
            OnStartOnlineGame(opponent);
        }
    }
}