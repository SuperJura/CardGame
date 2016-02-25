using System;
using UnityEngine;
using UnityEngine.UI;
using WebSocketSharp;

class OnlineGameManager : MonoBehaviour
{
    public Text txtNick;

    public delegate void OnServerConnectionOpenedHandler();
    public event OnServerConnectionOpenedHandler OnServerConnectionOpened;

    public delegate void OnPlayerJoinedHandler(string nick);
    public event OnPlayerJoinedHandler OnPlayerJoined;

    private WebSocket ws;

    public void ConnectToServer()
    {
        //WebSocket ws = new WebSocket("ws://192.168.1.249:8080/GameBehavior"); //laptop
        ws = new WebSocket("ws://localhost:8080/GameBehavior"); //ovo racunalo
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
        if (message[0] == "joined")
        {
            Dispatcher.Current.BeginInvoke(() => { CallOnPlayerJoined(message[1]); });
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

    private void ws_OnOpen(object sender, System.EventArgs e)
    {
        Debug.Log("Connection established");
        OnServerConnectionOpened();
        ws.Send("join|" + txtNick.text);
    }

    private void CallOnPlayerJoined(string nick)
    {
        if (OnPlayerJoined != null)
        {
            OnPlayerJoined(nick);
        }
    }
}