using UnityEngine;
using UnityEngine.UI;
using WebSocketSharp;

public class BotGameMenuManager : MonoBehaviour
{
    public Text nicknameText;

    public void EnterNickname()
    {
        if (!string.IsNullOrEmpty(nicknameText.text))    PlayerNamesForGame.NicknameForBotGame = nicknameText.text;
    }
}