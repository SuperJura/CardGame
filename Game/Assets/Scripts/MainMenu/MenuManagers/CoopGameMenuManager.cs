using UnityEngine;
using UnityEngine.UI;

public class CoopGameMenuManager : MonoBehaviour {

    public Text nicknameTextA;
    public Text nicknameTextB;

    public void EnterNicknames()
    {
        PlayerNamesForGame.NicknameForCoopGameA = nicknameTextA.text;
        PlayerNamesForGame.NicknameForCoopGameB = nicknameTextB.text;
    }
}
