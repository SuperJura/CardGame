using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIManager : MonoBehaviour
{
    public static GUIManager instance;

    private Text CardDescription;
    private Transform gameboardPanel;
    private RectTransform infoPanel;
    private RectTransform messages;
    private RectTransform notificationPanel;
    private Dictionary<char, string> playerCharName; //char = a/b; string = ime a/b

    private void Awake()
    {
        instance = this;

        transform.GetComponent<TurnsManager>().OnEndTurn += GUIManager_OnEndTurn;

        gameboardPanel = GameObject.Find("Canvas/Gameboard/MainPanel").transform;
        infoPanel = gameboardPanel.Find("InfoPanel").GetComponent<RectTransform>();
        CardDescription = infoPanel.Find("CardDescription/Text").GetComponent<Text>();
        notificationPanel = gameboardPanel.Find("NotificationPanel").GetComponent<RectTransform>();
        messages = notificationPanel.Find("MessagePanel/Messages").GetComponent<RectTransform>();

        string aName = gameboardPanel.Find("A_PlayerSide").GetComponent<BasePlayer>().playerName;
        string bName = gameboardPanel.Find("B_PlayerSide").GetComponent<BasePlayer>().playerName;

        notificationPanel.Find("A_PlayerName/NameText").GetComponentInChildren<Text>().text = aName;
        notificationPanel.Find("B_PlayerName/NameText").GetComponentInChildren<Text>().text = bName;

        playerCharName = new Dictionary<char, string>(2) {{'a', aName}, {'b', bName}};
    }

    public void MakeNotification(char player, string message)
    {
        string msgToWrite = playerCharName[player] + ": " + message + "\n\r";

        GameObject newGO = (GameObject) Resources.Load("GameResources/Notification");
        RectTransform notification = (RectTransform) Instantiate(newGO.transform);
        notification.SetParent(messages);

        Text messageText = notification.GetComponentInChildren<Text>();
        messageText.text = msgToWrite;

        switch (player)
        {
            case 'a':
                messageText.color = Color.blue;
                break;
            case 'b':
                messageText.color = Color.red;
                break;
        }
        messageText.alignment = TextAnchor.UpperCenter;
        notification.transform.localScale = new Vector3(1, 1, 1);
    }

    public void UpdateHealthValues(PlayerLoseHealthEventArgs args)
    {
        switch (args.PlayerPosition)
        {
            case 'a':
                notificationPanel.Find("A_PlayerHealth/Text").GetComponentInChildren<Text>().text =
                    args.CurrentHealth.ToString();
                break;
            case 'b':
                notificationPanel.Find("B_PlayerHealth/Text").GetComponentInChildren<Text>().text =
                    args.CurrentHealth.ToString();
                break;
        }
    }

    private void GUIManager_OnEndTurn(EndTurnEventArgs args)
    {
        infoPanel.Find("CurrentTurnText").GetComponentInChildren<Text>().text = args.TurnNumber.ToString();
        RotateCurrentTurnImage(args.NextPlayerChar);
        infoPanel.Find("PlayerPlayingText").GetComponentInChildren<Text>().text = ">" + args.NextPlayer;
    }

    //poziva se kada igrac prede misem preko karte
    public void DisplayCardHoverDetails(RectTransform card)
    {
        string staticId = card.Find("CardStaticID").GetComponent<Text>().text;
        string cardFlavour = Repository.GetCardDatabaseInstance().GetCard(staticId).CardFlavour;

        CardDescription.text = cardFlavour;
    }

    private void RotateCurrentTurnImage(char player)
    {
        Quaternion current = infoPanel.Find("CurrentTurnImage").localRotation;
        switch (player)
        {
            case 'a':
                current.eulerAngles = new Vector3(0, 0, 230);
                infoPanel.Find("CurrentTurnImage").localRotation = current;
                break;
            case 'b':
                current.eulerAngles = new Vector3(0, 0, 300);
                infoPanel.Find("CurrentTurnImage").localRotation = current;
                break;
        }
    }
}