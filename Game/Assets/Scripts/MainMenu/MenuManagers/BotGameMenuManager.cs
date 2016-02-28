using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BotGameMenuManager : MonoBehaviour {

    public Text nicknameText;

    public void EnterNickname()
    {
        PlayerNamesForGame.NicknameForBotGame = nicknameText.text;
    }
}