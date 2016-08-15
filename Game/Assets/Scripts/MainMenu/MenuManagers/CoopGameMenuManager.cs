using UnityEngine;
using UnityEngine.UI;

public class CoopGameMenuManager : MonoBehaviour
{
    public Text nicknameTextA;
    public Text nicknameTextB;

    public void EnterNicknames()
    {
        if(!string.IsNullOrEmpty(nicknameTextA.text))   PlayerNamesForGame.NicknameForCoopGameA = nicknameTextA.text;
        if(!string.IsNullOrEmpty(nicknameTextB.text))   PlayerNamesForGame.NicknameForCoopGameB = nicknameTextB.text;
    }
}