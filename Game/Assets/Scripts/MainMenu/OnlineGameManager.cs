using System;
using UnityEngine;
using UnityEngine.UI;
using WebSocketSharp;

class OnlineGameManager : MonoBehaviour
{
    public Text txtNick;

    public delegate void OnReceivePlayerListHandler(string[] playerList);
    public event OnReceivePlayerListHandler OnReceivePlayerList;

    public delegate void OnPlayerJoinedHandler(string nick);
    public event OnPlayerJoinedHandler OnPlayerJoined;

    public delegate void OnServerErrorHandler(int errorCode);
    public event OnServerErrorHandler OnServerError;

    private WebSocket ws;

    public void ConnectToServer()
    {
        //WebSocket ws = new WebSocket("ws://192.168.1.249:8080/GameBehavior"); //laptop
        ws = new WebSocket("ws://192.168.1.247:8080/GameBehavior"); //ovo racunalo, ip adresa
        //ws = new WebSocket("ws://localhost:8080/GameBehavior"); //ovo racunalo
        ws.OnOpen += ws_OnOpen;
        ws.OnError += ws_OnError;
        ws.OnClose += Ws_OnClose;
        ws.OnMessage += Ws_OnMessage;

        ws.Connect();
    }

    private void Ws_OnMessage(object sender, MessageEventArgs e)
    {
        Debug.Log(e.Data);
        string[] message = e.Data.Split('|');

        switch (message[0])
        {
            case "joined":
                Dispatcher.Current.BeginInvoke(() => { CallOnPlayerJoined(message[1]); });
                break;
            case "list":
                Dispatcher.Current.BeginInvoke(() => { CallOnReceivePlayerList(message[1]); });
                break;
            case "error":
                Dispatcher.Current.BeginInvoke(() => { CallOnServerError(message[1]); });
                break;
        }
    }

    private void Ws_OnClose(object sender, CloseEventArgs e)
    {
        Debug.Log("Close: " + e.Reason);
    }

    private void ws_OnError(object sender, ErrorEventArgs e)
    {
        Debug.Log("Error: " + e.Exception);
    }

    private void ws_OnOpen(object sender, EventArgs e)
    {
        Debug.Log("Connection established");
        ws.Send("join|" + txtNick.text);
    }

    void OnApplicationQuit()
    {
        if (ws != null && ws.IsAlive)
        {
            ws.Close();
        }
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

}