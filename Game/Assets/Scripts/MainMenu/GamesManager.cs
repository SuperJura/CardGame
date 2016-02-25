using UnityEngine;
using UnityEngine.SceneManagement;
using WebSocketSharp;

public class GamesManager : MonoBehaviour {

    public delegate void OnServerConnectionOpenedHandler();
    public event OnServerConnectionOpenedHandler OnServerConnectionOpened;

    public void LoadCoopGame()
    {
        SceneManager.LoadScene("CoopGame");
    }

    public void LoadBotGame()
    {
        SceneManager.LoadScene("BotGame");
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void ConnectToServer()
    {
        //WebSocket ws = new WebSocket("ws://192.168.1.249:8080/GameBehavior"); //laptop
        WebSocket ws = new WebSocket("ws://localhost:8080/GameBehavior"); //ovo racunalo
        ws.OnOpen += ws_OnOpen;
        ws.OnError += ws_OnError;
        ws.OnClose += Ws_OnClose;
        ws.OnMessage += Ws_OnMessage;

        ws.Connect();
    }

    private void Ws_OnMessage(object sender, MessageEventArgs e)
    {
        Debug.Log(e.Data);
    }

    private void Ws_OnClose(object sender, CloseEventArgs e)
    {
        Debug.Log(e.Reason);
    }

    private void ws_OnError(object sender, ErrorEventArgs e)
    {
        Debug.Log(e.Exception);
    }

    private void ws_OnOpen(object sender, System.EventArgs e)
    {
        Debug.Log("Connection established");
        OnServerConnectionOpened();
    }
}