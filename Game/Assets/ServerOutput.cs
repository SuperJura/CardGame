using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ServerOutput : MonoBehaviour {

    private OnlineGameManager onlineGameManager;
    private Button btnStartOnlineGame;
    private Button btnConnectToServer;
    private Transform transformPlayerList;

    void Start () {
        onlineGameManager = GameObject.Find("GameManager").GetComponent<OnlineGameManager>();

        onlineGameManager.OnServerConnectionOpened += EnableOnlineGame_OnServerConnectionOpened;
        onlineGameManager.OnPlayerJoined += OnlineGameManager_OnPlayerJoined;

        btnStartOnlineGame = transform.Find("StartOnlineGame").GetComponent<Button>();
        btnConnectToServer = transform.Find("ConnectToServer").GetComponent<Button>();
        transformPlayerList = transform.Find("CurrentPlayers/ListOfPlayers");
    }

    private void OnlineGameManager_OnPlayerJoined(string nick)
    {
        GameObject go = (GameObject)Resources.Load("MainMenuResources/PlayerListItem");
        RectTransform prefab = (RectTransform)Instantiate(go.transform);
        prefab.GetComponent<Text>().text = nick;

        prefab.SetParent(transformPlayerList);
        prefab.transform.localScale = new Vector3(1, 1, 1);
    }

    private void EnableOnlineGame_OnServerConnectionOpened()
    {
        btnStartOnlineGame.interactable = true;
        btnConnectToServer.interactable = false;
    }
}