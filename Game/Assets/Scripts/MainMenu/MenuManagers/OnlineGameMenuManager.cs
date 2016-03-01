using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class OnlineGameMenuManager : MonoBehaviour {

    private OnlineGameManager onlineGameManager;
    private Button btnStartOnlineGame;
    private Button btnConnectToServer;
    private Transform transformPlayerList;
    private Text errorText;
    private GameObject waitingBar;

    void Start () {
        onlineGameManager = GameObject.Find("GameManager").GetComponent<OnlineGameManager>();

        onlineGameManager.OnReceivePlayerList += OnlineGameManager_OnReceivePlayerList;
        onlineGameManager.OnPlayerJoined += OnlineGameManager_OnPlayerJoined;
        onlineGameManager.OnServerError += OnlineGameManager_OnServerError;

        btnStartOnlineGame = transform.Find("StartOnlineGame").GetComponent<Button>();
        btnConnectToServer = transform.Find("ConnectToServer").GetComponent<Button>();
        transformPlayerList = transform.Find("CurrentPlayers/ListOfPlayers");
        errorText = transform.Find("ErrorText").GetComponent<Text>();
        waitingBar = transform.Find("WaitingBar").gameObject;
    }

    void OnDestroy()
    {
        onlineGameManager.OnReceivePlayerList -= OnlineGameManager_OnReceivePlayerList;
        onlineGameManager.OnPlayerJoined -= OnlineGameManager_OnPlayerJoined;
        onlineGameManager.OnServerError -= OnlineGameManager_OnServerError;
    }

    private void OnlineGameManager_OnReceivePlayerList(string[] playerList)
    {
        foreach (Transform child in transformPlayerList)
        {
            Debug.Log(child.name);
            Destroy(child.gameObject);
        }

        foreach (string nick in playerList)
        {
            GameObject go = (GameObject)Resources.Load("MainMenuResources/PlayerListItem");
            RectTransform prefab = (RectTransform)Instantiate(go.transform);
            prefab.GetComponent<Text>().text = nick;

            prefab.SetParent(transformPlayerList);
            prefab.transform.localScale = new Vector3(1, 1, 1);
        }
    }

    private void OnlineGameManager_OnPlayerJoined(string nick)
    {
        btnStartOnlineGame.interactable = true;
        btnConnectToServer.interactable = false;
    }

    private void OnlineGameManager_OnServerError(int errorCode)
    {
        switch (errorCode)
        {
            case 1:
                StartCoroutine(DisplayError("Name is taken"));
                break;
            case 2:
                StartCoroutine(DisplayError("Name cannot be empty"));
                break;
            case 3:
                StartCoroutine(DisplayError("Name cannot contain ;"));
                break;
            case 4:
                StartCoroutine(DisplayError("Error while connecting"));
                break;
        }
    }

    private IEnumerator DisplayError(string error)
    {
        errorText.text = error;
        errorText.enabled = true;
        btnConnectToServer.interactable = true;
        btnStartOnlineGame.interactable = false;
        yield return new WaitForSeconds(5);
        errorText.enabled = false;
    }

    public void StartWaiting()
    {
        waitingBar.SetActive(true);
        btnStartOnlineGame.enabled = false;
        btnStartOnlineGame.GetComponentInChildren<Text>().text = "Waiting for opponent";
    }
}