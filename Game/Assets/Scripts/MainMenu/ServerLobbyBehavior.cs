using System;
using UnityEngine;
using UnityEngine.UI;
using WebSocketSharp;

internal class ServerLobbyBehavior : MonoBehaviour
{
    public static string serverIPAddress;
    public static ServerLobbyBehavior instance;

    private static WebSocket ws;
    private string nickname;

    void Awake()
    {
        instance = this;
    }

    public void ConnectToServer(string nick, string IPAddress)
    {
        if (nick == "")
        {
            DisplayError(2);
            return;
        }
        if (nick.Contains(";"))
        {
            DisplayError(3);
            return;
        }
        if (IPAddress.IsNullOrEmpty()) serverIPAddress = "192.168.1.247";
        else serverIPAddress = IPAddress;
        nickname = nick;

        //WebSocket ws = new WebSocket("ws://192.168.1.249:8080/LobbyBehavior"); //laptop
        ws = new WebSocket("ws://" + serverIPAddress + ":8080/LobbyBehavior"); //ovo racunalo, ip adresa
        //ws = new WebSocket("ws://93.138.64.118:8080/LobbyBehavior"); //ovo racunalo, ip adresa koja nije iz NAT tablice
        //ws = new WebSocket("ws://localhost:8080/LobbyBehavior"); //ovo racunalo
        ws.OnOpen += Ws_OnOpen;
        ws.OnError += Ws_OnError;
        ws.OnClose += Ws_OnClose;
        ws.OnMessage += Ws_OnMessage;

        ws.Connect();
    }

    public void SendWantToPlay()
    {
        ws.Send("wantToPlay|");
    }

    private void Ws_OnMessage(object sender, MessageEventArgs e)
    {
        Debug.Log("Lobby Behavior: " + e.Data);
        string[] message = e.Data.Split('|');

        switch (message[0])
        {
            case "joined":
                nickname = message[1];
                Dispatcher.Current.BeginInvoke(() => { DisplayPlayerJoined(nickname); });
                break;
            case "list":
                Dispatcher.Current.BeginInvoke(() => { DisplayPlayerList(message[1]); });
                break;
            case "playing":
                Dispatcher.Current.BeginInvoke(() => { StartOnlineGame(message[1]); });
                break;
            case "error":
                Dispatcher.Current.BeginInvoke(() => { DisplayError(int.Parse(message[1])); });
                break;
        }
    }

    private void Ws_OnClose(object sender, CloseEventArgs e)
    {
        if (e.Code == 1006) //ako je code 1006 znaci da se server uopce ne javlja
        {
            Dispatcher.Current.BeginInvoke(() => { DisplayError(4); });
        }
        Debug.Log("Close: " + e.Reason);
    }

    private void Ws_OnError(object sender, ErrorEventArgs e)
    {
        Debug.Log("Error: " + e.Exception);
    }

    private void Ws_OnOpen(object sender, EventArgs e)
    {
        ws.Send("join|" + nickname);
    }

    private void DisplayPlayerList(string playerList)
    {
        string[] nicks = playerList.Split(';');
        MainMenuManager.instance.OnlineMenu_DisplayPlayerList(nicks);
    }

    private void DisplayPlayerJoined(string nick)
    {
        MainMenuManager.instance.OnlineMenu_DisplayPlayerJoined(nick);
    }

    private void DisplayError(int errorCode)
    {
        MainMenuManager.instance.OnlineMenu_DisplayError(errorCode);
    }

    public void StartOnlineGame(string opponent)
    {
        MainMenuManager.instance.OnlineMenu_StartOnlineGame(opponent);
    }

    public static void CloseWebSocket()
    {
        if (ws != null && ws.IsAlive)
        {
            ws.Close();
        }
    }

    private void OnApplicationExit()
    {
        CloseWebSocket();
    }
}